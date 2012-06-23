using System;
using System.Collections.Generic;
using System.Text;
using fireBwall.Modules;
using fireBwall.Packets;
using fireBwall.Utils;
using fireBwall.Configuration;
using fireBwall.Logging;

namespace PortTrap
{
    public class PortTrapModule : NDISModule
    {
        public PortTrapModule()
            : base()
        {
            ModuleMeta.Meta m = new ModuleMeta.Meta();
            m.Name = "PortTrap";
            m.Version = "0.0.0.1";
            m.Author = "Brian W.";
            m.Description = "This module will block any IP address that tries to connect to specified ports, adding it to a block list.";
            m.Help = "";
            m.Contact = "";
            MetaData = new ModuleMeta(m);
        }

        SerializableList<ushort> Traps = new SerializableList<ushort>();

        public override bool ModuleStart()
        {
            SerializableList<ushort> temp = Load<SerializableList<ushort>>();
            if (temp == null)
            {
                Traps = new SerializableList<ushort>();
            }
            else
            {
                Traps = temp;
            }
            return true;
        }

        public override bool ModuleStop()
        {
            if (Traps != null)
            {
                Save<SerializableList<ushort>>(Traps);
            }
            return true;
        }

        public override PacketMainReturnType interiorMain(ref Packet in_packet)
        {
            if (in_packet.ContainsLayer(Protocol.IP))
            {
                IPPacket ipp = (IPPacket)in_packet;
                if((ipp.Outbound && IPLists.Instance.InList("blacklist", ipp.DestIP)) || (!ipp.Outbound && IPLists.Instance.InList("blacklist", ipp.SourceIP)))
                    return PacketMainReturnType.Drop;
                if (in_packet.ContainsLayer(Protocol.TCP) && !ipp.Outbound)
                {
                    TCPPacket tcp = (TCPPacket)ipp;
                    if(Traps.Contains(tcp.DestPort))
                    {
                        IPLists.Instance.AddToList("blacklist", tcp.SourceIP);
                        LogCenter.Instance.LogEvent(new LogEvent(tcp.SourceIP.ToString() + " tried to access port " + tcp.DestPort.ToString() + " and is now blacklisted", this));
                        return PacketMainReturnType.Drop;
                    }
                }
            }
            return 0;
        }
    }
}
