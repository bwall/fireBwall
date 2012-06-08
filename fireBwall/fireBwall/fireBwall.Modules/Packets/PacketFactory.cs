using System;
using System.Collections.Generic;
using System.Text;
using fireBwall.Filters.NDIS;
using fireBwall.Utils;

namespace fireBwall.Packets
{
    public static class PacketFactory
    {
        public static TCPPacket MakeSynPacket(byte[] fromMac, byte[] toMac, byte[] fromIP, byte[] toIP, ushort fromPort, ushort toPort)
        {
            EthPacket e = new EthPacket(60);
            e.FromMac = fromMac;
            e.ToMac = toMac;
            e.Proto = new byte[2] { 0x08, 0x00 };
            IPPacket ip = new IPPacket(e);
            ip.DestIP = new IPAddr(fromIP);
            ip.SourceIP = new IPAddr(toIP);
            ip.NextProtocol = 0x06;
            ip.TotalLength = 40;
            ip.HeaderChecksum = ip.GenerateIPChecksum;
            TCPPacket tcp = new TCPPacket(ip);
            tcp.SourcePort = fromPort;
            tcp.DestPort = toPort;
            tcp.SequenceNumber = (uint)new Random().Next();
            tcp.AckNumber = 0;
            tcp.WindowSize = 8192;
            tcp.SYN = true;
            tcp.Checksum = tcp.GenerateChecksum;
            tcp.Outbound = true;
            return tcp;
        }

        public static TCPPacket MakeSynPacket(INDISFilter fromAdapter, byte[] toMac, byte[] toIP, ushort fromPort, ushort toPort)
        {
            return MakeSynPacket(fromAdapter.GetAdapterInformation().InterfaceInformation.GetPhysicalAddress().GetAddressBytes(), toMac, fromAdapter.GetAdapterInformation().IPv4.GetAddressBytes(), toIP, fromPort, toPort);
        }

        public static TCPPacket MakePortClosedPacket(byte[] fromMac, byte[] toMac, byte[] fromIP, byte[] toIP, ushort fromPort, ushort toPort, uint ackNumber)
        {
            EthPacket e = new EthPacket(60);
            e.FromMac = fromMac;
            e.ToMac = toMac;
            e.Proto = new byte[2] { 0x08, 0x00 };
            IPPacket ip = new IPPacket(e);
            ip.DestIP = new IPAddr(fromIP);
            ip.SourceIP = new IPAddr(toIP);
            ip.NextProtocol = 0x06;
            ip.TotalLength = 40;
            ip.HeaderChecksum = ip.GenerateIPChecksum;
            TCPPacket tcp = new TCPPacket(ip);
            tcp.SourcePort = fromPort;
            tcp.DestPort = toPort;
            tcp.SequenceNumber = (uint)0;
            tcp.AckNumber = ackNumber;
            tcp.WindowSize = 8192;
            tcp.ACK = true;
            tcp.RST = true;
            tcp.Checksum = tcp.GenerateChecksum;
            tcp.Outbound = true;
            return tcp;
        }

        public static TCPPacket MakePortClosedPacket(TCPPacket in_packet)
        {
            return MakePortClosedPacket(in_packet.ToMac, in_packet.FromMac, in_packet.DestIP.AddressBytes, in_packet.SourceIP.AddressBytes, in_packet.DestPort, in_packet.SourcePort, in_packet.SequenceNumber);
        }
    }
}
