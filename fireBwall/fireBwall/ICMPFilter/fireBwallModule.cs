using System;
using System.Collections.Generic;
using System.Text;

using fireBwall.Modules;
using fireBwall.Packets;
using fireBwall.Utils;
using fireBwall.Configuration;
using fireBwall.Logging;

namespace ICMPFilter
{
    /// <summary>
    /// ICMP filter module
    /// 
    /// Allows filtering by type/code
    /// </summary>
    public class ICMPFilterModule : NDISModule
    {
        public ICMPFilterModule()
            : base()
        {
            Help();
        }

        // return local user control
        public override fireBwall.UI.DynamicUserControl GetUserInterface()
        {
            return new ICMPFilterDisplay(this) { Dock = System.Windows.Forms.DockStyle.Fill };
        }

        // Action for ModuleStart
        public override bool ModuleStart()
        {
            try
            {
                data = Load<ICMPData>();
                if (data == null)
                    data = new ICMPData();
            }
            catch (Exception e)
            {
                LogCenter.Instance.LogException(e);
            }
            return true;
        }

        // Action for ModuleStop
        public override bool ModuleStop()
        {
            try
            {
                if (data != null)
                {
                    ICMPData d = data;
                    if (!data.Save)
                    {
                        data.RuleTable = new SerializableDictionary<string, List<string>>();
                        data.RuleTablev6 = new SerializableDictionary<string, List<string>>();
                    }
                    Save<ICMPData>(d);
                }
            }
            catch (Exception e)
            {
                LogCenter.Instance.LogException(e);
            }

            return true;
        }

        /// <summary>
        /// Object used to serialize the data we need to persist.
        /// 
        /// The v4 and v6 rule tables need to be split up because of clashing types/codes
        /// </summary>
        [Serializable]
        public class ICMPData
        {
            private SerializableDictionary<string, List<string>> ruleTable = new SerializableDictionary<string, List<string>>();
            public SerializableDictionary<string, List<string>> RuleTable
            { get { return ruleTable; } set { ruleTable = new SerializableDictionary<string, List<string>>(value); } }

            private SerializableDictionary<string, List<string>> ruleTablev6 = new SerializableDictionary<string, List<string>>();
            public SerializableDictionary<string, List<string>> RuleTablev6
            { get { return ruleTablev6; } set { ruleTablev6 = new SerializableDictionary<string, List<string>>(value); } }

            private bool denyIPv4 = false;
            public bool DenyIPv4
            { get { return denyIPv4; } set { denyIPv4 = value; } }

            private bool denyIPv6 = false;
            public bool DenyIPv6
            { get { return denyIPv6; } set { denyIPv6 = value; } }

            private bool denyIPv6NDP = false;
            public bool DenyIPv6NDP
            { get { return denyIPv6NDP; } set { denyIPv6NDP = value; } }

            private bool log = true;
            public bool Log
            { get { return log; } set { log = value; } }

            public bool Save = true;
        }

        public ICMPData data;
        private MultilingualStringManager multistring = new MultilingualStringManager();

        // main routine
        public override PacketMainReturnType interiorMain(ref Packet in_packet)
        {
            LogEvent le;

            // if the packet is ICMPv4
            if (in_packet.GetHighestLayer() == Protocol.ICMP)
            {
                ICMPPacket packet = (ICMPPacket)in_packet;
                // check if the packet is allowed and deny all is false
                if (isAllowed(packet.Type.ToString(), packet.Code.ToString(), 4) &&
                    !data.DenyIPv4)
                {
                    return PacketMainReturnType.Allow;
                }
                // else, log and drop it
                else
                {
                    PacketMainReturnType pmr = PacketMainReturnType.Drop;
                    if (data.Log)
                    {
                        pmr |= PacketMainReturnType.Log;
                        le = new LogEvent(String.Format(multistring.GetString("ICMPv4 was dropped"), packet.SourceIP.ToString(), packet.DestIP.ToString()), this);
                        le.PMR = PacketMainReturnType.Log | PacketMainReturnType.Drop;
                        LogCenter.Instance.LogEvent(le);
                    }
                    return pmr;
                }
            }

            // if the packet is ICMPv6
            if (in_packet.GetHighestLayer() == Protocol.ICMPv6)
            {
                ICMPv6Packet packet = (ICMPv6Packet)in_packet;
                if ((isAllowed(packet.Type.ToString(), packet.Code.ToString(), 6) &&
                    !data.DenyIPv6) && isDeniedNDP(packet))
                {
                    return PacketMainReturnType.Allow;
                }
                else
                {
                    PacketMainReturnType pmr = PacketMainReturnType.Drop;
                    if (data.Log)
                    {
                        pmr |= PacketMainReturnType.Log;
                        le = new LogEvent(String.Format(multistring.GetString("ICMPv6 was dropped"), packet.SourceIP.ToString(), packet.DestIP.ToString()), this);
                        le.PMR = PacketMainReturnType.Log | PacketMainReturnType.Drop;
                    }
                    return pmr;
                }
            }
            return PacketMainReturnType.Allow;
        }

        /*
         * Method used to check whether an ICMP packet should be
         * allowed through.
         * 
         * @param type is the ICMP type
         * @param code is the ICMP code
         * @param version is the IP version to look for the type/code in
         *  Accepts 4 for ipv4 and 6 for ipv6
         */
        private bool isAllowed(string type, string code, int version)
        {
            bool isAllowed = true;

            if (version == 4)
            {
                // if the table contains the type, check if it
                // also contains the code
                if (data.RuleTable.ContainsKey(type))
                {
                    List<string> temp;
                    data.RuleTable.TryGetValue(type, out temp);
                    // invert logic; if found, disallow, if not, allow
                    isAllowed = !(temp.Contains(code));
                }
            }
            else if (version == 6)
            {
                if (data.RuleTablev6.ContainsKey(type))
                {
                    List<string> tmp;
                    data.RuleTablev6.TryGetValue(type, out tmp);
                    isAllowed = !(tmp.Contains(code));
                }

            }

            return isAllowed;
        }

