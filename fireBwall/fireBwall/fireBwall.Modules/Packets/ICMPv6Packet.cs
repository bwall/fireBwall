using System;
using System.Collections.Generic;
using System.Text;
using fireBwall.Utils;

namespace fireBwall.Packets
{
    /// <summary>
    /// IPv6 packet obj
    /// </summary>
    public unsafe class ICMPv6Packet : IPPacket
    {
        public ICMPv6Packet(IPPacket eth)
            : base(eth.data)
        {
            if (!isICMPv6())
                throw new Exception("Not an ICMPv6 packet!");
            start = base.LayerStart() + base.LayerLength();
            if (eth.CodeGenerated)
            {
                this.CodeGenerated = true;
            }
            length = base.TotalLength;
        }

        public ICMPv6Packet(INTERMEDIATE_BUFFER* in_packet)
            : base(in_packet)
        {
            if (!isICMPv6())
                throw new Exception("Not an ICMPv6 packet!");
            start = base.LayerStart() + base.LayerLength();
            length = base.TotalLength;
        }

        public override bool ContainsLayer(Protocol layer)
        {
            if (layer == Protocol.ICMPv6)
                return true;
            else
                return base.ContainsLayer(layer);
        }

        public override Protocol GetHighestLayer()
        {
            return Protocol.ICMPv6;
        }

        public override Packet MakeNextLayerPacket()
        {
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

        public byte Type
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

        public byte Code
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

        public ushort ICMPv6Checksum
        {
            get
            {
                return (ushort)((data->m_IBuffer[start + 2] << 8) | (data->m_IBuffer[start + 3]));
            }
            set
            {
                data->m_IBuffer[start + 2] = (byte)(value >> 8);
                data->m_IBuffer[start + 3] = (byte)(value & 0xff);
            }
        }

        public UInt32 Reserved
        {
            get
            {
                return (UInt32)((data->m_IBuffer[start + 4] << 24) | (data->m_IBuffer[start + 5] << 16) | (data->m_IBuffer[start + 6] << 8)  | (data->m_IBuffer[start + 7]));
            }
            set
            {
                data->m_IBuffer[start + 4] = (byte)(value >> 24);
                data->m_IBuffer[start + 5] = (byte)(value >> 16);
                data->m_IBuffer[start + 6] = (byte)(value >> 8);
                data->m_IBuffer[start + 7] = (byte)(value & 0xff);
            }
        }

        public IPAddr Target
        {
            get
            {
                byte[] ip = new byte[16];
                for (int x = 0; x < 16; x++)
                {
                    ip[x] = data->m_IBuffer[start + 0x8 + x];
                }
                return new IPAddr(ip);
            }
            set
            {
                byte[] ip = value.AddressBytes;
                for (int x = 0; x < 16; x++)
                    data->m_IBuffer[start + 0x8 + x] = ip[x];
            }
        }
    }
}
