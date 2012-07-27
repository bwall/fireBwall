using System;
using System.Collections.Generic;
using System.Text;
using fireBwall.Utils;

namespace fireBwall.Packets
{
    /// <summary>
    /// IPPacket obj
    /// </summary>
    public unsafe class IPPacket : EthPacket
    {     
        public IPPacket(EthPacket eth)
            : base(eth.data)
        {
            if (!isIP() && !isIPv6())
                throw new Exception("Not an IP packet!");
            start = base.LayerStart() + base.LayerLength();
            if (eth.CodeGenerated)
            {
                this.CodeGenerated = true;
                if (isIPv6())
                {
                    data->m_IBuffer[start] = 0x60;
                    data->m_IBuffer[start + 1] = 0x00;
                    this.TTL = 0xff;
                }
                else
                {
                    data->m_IBuffer[start] = 0x45;
                    this.DiffServicesField = 0x00;
                    this.Identification = 23950;
                    this.FragmentOffset = 0;
                    this.Flags = 0x40;
                    this.TTL = 128;
                }                
            }
            if (isIPv6())
                length = 40;
            else
                length = (uint)((data->m_IBuffer[start] & 0xf) * 4);
        }

        public byte TTL
        {
            get
            {
                if (isIPv6())
                {
                    return data->m_IBuffer[start + 7];
                }
                return data->m_IBuffer[start + 8];
            }
            set
            {
                if (isIPv6())
                {
                    data->m_IBuffer[start + 7] = value;
                }
                else
                    data->m_IBuffer[start + 8] = value;
            }
        }

        public byte NextProtocol
        {
            get
            {
                if (isIPv6())
                    return data->m_IBuffer[start + 6];
                else
                    return data->m_IBuffer[start + 9];
            }
            set
            {
                if (isIPv6())
                    data->m_IBuffer[start + 6] = value;
                else
                    data->m_IBuffer[start + 9] = value;
            }
        }

        public ushort HeaderChecksum
        {
            get
            {
                if (isIPv6())
                    return 0;
                return (ushort)((data->m_IBuffer[start + 10] << 8) | data->m_IBuffer[start + 11]);
            }
            set
            {
                if (!isIPv6())
                {
                    data->m_IBuffer[start + 10] = (byte)(value >> 8);
                    data->m_IBuffer[start + 11] = (byte)(value & 0xff);
                }
            }
        }

        public byte Flags
        {
            get
            {
                if (isIPv6())
                    return 0;
                return data->m_IBuffer[start + 6];
            }
            set
            {
                if (!isIPv6())
                {
                    data->m_IBuffer[start + 6] = value;
                }
            }
        }

        public ushort FragmentOffset
        {
            get
            {
                if (!isIPv6())
                {
                    return (ushort)((data->m_IBuffer[start + 6] << 8) | data->m_IBuffer[start + 7]);
                }
                else return 0;
            }
            set
            {
                if (!isIPv6())
                {
                    data->m_IBuffer[start + 6] = (byte)(value >> 8);
                    data->m_IBuffer[start + 7] = (byte)(value & 0xff);
                }
            }
        }

