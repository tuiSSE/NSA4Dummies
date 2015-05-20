using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Softwareprojekt2015
{
    class DataPacket
    {
        // Enum to determine the data transfer protocol
        public enum DataTransferProtocol
        {
            DTP_TCP,
            DTP_UDP,
            DTP_OTHER
        }

        public DataTransferProtocol Protocol { get; set; }

        // destination IP of the packet
        public IPAddress DestIP { get; set; }

        // source IP of the packet
        public IPAddress SourceIP { get; set; }

        // payload data of the packet
        public Byte[] Data { get; set; }

        // timestamp of when the packet arrived
        public DateTime Time { get; set; }

        // payload length in bytes
        public int Length { get; set; }

    }
}
