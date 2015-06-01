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

        /// <summary>
        /// A list of all registered devices
        /// </summary>
        private LibPcapLiveDeviceList deviceList;

        /// <summary>
        /// Event handle to signal the processing thread when a packet arrives.
        /// </summary>
        private EventWaitHandle ewh;
         
        // TODO: Write on disk !!!
        // Create the FileWriterDevice to write to a pcap file.
        //private CaptureFileWriterDevice captureFileWriter;

        /// <summary>
        /// The most recently arrived packet
        /// </summary>
        private DataPacket currentPacket;


		/// <summary>
		/// The BackgroundWorker running the sniffer and reporting the packet to every registered handler 
		/// </summary>
		private BackgroundWorker snifferWorker;

        /// <summary>
        /// Constructor.
        /// </summary>
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
        /// <summary>
        /// Starts the packet sniffer in a new thread.
        /// </summary>
		public void StartSniffer()
		{
            // Sniffer already running?
            if (!snifferWorker.IsBusy)
            {
                snifferWorker.RunWorkerAsync();


                // Capture data on every available adapter in the network.
                foreach (var device in deviceList)
                {

                    // Register the handler function to the packet arrival event.
                    device.OnPacketArrival += new PacketArrivalEventHandler(device_OnPacketArrival);

                    //captureFileWriter = new CaptureFileWriterDevice(device, "test.pcpap");

                    // Open the device for capturing.
                    int readTimeoutMilliseconds = 1000;



                    // Distinction between AirPcap, WinPcap und LibPcap devices.
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
                    // If the type of device is unknown, throw a new exception.
                    else
                    {
                        throw new System.InvalidOperationException("unknown device type of " + device.GetType().ToString());
                    }

                    // Limit the capturing process to IP and TCP packets.
                    string filter = "ip and tcp";
                    device.Filter = filter;

                    // Start the capturing process.
                    device.StartCapture();

                }
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

        /// <summary>
        /// Handles incoming packets and creates a DataPacket object and stores it in currentPacket.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Containes all information attached to the incoming packet</param>
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


        /// <summary>
        /// Stops the sniffer and the associated thread as well as the capturing process on all devices.
        /// </summary>
        public void StopSniffer()
        {
            if (snifferWorker.IsBusy)
            {
                snifferWorker.CancelAsync();
                ewh.Set();
            }
            
            
        }

        /// <summary>
        /// Registers a method handling incoming packets, methods registered before will not be overridden.
        /// When defining the handling method be aware that the progress will always be zero (0).
        /// </summary>
        /// <param name="handler">A ProgressChangedEventHandler reciving a packet on arrivel.</param>
		public void AddPacketHandler(ProgressChangedEventHandler handler)
		{
			snifferWorker.ProgressChanged += handler;
		}
    }

}
