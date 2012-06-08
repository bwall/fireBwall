using System;
using System.Collections.Generic;
using System.Text;
using fireBwall.Utils;

namespace fireBwall.Packets
{
    /// <summary>
    /// ARP packet obj
    /// </summary>
    public unsafe class ARPPacket : EthPacket
    {
        public ARPPacket(EthPacket eth)
            : base(eth.data)
        {
            if (!isARP())
                throw new Exception("Not an ARP packet!");
            start = base.LayerStart() + base.LayerLength();
            length = this.Length() - base.LayerLength();
            if (eth.CodeGenerated)
            {
                HardwareType = 1;
                ProtocolType = 0x0800;
                HardwareSize = 0x06;
                ProtocolSize = 4;
                ARPOpcode = 0x0002;
            }
        }

        public ushort ARPOpcode
        {
            get
            {
                return (ushort)((data->m_IBuffer[start + 6] << 8) | data->m_IBuffer[start + 7]);
            }
            set
            {
                data->m_IBuffer[start + 6] = (byte)(value >> 8);
                data->m_IBuffer[start + 7] = (byte)(value & 0xff);
            }
        }

        public byte ProtocolSize
        {
            get
            {
                return data->m_IBuffer[start + 5];
            }
            set
            {
                data->m_IBuffer[start + 5] = value;
            }
        }

        public byte HardwareSize
        {
            get
            {
                return data->m_IBuffer[start + 4];
            }
            set
            {
                data->m_IBuffer[start + 4] = value;
            }
        }

        public ushort ProtocolType
        {
            get
            {
                return (ushort)((data->m_IBuffer[start + 2] << 8) | data->m_IBuffer[start + 3]);
            }
            set
            {
                data->m_IBuffer[start + 2] = (byte)(value >> 8);
                data->m_IBuffer[start + 3] = (byte)(value & 0xff);
            }
        }

        public ushort HardwareType
        {
            get
            {
                return (ushort)((data->m_IBuffer[start] << 8) | data->m_IBuffer[start + 1]);
            }
            set
            {
                data->m_IBuffer[start] = (byte)(value >> 8);
                data->m_IBuffer[start + 1] = (byte)(value & 0xff);
            }
        }

        public ARPPacket(INTERMEDIATE_BUFFER* in_packet)
            : base(in_packet)
        {
            if (!isARP())
                throw new Exception("Not an ARP packet!");
            start = base.LayerStart() + base.LayerLength();
            length = this.Length() - base.LayerLength();
        }

        public override bool ContainsLayer(Protocol layer)
        {
            if (layer == Protocol.ARP)
                return true;
            else
                return base.ContainsLayer(layer);
        }

        public override Protocol GetHighestLayer()
        {
            return Protocol.ARP;
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

        public bool isRequest
        {
            get
            {
                return (data->m_IBuffer[start + 6] == 0x00 && data->m_IBuffer[start + 7] == 0x01);
            }
            set
            {
                if (value)
                {
                    data->m_IBuffer[start + 6] = 0x00;
                    data->m_IBuffer[start + 7] = 0x01;
                }
                else
                {
                    data->m_IBuffer[start + 6] = 0x00;
                    data->m_IBuffer[start + 7] = 0x02;
                }
            }
        }

        /// <summary>
        /// Returns sender's IP
        /// 
        /// This IP will be all zeros if it's an ARP probe
        /// </summary>
        public IPAddr ASenderIP
        {
            get
            {
                byte[] ip = new byte[4];
                for (int x = 0; x < 4; x++)
                    ip[x] = data->m_IBuffer[start + 0xe + x];
                return new IPAddr(ip);
            }
            set
            {
                byte[] ip = value.AddressBytes;
                for (int x = 0; x < 4; x++)
                    data->m_IBuffer[start + 0xe + x] = ip[x];
            }
        }

        public byte[] ASenderMac
        {
            get
            {
                byte[] ip = new byte[6];
                for (int x = 0; x < 6; x++)
                    ip[x] = data->m_IBuffer[start + 0x8 + x];
                return ip;
            }
            set
            {
                for (int x = 0; x < 6; x++)
                    data->m_IBuffer[start + 0x8 + x] = value[x];
            }
        }

        public IPAddr ATargetIP
        {
            get
            {
                byte[] ip = new byte[4];
                for (int x = 0; x < 4; x++)
                    ip[x] = data->m_IBuffer[start + 0x18 + x];
                return new IPAddr(ip);
            }
            set
            {
                byte[] ip = value.AddressBytes;
                for (int x = 0; x < 4; x++)
                    data->m_IBuffer[start + 0x18 + x] = ip[x];
            }
        }

        public byte[] ATargetMac
        {
            get
            {
                byte[] ip = new byte[6];
                for (int x = 0; x < 6; x++)
                    ip[x] = data->m_IBuffer[start + 0x12 + x];
                return ip;
            }
            set
            {
                for (int x = 0; x < 6; x++)
                    data->m_IBuffer[start + 0x12 + x] = value[x];
            }
        }
    }
}
