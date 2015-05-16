using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public string DestIP { get; set; }

        public string SourceIP { get; set; }

        public string DestMAC { get; set; }

        public string SourceMAC { get; set; }

        public Byte[] Data { get; set; }

        public DateTime Time { get; set; }

    }
}
