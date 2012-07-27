using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Net;

namespace fireBwall.Packets
{
    //
    // INTERMEDIATE_BUFFER contains packet buffer, packet NDIS flags, WinpkFilter specific flags
    //
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LIST_ENTRY
    {
        public IntPtr Flink;
        public IntPtr Blink;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct INTERMEDIATE_BUFFER
    {
        public LIST_ENTRY m_qLink;
        public uint m_dwDeviceFlags;
        public uint m_Length;
        public uint m_Flags;
        public fixed byte m_IBuffer[1514];
    }

    /// <summary>
    /// Different protocols that are supported or that will be
    /// </summary>
    public enum Protocol
    {
        EEth,
        Ethernet,
        IP,
        ICMPv6,
        ARP,
        TCP,
        UDP,
        ICMP,
        DNS,
        DHCP,
        SNMP
    }

    /// <summary>
    /// Interface for a network Packet
    /// Packets are upward processing!
    /// </summary>
    public abstract unsafe class Packet
    {
        public const uint PACKET_FLAG_ON_RECEIVE = 0x00000002;
        public const uint PACKET_FLAG_ON_SEND = 0x00000001;

        public abstract bool ContainsLayer(Protocol layer);

        public abstract byte* Data();

        public abstract INTERMEDIATE_BUFFER* IB
        {
            get;
        }

        public abstract Protocol GetHighestLayer();

        public abstract uint Length();

        public abstract uint LayerStart();

        public abstract uint LayerLength();

        public abstract Packet MakeNextLayerPacket();

        public abstract bool CodeGenerated
        {
            get;
            set;
        }

        public abstract void ClearGeneratedPacket();

        public abstract bool Outbound
        {
            get;
            set;
        }

        // time a packet is captured.
        // should be logged with DateTime.UtcNow
        private DateTime packetTime;
        public DateTime PacketTime
        {
            get { return packetTime; }
            set { packetTime = value; }
        }
    }
}
