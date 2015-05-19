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

        enum DataTransferProtocol
        {
            DTP_TCP,
            DTP_UDP,
            DTP_OTHER
        }

        public DataTransferProtocol Protocol { get; set; }

        public IPAddress DestIP { get; set; }

        public IPAddress SourceIP { get; set; }

        public string DestMAC { get; set; }

        public string SourceMAC { get; set; }

        public Byte[] Data { get; set; }

        public DateTime Time { get; set; }

        public int Length { get; set; }

    }
}
