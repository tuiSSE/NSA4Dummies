using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace NSA4Dummies
{
    /// <summary>
    /// This class represents a packet which was captured by the sniffer includings it's meta data
    /// </summary>
    [Serializable] public class DataPacket
    {

        /// <summary>
        /// Enum to determine the protocol independently from what the snifferlibrary has
        /// </summary>
        public enum DataTransferProtocol
        {
            /// <summary>
            /// Transmission Control Protocol
            /// </summary>
            DTP_TCP,
            /// <summary>
            /// User Datagram Protocol
            /// </summary>
            DTP_UDP,
            /// <summary>
            /// Internet Control Message Protocol
            /// </summary>
            DTP_ICMP,
            /// <summary>
            /// Coud not determine protocol type
            /// </summary>
            DTP_OTHER
        }

        /// <summary>
        /// The protocol used to send the packet
        /// </summary>
        public DataTransferProtocol Protocol { get; set; }


        /// <summary>
        /// Destination IP-Adress of the packet
        /// </summary>
        public IPAddress DestIP { get; set; }

        /// <summary>
        /// Source IP-Adress of the packet
        /// </summary>
        public IPAddress SourceIP { get; set; }

        /// <summary>
        /// The payload of the packet
        /// </summary>
        public Byte[] Data { get; set; }

        /// <summary>
        /// Timestamp of when the packet arrived
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Payload length in bytes
        /// </summary>
        public int Length { get; set; }

    }
}
