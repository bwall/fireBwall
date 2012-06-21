using System;
using System.Collections.Generic;
using System.Text;
using fireBwall.Modules;
using fireBwall.Packets;
using fireBwall.Utils;

namespace PortKnocker
{
    public class PortKnockerModule : NDISModule
    {
        public class KnockRule
        {
            public IPAddr triggerIP = new IPAddr();
            public ushort triggerPort = 0;
            public IPAddr knockIP = new IPAddr();
            public ushort knockPort = 0;
        }

        public PortKnockerModule()
            : base()
        {
            ModuleMeta.Meta m = new ModuleMeta.Meta();
            m.Name = "PortKnocker";
            m.Version = "0.0.0.1";
            m.Author = "Brian W.";
            m.Description = "";
            m.Help = "";
            m.Contact = "";
            MetaData = new ModuleMeta(m);
        }

        List<KnockRule> rules = new List<KnockRule>();

        public override bool ModuleStart()
        {
            KnockRule[] temp = Load<KnockRule[]>();
            if (temp == null)
            {
                rules = new List<KnockRule>();
            }
            else
            {
                rules = new List<KnockRule>(temp);
            }
            return true;
        }

        public override bool ModuleStop()
        {
            Save<KnockRule[]>(rules.ToArray());
            return true;
        }

        public override PacketMainReturnType interiorMain(ref Packet in_packet)
        {
            if (in_packet.Outbound && in_packet.GetHighestLayer() == Protocol.TCP)
            {
                TCPPacket tcp = (TCPPacket)in_packet;
                if (tcp.SYN && !tcp.ACK)
                {
                    foreach (KnockRule rule in rules)
                    {
                        if (tcp.DestPort == rule.triggerPort && tcp.DestIP.Equals(rule.triggerIP))
                        {
                            Adapter.SendPacket(PacketFactory.MakeSynPacket(Adapter, tcp.ToMac, rule.knockIP.AddressBytes, tcp.SourcePort, rule.knockPort));
                            break;
                        }
                    }
                }
            }
            return 0;
        }
    }
}
