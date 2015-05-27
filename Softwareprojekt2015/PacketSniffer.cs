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
using System.Windows;

namespace Softwareprojekt2015
{
    public class PacketSniffer
    {

        // Retrieves the list of network devices.
        private LibPcapLiveDeviceList deviceList;

        // EventWaitHandle to let thread react on event.
        private EventWaitHandle ewh;

        // Create the FileWriterDevice to write to a pcap file.
        //private CaptureFileWriterDevice captureFileWriter;

        // the DataPacket which is currently processed
        private DataPacket currentPacket;


		// The BackgroundWorker proessing packets on arrival
		private BackgroundWorker snifferWorker;


        public PacketSniffer()
        {
            // get list of all devices
            deviceList = LibPcapLiveDeviceList.Instance;

			snifferWorker = new BackgroundWorker();

			snifferWorker = new BackgroundWorker();
			snifferWorker.WorkerReportsProgress = true;
			snifferWorker.WorkerSupportsCancellation = true;
			snifferWorker.DoWork += new DoWorkEventHandler(RunPacketSniffer);
			snifferWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCompleted);
            
            // Capture data on every available adapter in the network.
            

            ewh = new EventWaitHandle(false, EventResetMode.AutoReset);

        }

		private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			foreach (var device in deviceList)
			{
				device.StopCapture();
				device.Close();
			}
		}

		public void StartSniffer()
		{
			snifferWorker.RunWorkerAsync();

			foreach (var adapter in deviceList)
			{

				// Register the handler function to the packet arrival event.
				adapter.OnPacketArrival += new PacketArrivalEventHandler(device_OnPacketArrival);

				//captureFileWriter = new CaptureFileWriterDevice(device, "test.pcpap");

				// Open the device for capturing.
				int readTimeoutMilliseconds = 1000;



				// Distinction between AirPcap, WinPcap und LibPcap devices.
				if (adapter is AirPcapDevice)
				{
					var airPcap = adapter as AirPcapDevice;
					airPcap.Open(SharpPcap.WinPcap.OpenFlags.DataTransferUdp, readTimeoutMilliseconds);
				}
				else if (adapter is WinPcapDevice)
				{
					var winPcap = adapter as WinPcapDevice;
					winPcap.Open(SharpPcap.WinPcap.OpenFlags.DataTransferUdp | SharpPcap.WinPcap.OpenFlags.NoCaptureLocal, readTimeoutMilliseconds);
				}
				else if (adapter is LibPcapLiveDevice)
				{
					var livePcapDevice = adapter as LibPcapLiveDevice;
					livePcapDevice.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);
				}
				// If the type of device is unknown, throw a new exception.
				else
				{
					throw new System.InvalidOperationException("unknown device type of " + adapter.GetType().ToString());
				}

				// Limit the capturing process to IP and TCP packets.
				string filter = "ip and tcp";
				adapter.Filter = filter;

				// Start the capturing process.
				adapter.StartCapture();

			}
		}

        private void RunPacketSniffer(object sender, DoWorkEventArgs e)
        {

            // As long as the sniffer doesn't receive a cancel event, it reports the current progress.
            while (!e.Cancel)
            {

                ewh.WaitOne();

                if (snifferWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                snifferWorker.ReportProgress(0, currentPacket);

                

            }


        }

        private static int packetIndex = 0;

        // This is the event that is triggered, when a packet arrives.
        // It creates a new DataPacket and assigns data, length, arrival time,
        // destination & source IP address of the packet to the variable currentPacket. 
        private void device_OnPacketArrival(object sender, CaptureEventArgs e)
        {

            //captureFileWriter.Write(e.Packet);
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


        // This method cancels the capturing process and closes the pcap device by calling the KillSniffer() method.
        public void StopSniffer()
        {
            
            snifferWorker.CancelAsync();
            ewh.Set();
            
        }

		public void AddPacketHandler(ProgressChangedEventHandler handler)
		{
			snifferWorker.ProgressChanged += handler;
		}
    }

}