        public ushort Identification
        {
            get
            {
                if (!isIPv6())
                {
                    return (ushort)((data->m_IBuffer[start + 4] << 8) | data->m_IBuffer[start + 5]);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (!isIPv6())
                {
                    data->m_IBuffer[start + 4] = (byte)(value >> 8);
                    data->m_IBuffer[start + 5] = (byte)(value & 0xff);
                }
            }
        }

        public ushort TotalLength
        {
            get
            {
                if (!isIPv6())
                {
                    return (ushort)((data->m_IBuffer[start + 2] << 8) | data->m_IBuffer[start + 3]);
                }
                else
                {
                    return (ushort)((data->m_IBuffer[start + 4] << 8) | data->m_IBuffer[start + 5]);
                }
            }
            set
            {
                if (!isIPv6())
                {
                    data->m_IBuffer[start + 2] = (byte)(value >> 8);
                    data->m_IBuffer[start + 3] = (byte)(value & 0xff);
                }
                else
                {
                    data->m_IBuffer[start + 4] = (byte)(value >> 8);
                    data->m_IBuffer[start + 5] = (byte)(value & 0xff);
                }
            }
        }

        // generate the checksum of the IP packet
        public ushort GenerateIPChecksum
        {
            get
            {
                if (isIPv6())
                    return 0;
                return GetIPChecksum();
            }
        }

        // adaptation from http://www.netfor2.com/ipsum.htm
        private ushort GetIPChecksum()
        {
            UInt32 sum = 0;

            for (uint i = this.start; i < this.start + 10; i += 2)
            {
                sum += (UInt32)(data->m_IBuffer[i] << 8) | data->m_IBuffer[i + 1];
            }
            for (uint i = this.start + 12; i < this.start + this.length; i += 2)
            {
                sum += (UInt32)(data->m_IBuffer[i] << 8) | data->m_IBuffer[i + 1];
            }            

            // 1's compliment
            while ((sum >> 16) != 0)
                sum = ((sum & 0xFFFF) + (sum >> 16));

            sum = ~sum;

            return (ushort)sum;
        }

        public byte DiffServicesField
        {
            get
            {
                return data->m_IBuffer[start + 1];
            }
            set
            {
                data->m_IBuffer[start + 1] = value;
            }
        }

        public byte VersionAndLength
        {
            get
            {
                return data->m_IBuffer[start];
            }
            set
            {
                data->m_IBuffer[start] = value;
            }
        }

        public IPPacket(INTERMEDIATE_BUFFER* in_packet)
            : base(in_packet)
        {
            if (!isIP())
                throw new Exception("Not an IP packet!");
            start = base.LayerStart() + base.LayerLength();
            if (isIPv6())
                length = 40;
            else
                length = (uint)((data->m_IBuffer[start] & 0xf) * 4);
        }

        public override bool ContainsLayer(Protocol layer)
        {
            if (layer == Protocol.IP)
                return true;
            else
                return base.ContainsLayer(layer);
        }

        public override Protocol GetHighestLayer()
        {
            return Protocol.IP;
        }

        public override Packet MakeNextLayerPacket()
        {
            if (isTCP())
            {
                return new TCPPacket(data).MakeNextLayerPacket();
            }
            else if (isUDP())
                return new UDPPacket(data).MakeNextLayerPacket();
            else if (isICMP())
                return new ICMPPacket(data).MakeNextLayerPacket();
            else if (isICMPv6())
                return new ICMPv6Packet(data).MakeNextLayerPacket();
            else
                return this;
        }

        uint start = 0;

        public override uint LayerStart()
        {
            return start;
        }

        uint length = 0;

        public override uint LayerLength()
        {
            return length;
        }

        public bool isTCP()
        {
            return (this.NextProtocol == 0x06);
        }

        public bool isUDP()
        {
            return (this.NextProtocol == 0x11);
        }

        public bool isICMP()
        {
            return (this.NextProtocol == 0x01);
        }

        public bool isICMPv6()
        {
            return (this.NextProtocol == 0x3a);
        }

        public IPAddr DestIP
        {
            get
            {
                if (isIPv6())
                {
                    byte[] ip = new byte[16];
                    for (int x = 0; x < 16; x++)
                    {
                        ip[x] = data->m_IBuffer[start + 0x18 + x];
                    }
                    return new IPAddr(ip);
                }
                else
                {
                    byte[] ip = new byte[4];
                    for (int x = 0; x < 4; x++)
                    {
                        ip[x] = data->m_IBuffer[start + 0x10 + x];
                    }
                    return new IPAddr(ip);
                }
            }
            set
            {
                if (isIPv6())
                {
                    byte[] ip = value.AddressBytes;
                    for (int x = 0; x < 16; x++)
                        data->m_IBuffer[start + 0x18 + x] = ip[x];
                }
                else
                {
                    byte[] ip = value.AddressBytes;
                    for (int x = 0; x < 4; x++)
                    {
                        data->m_IBuffer[start + 0x10 + x] = ip[x];
                    }
                }
            }
        }

        public byte IPVersion
        {
            get
            {
                if (isIPv6()) return 6;
                return (byte)(data->m_IBuffer[start] >> 4);
            }
        }

        public IPAddr SourceIP
        {
            get
            {
                if (isIPv6())
                {
                    byte[] ip = new byte[16];
                    for (int x = 0; x < 16; x++)
                    {
                        ip[x] = data->m_IBuffer[start + 0x8 + x];
                    }
                    return new IPAddr(ip);
                }
                else
                {
                    byte[] ip = new byte[4];
                    for (int x = 0; x < 4; x++)
                    {
                        ip[x] = data->m_IBuffer[start + 0xc + x];
                    }
                    return new IPAddr(ip);
                }                
            }
            set
            {
                if (isIPv6())
                {
                    byte[] ip = value.AddressBytes;
                    for (int x = 0; x < 16; x++)
                        data->m_IBuffer[start + 0x8 + x] = ip[x];
                }
                else
                {
                    byte[] ip = value.AddressBytes;
                    for (int x = 0; x < 4; x++)
                    {
                        data->m_IBuffer[start + 0xc + x] = ip[x];
                    }
                }
            }
        }
    }
}
