using System;
using System.Collections.Generic;
using System.Text;

namespace fireBwall.Packets
{
    /// <summary>
    /// ICMP packet obj
    /// </summary>
    public unsafe class ICMPPacket : IPPacket
    {
        // (g|s)et code
        public int Code
        {
            get
            {
                return (data->m_IBuffer[start + 1]);
            }
            set
            {
                data->m_IBuffer[start + 1] = (byte)value;
            }
        }

        // (g|s)et type
        public int Type
        {
            get
            {
                return (data->m_IBuffer[start]);
            }
            set
            {
                data->m_IBuffer[start] = (byte)value;
            }
        }

        // (g|s)et checksum
        public ushort Checksum
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

        // (g|s)et data
        public byte[] ICMPData
        {
            get
            {
                return GetICMPData();
            }
            set
            {
                SetICMPData(value);
            }
        }

        // get icmp data
        private byte[] GetICMPData()
        {
            // data seg starts +8 bytes in
            uint dataStart = start + 8;
            // data seg is approx 32 bytes long
            uint dataEnd = dataStart + 32;

            byte[] d = new byte[dataEnd - dataStart];
            for (uint i = dataStart; i < dataEnd; ++i)
                d[i - dataStart] = data->m_IBuffer[i];
            return d;
        }

        // set icmp data
        private void SetICMPData(byte[] arr)
        {
            // data seg starts +8 bytes in
            uint dataStart = start + 8;
            // data seg is approx 32 bytes long
            uint dataEnd = dataStart + 32;

            for (uint i = dataStart; i < arr.Length; ++i)
                data->m_IBuffer[i] = arr[i - dataStart];
        }

        // accepts intermediate buff, checks if ICMP
        public ICMPPacket(INTERMEDIATE_BUFFER* in_packet)
            : base(in_packet)
        {
            if (!isICMP())
                throw new Exception("Not an ICMP packet!");
            start = base.LayerStart() + base.LayerLength();

        }

        // accepts IPPacket, checks if ICMP
        public ICMPPacket(IPPacket eth)
            : base(eth.data)
        {
            if (!isICMP())
                throw new Exception("Not an ICMP packet!");
            start = base.LayerStart() + base.LayerLength();
        }

        // return if ICMP; else return to base layer to find layer
        public override bool ContainsLayer(Protocol layer)
        {
            if (layer == Protocol.ICMP)
                return true;
            else
                return base.ContainsLayer(layer);
        }

        // return ICMP (highest)
        public override Protocol GetHighestLayer()
        {
            return Protocol.ICMP;
        }

        // return this (highest)
        public override Packet MakeNextLayerPacket()
        {
            return this;
        }

        uint start = 0;

        public override uint LayerStart()
        {
            return start;
        }

        public override uint LayerLength()
        {
            return 8;
        }
    }
}
