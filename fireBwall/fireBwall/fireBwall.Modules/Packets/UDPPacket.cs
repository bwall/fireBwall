using System;
using System.Collections.Generic;
using System.Text;

namespace fireBwall.Packets
{
    /// <summary>
    /// UDP Packet obj
    /// </summary>
    public unsafe class UDPPacket : IPPacket
    {
        public UDPPacket(INTERMEDIATE_BUFFER* in_packet)
            : base(in_packet)
        {
            if (!isUDP())
                throw new Exception("Not a UDP packet!");
            start = base.LayerStart() + base.LayerLength();
        }

        public UDPPacket(IPPacket eth)
            : base(eth.data)
        {
            if (!isUDP())
                throw new Exception("Not a UDP packet!");
            if (eth.CodeGenerated)
            {
                this.CodeGenerated = true;                
            }
            start = base.LayerStart() + base.LayerLength();
        }

        public override bool ContainsLayer(Protocol layer)
        {
            if (layer == Protocol.UDP)
                return true;
            else
                return base.ContainsLayer(layer);
        }

        public override Protocol GetHighestLayer()
        {
            return Protocol.UDP;
        }

        public override Packet MakeNextLayerPacket()
        {
            if (isDNS())
            {
                return new DNSPacket(data).MakeNextLayerPacket();
            }
            return this;
        }

        uint start = 0;
        uint length = 8;

        public override uint LayerStart()
        {
            return start;
        }

        public override uint LayerLength()
        {
            return 8;
        }

        // check if the UDP packet has an empty header.
        // This is usually the case with port scans.
        public bool isEmpty()
        {
            return ((data->m_IBuffer[start + 8] << 8) == 0x00);
        }

        // check if the packet is a UDP DNS packet
        public bool isDNS()
        {
            return (SourcePort == 53 || DestPort == 53);
        }

        public bool isSNMP()
        {
            // agent listens on 161, manager receives on 162
            return (SourcePort == 161 || SourcePort == 162);
        }

        public ushort DestPort
        {
            get
            {
                return (ushort)((data->m_IBuffer[start + 2] << 8) + data->m_IBuffer[start + 3]);
            }
            set
            {
                data->m_IBuffer[start + 2] = (byte)(value >> 8);
                data->m_IBuffer[start + 3] = (byte)(value & 0xff);
            }
        }

        public ushort UDPChecksum
        {
            get
            {
                return (ushort)((data->m_IBuffer[start + 6] << 8) + data->m_IBuffer[start + 7]);
            }
            set
            {
                data->m_IBuffer[start + 6] = (byte)(value >> 8);
                data->m_IBuffer[start + 7] = (byte)(value & 0xff);
            }
        }

        public byte[] ApplicationLayer_safe
        {
            get
            {
                if (data->m_IBuffer[start + 14] == 0xff && data->m_IBuffer[start + 15] == 0x37)
                    return new byte[0];
                uint dataStart = start + length;
                uint dataEnd = this.Length();

                byte[] d = new byte[dataEnd - dataStart];
                for (uint i = dataStart; i < dataEnd; i++)
                {
                    d[i - dataStart] = data->m_IBuffer[i];
                }
                return d;
            }
            set
            {
                uint dataStart = start + length;
                data->m_Length = dataStart + (uint)value.Length;
                for (int i = 0; i < value.Length; i++)
                {
                    data->m_IBuffer[dataStart + i] = value[i];
                }
            }
        }

        public ushort UDPLength
        {
            get
            {
                return (ushort)((data->m_IBuffer[start + 4] << 8) + data->m_IBuffer[start + 5]);
            }
            set
            {
                data->m_IBuffer[start + 4] = (byte)(value >> 8);
                data->m_IBuffer[start + 5] = (byte)(value & 0xff);
            }
        }

        public ushort SourcePort
        {
            get
            {
                return (ushort)((data->m_IBuffer[start] << 8) + data->m_IBuffer[start + 1]);
            }
            set
            {
                data->m_IBuffer[start] = (byte)(value >> 8);
                data->m_IBuffer[start + 1] = (byte)(value & 0xff);
            }
        }

        // generate the checksum of the UDP packet
        public ushort GenerateUDPChecksum
        {
            get
            {
                return GetUDPChecksum();
            }
        }

        // adaptation from http://www.netfor2.com/udpsum.htm
        private ushort GetUDPChecksum()
        {
            UInt32 sum = 0;

            for (uint i = this.start; i < this.start + 6; i += 2)
            {
                sum += (UInt32)(data->m_IBuffer[i] << 8) | data->m_IBuffer[i + 1];
            }
            for (uint i = this.start + 8; i < TotalLength + base.LayerStart() - 1; i += 2)
            {
                sum += (UInt32)(data->m_IBuffer[i] << 8) | data->m_IBuffer[i + 1];
            }
            if (((TotalLength + base.LayerStart()) % 2) == 1)
            {
                sum += (UInt32)(data->m_IBuffer[TotalLength + base.LayerStart() - 1] << 8);
            }

            // src addr
            byte[] srcB = SourceIP.AddressBytes;
            for (int i = 0; i < 4; i += 2)
            {
                sum += (UInt32)(((srcB[i] << 8) & 0xFF00) | (srcB[i + 1] & 0xFF));
            }

            // dst addr
            byte[] destB = DestIP.AddressBytes;
            for (int i = 0; i < 4; i += 2)
            {
                sum += (UInt32)(((destB[i] << 8) & 0xFF00) | (destB[i + 1] & 0xFF));
            }

            // proto
            sum += 0x00011;

            // length
            sum += ((UInt16)(TotalLength - (base.LayerLength())));

            // 1's compliment
            while ((sum >> 16) != 0)
                sum = ((sum & 0xFFFF) + (sum >> 16));

            sum = ~sum;

            return (ushort)sum;
        }
    }
}
