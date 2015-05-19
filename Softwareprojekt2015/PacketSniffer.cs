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

namespace Softwareprojekt2015
{
    class PacketSniffer
    {
        
        public static void Main (string[] args)
        {   
            
        }

        // Create the FileWriterDevice to write to a pcap file.
        private static CaptureFileWriterDevice captureFileWriter;

        public void RunPacketSniffer(object sender, DoWorkEventArgs e)
        {
            // Retrieves the list of network devices.
            var devices = LibPcapLiveDeviceList.Instance;
            
            // Assign a device with an index.
            int i = 0;
            var device = devices[i];

            // Register the handler function to the packet arrival event.
            device.OnPacketArrival += new PacketArrivalEventHandler(device_OnPacketArrival);

            captureFileWriter = new CaptureFileWriterDevice(device, DateTime.Now.ToString());


            foreach(var adapter in devices)
            {
                
                // Open the device for capturing
                int readTimeoutMilliseconds = 1000;

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
        }

        private static int packetIndex = 0;

        private static void deivce_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            captureFileWriter.Write(e.Packet);
            
            if(e.Packet.LinkLayerType == PacketDotNet.LinkLayers.Ethernet)
            {
                var packet = PacketDotNet.Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);
                var ethernetPacket = (PacketDotNet.EthernetPacket)packet;

                packetIndex++;
            }
        }
    }

}
