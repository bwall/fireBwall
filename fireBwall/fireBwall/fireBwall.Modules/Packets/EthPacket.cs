using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace fireBwall.Packets
{
    /// <summary>
    /// Ethernet packet
    /// </summary>
    public unsafe class EthPacket : Packet
    {
        [DllImport("kernel32", EntryPoint = "RtlZeroMemory")]
        public static extern void ZeroMemory(IntPtr Destination, int Length);

        public override bool CodeGenerated
        {
            get { return generated; }
            set { generated = value; }
        }

        public override INTERMEDIATE_BUFFER* IB
        {
            get { return data; }
        }

        bool generated = false;

        public override void ClearGeneratedPacket()
        {
            Marshal.FreeHGlobal((IntPtr)data);
        }

        public EthPacket(int length)
        {
            data = (INTERMEDIATE_BUFFER*)Marshal.AllocHGlobal(Marshal.SizeOf(new INTERMEDIATE_BUFFER()));
            ZeroMemory((IntPtr)data, Marshal.SizeOf(new INTERMEDIATE_BUFFER()));
            data->m_qLink = new LIST_ENTRY();
            data->m_qLink.Blink = IntPtr.Zero;
            data->m_qLink.Flink = IntPtr.Zero;
            data->m_Length = (uint)length;
            data->m_dwDeviceFlags = PACKET_FLAG_ON_SEND;
            data->m_Flags = 0;
            generated = true;
        }

        public EthPacket(INTERMEDIATE_BUFFER* in_packet)
        {
            data = in_packet;
        }
        public INTERMEDIATE_BUFFER* data;

        public override bool ContainsLayer(Protocol layer)
        {
            return (layer == Protocol.Ethernet);
        }

        /// <summary>
        /// Returns the data inside the ethernet frame
        /// </summary>
        /// <returns>byte pointer of frame data</returns>
        public override byte* Data()
        {
            return data->m_IBuffer;
        }

        /// <summary>
        /// Returns this, the highest layer in the ethernet frame
        /// </summary>
        /// <returns>Protocol ethernet</returns>
        public override Protocol GetHighestLayer()
        {
            return Protocol.Ethernet;
        }

        public override uint Length()
        {
            return data->m_Length;
        }

        public override uint LayerStart()
        {
            return 0;
        }

        public override uint LayerLength()
        {
            return 14;
        }

        public override Packet MakeNextLayerPacket()
        {
            if (isIP())
            {
                return new IPPacket(this).MakeNextLayerPacket();
            }
            else if (isARP())
                return new ARPPacket(data);
            else
                return this;
        }

        /// <summary>
        /// Returns whether the packet is outbound or not
        /// </summary>
        public override bool Outbound
        {
            get
            {
                return (data->m_dwDeviceFlags == PACKET_FLAG_ON_SEND);
            }
            set
            {
                if (value)
                {
                    data->m_dwDeviceFlags = PACKET_FLAG_ON_SEND;
                }
                else
                {
                    data->m_dwDeviceFlags = PACKET_FLAG_ON_RECEIVE;
                }
            }
        }

        public bool isARP()
        {
            return (data->m_IBuffer[0x0c] == 0x08 && data->m_IBuffer[0x0d] == 0x06);
        }

        public bool isEETH()
        {
            return (data->m_IBuffer[0x0c] == 0x98 && data->m_IBuffer[0x0d] == 0x09);
        }

        public bool isIP()
        {
            return (data->m_IBuffer[0x0c] == 0x08 && data->m_IBuffer[0x0d] == 0x00) || isIPv6();
        }

        public bool isIPv6()
        {
            return (data->m_IBuffer[0x0c] == 0x86 && data->m_IBuffer[0x0d] == 0xdd);
        }

        public byte[] Proto
        {
            get
            {
                byte[] mac = new byte[2];
                for (int x = 0; x < 2; x++)
                    mac[x] = data->m_IBuffer[x + 12];
                return mac;
            }
            set
            {
                for (int x = 0; x < 2; x++)
                    data->m_IBuffer[12 + x] = value[x];
            }
        }

        public byte[] FromMac
        {
            get
            {
                byte[] mac = new byte[6];
                for (int x = 0; x < 6; x++)
                    mac[x] = data->m_IBuffer[x + 6];
                return mac;
            }
            set
            {
                for (int x = 0; x < 6; x++)
                    data->m_IBuffer[6 + x] = value[x];
            }
        }

        public byte[] ToMac
        {
            get
            {
                byte[] mac = new byte[6];
                for (int x = 0; x < 6; x++)
                    mac[x] = data->m_IBuffer[x];
                return mac;
            }
            set
            {
                for (int x = 0; x < 6; x++)
                    data->m_IBuffer[x] = value[x];
            }
        }
    }
}