        /// <summary>
        /// Checks if ICMPv6 is blocked, and then whether or not the packet is an NDP 
        /// </summary>
        /// <param name="packet"></param>
        /// <returns>Returns true if the packet is allowed, false if it is not allowed</returns>
        private bool isDeniedNDP(ICMPv6Packet packet)
        {
            // if they're denying all IPv6 except NDP
            if (data.DenyIPv6NDP)
            {
                // check if it's NDP
                if ((packet.Type <= 133) && (packet.Type >= 137))
                {
                    // it is, return allowed
                    return true;
                }
                // nope it's not, return BLOCKED
                else
                    return false;
            }

            // default true because they're not blocking ipv6 
            return true;
        }

        // module metadata
        private void Help()
        {
            ModuleMeta.Meta m = new ModuleMeta.Meta();
            m.Name = "ICMP Filter";
            m.Version = "1.0.4.0";
            m.Description = "Blocks ICMP packets of a given type/code";
            m.Contact = "shodivine@gmail.com";
            m.Author = "Bryan A. (drone)";
            m.Help = "This module can be used to block all (or particular) ICMP packets from flowing in or out of your network."
                + "ICMP packets are used to exchange error messages between networked computers, but it's also used with tools such as nmap "
                + "and nessus to gain valuable information about systems behind a gateway.\n\n"
                + "ICMP Segment Structure\n "
                + "\t|——————————————————————————————|\n"
                + "\t|  BITS  |  0-7  |  8-15  |  16-23  |  24-31 |\n"
                + "\t|——————————————————————————————|\n"
                + "\t|    0   |  Type |  Code  |  Checksum       |\n"
                + "\t|   32   |      Rest of Header                   |\n"
                + "\t|——————————————————————————————|\n"
                + "A full list of supported control messages can be found on the module page (View ICMP)."
                + "\n\nAs of .3.11, ICMPFilter differentiates between ICMPv4 and ICMPv6.  It can block all IPv4 or all IPv6 packets, as well"
                + " as in inidividual v4/v6.\n\nThe module can also now block all ICMPv6 packets EXCEPT for NDP packets.  This is because"
                + " NDP packets are required for intranet connectivity and other vital tasks.";
            MetaData = new ModuleMeta(m);

            Language lang = Language.ENGLISH;
            multistring.SetString(lang, "ICMPv6 was dropped", "ICMPv6 from {0} for {1} was dropped.");
            multistring.SetString(lang, "ICMPv4 was dropped", "ICMPv4 from {0} for {1} was dropped.");

            lang = Language.CHINESE;
            multistring.SetString(lang, "ICMPv6 was dropped", "ICMPv6 从 {0} 至 {1} 被丢弃.");
            multistring.SetString(lang, "ICMPv4 was dropped", "ICMPv4 从 {0} 至 {1} 被丢弃.");

            lang = Language.DUTCH;
            multistring.SetString(lang, "ICMPv6 was dropped", "ICMPv6 uit {0} voor {1} is neergezet.");
            multistring.SetString(lang, "ICMPv4 was dropped", "ICMPv4 uit {0} voor {1} is neergezet.");

            lang = Language.FRENCH;
            multistring.SetString(lang, "ICMPv6 was dropped", "ICMPv6 à partir de {0} pour {1} a été abandonné.");
            multistring.SetString(lang, "ICMPv4 was dropped", "ICMPv4 à partir de {0} pour {1} a été abandonné.");

            lang = Language.GERMAN;
            multistring.SetString(lang, "ICMPv6 was dropped", "ICMPv6 aus {0} für {1} wurde fallengelassen.");
            multistring.SetString(lang, "ICMPv4 was dropped", "ICMPv4 aus {0} für {1} wurde fallengelassen.");

            lang = Language.HEBREW;
            // JUST KIDDING; 

            lang = Language.ITALIAN;
            multistring.SetString(lang, "ICMPv6 was dropped", "ICMPv6 da {0} per {1} è stato eliminato.");
            multistring.SetString(lang, "ICMPv4 was dropped", "ICMPv4 da {0} per {1} è stato eliminato.");

            lang = Language.JAPANESE;
            multistring.SetString(lang, "ICMPv6 was dropped", "ICMPv6 から {0} のために {1} 削除されました.");
            multistring.SetString(lang, "ICMPv4 was dropped", "ICMPv4 から {0} のために {1} 削除されました.");

            lang = Language.PORTUGUESE;
            multistring.SetString(lang, "ICMPv6 was dropped", "ICMPv6 a partir de {0} para {1} foi descartado.");
            multistring.SetString(lang, "ICMPv4 was dropped", "ICMPv4 a partir de {0} para {1} foi descartado.");

            lang = Language.RUSSIAN;
            multistring.SetString(lang, "ICMPv6 was dropped", "ICMPv6 от {0} для {1} было прекращено.");
            multistring.SetString(lang, "ICMPv4 was dropped", "ICMPv4 от {0} для {1} было прекращено.");

            lang = Language.SPANISH;
            multistring.SetString(lang, "ICMPv6 was dropped", "ICMPv6 a partir {0} de {1} se cayo.");
            multistring.SetString(lang, "ICMPv4 was dropped", "ICMPv4 a partir {0} de {1} se cayo.");
        }
    }
}
