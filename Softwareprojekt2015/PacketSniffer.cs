using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpPcap;
using SharpPcap.LibPcap;
using SharpPcap.AirPcap;
using SharpPcap.WinPcap;
using PacketDotNet;
using System.ComponentModel;
using System.Threading;

namespace Softwareprojekt2015
{
    class PacketSniffer
    {

        // Retrieves the list of network devices.
        private LibPcapLiveDeviceList deviceList;

        // EventWaitHandle to let thread react on event
        private EventWaitHandle ewh;

        // Create the FileWriterDevice to write to a pcap file.
        private CaptureFileWriterDevice captureFileWriter;

        DataPacket currentPacket;


        public PacketSniffer()
        {

            deviceList = LibPcapLiveDeviceList.Instance;

            // Assign a device with an index.
            int i = 0;
            var device = deviceList[i];

            // Register the handler function to the packet arrival event.
            device.OnPacketArrival += new PacketArrivalEventHandler(device_OnPacketArrival);

            captureFileWriter = new CaptureFileWriterDevice(device, DateTime.Now.ToString());

            // Open the device for capturing
            int readTimeoutMilliseconds = 1000;

            string filter = "ip and tcp";
            device.Filter = filter;

            foreach (var adapter in deviceList)
            {


                if (device is AirPcapDevice)
                {
                    var airPcap = device as AirPcapDevice;
                    airPcap.Open(SharpPcap.WinPcap.OpenFlags.DataTransferUdp, readTimeoutMilliseconds);
                }
                else if (device is WinPcapDevice)
                {
                    var winPcap = device as WinPcapDevice;
                    winPcap.Open(SharpPcap.WinPcap.OpenFlags.DataTransferUdp | SharpPcap.WinPcap.OpenFlags.NoCaptureLocal, readTimeoutMilliseconds);
                }
                else if (device is LibPcapLiveDevice)
                {
                    var livePcapDevice = device as LibPcapLiveDevice;
                    livePcapDevice.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);
                }
                else
                {
                    throw new System.InvalidOperationException("unknown device type of " + device.GetType().ToString());
                }

                // Start the capturing process
                device.StartCapture();
            }

            ewh = new EventWaitHandle(false, EventResetMode.AutoReset);

        }

        public ~PacketSniffer()
        {
            foreach (var device in deviceList)
            {
                device.StopCapture();
                device.Close();
            }
        }


        public void RunPacketSniffer(object sender, DoWorkEventArgs e)
        {

            while (!e.Cancel)
            {

                ewh.WaitOne();

                ((App)App.Current).snifferWorker.ReportProgress(0, currentPacket);

            }


        }

        private static int packetIndex = 0;

        private void device_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            captureFileWriter.Write(e.Packet);
            currentPacket = new DataPacket();

            currentPacket.Data = e.Packet.Data;
            currentPacket.Length = e.Packet.Data.Length;
            currentPacket.Time = e.Packet.Timeval.Date;

            var packet = PacketDotNet.Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);
            var tcpPacket = PacketDotNet.TcpPacket.GetEncapsulated(packet);
            if (tcpPacket != null)
            {
                

                IpPacket ipPacket = (IpPacket)tcpPacket.ParentPacket;


                currentPacket.DestIP = ipPacket.DestinationAddress;
                currentPacket.SourceIP = ipPacket.SourceAddress;

            }




            ewh.Set();
        }
    }

}
