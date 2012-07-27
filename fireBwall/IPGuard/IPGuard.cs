using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text.RegularExpressions;

using fireBwall.Modules;
using fireBwall.Logging;
using fireBwall.Packets;
using fireBwall.Configuration;
using fireBwall.Utils;

/// Class mimics the behavior of tools like PeerBlock.  Give it some lists
/// and it'll block all outgoing (or incoming) requests from the IP addresses.
namespace PassThru
{
    public class IPGuard : NDISModule
    {
        // all the blocked ranges
        private List<IPAddressRange> block_ranges = new List<IPAddressRange>();
        
        private List<string> available_lists = new List<string>();
        public List<string> Available_Lists
        { get { return available_lists; } set { available_lists = value; } }

        private IPGuardUI guardUI;

        public IPGuard() : base()
        {
            Help();
        }

        /// <summary>
        /// returns the user control gui
        /// </summary>
        /// <returns>A UI!</returns>
        public override fireBwall.UI.DynamicUserControl  GetUserInterface()
        {
            // generate the UI and load the available lists
            guardUI = new IPGuardUI(this) { Dock = System.Windows.Forms.DockStyle.Fill };
            return guardUI;
        }
        
        /// <summary>
        /// Start the mod, deserialize data into GuardData
        /// </summary>
        /// <returns></returns>
        public override bool ModuleStart()
        {
            try
            {
                data = Load<GuardData>();
                if (null == data)
                    data = new GuardData();

                // block ranges aren't serialized, so go rebuild them with the loaded lists
                // when the module is started
                rebuild();
            }
            catch (Exception e)
            {
                LogCenter.Instance.LogException(e);
                return false;
            }
        
            return true;
        }

        /// <summary>
        /// Stop the module, serialize the data object out
        /// </summary>
        /// <returns></returns>
        public override bool ModuleStop()
        {
            try
            {
                GuardData d = data;
                if (data != null)
                {
                    if (!data.Save)
                    {
                        data.Loaded_Lists = new List<string>();
                        available_lists = new List<string>();
                        data.logBlocked = false;
                        data.blockIncoming = false;
                    }
                }
                Save<GuardData>(d);
            }
            catch (Exception e)
            {
                LogCenter.Instance.LogException(e);
                return false;
            }

            return true;
        }

        // serialized object for IPGuard data
        [Serializable]
        public class GuardData
        {
            private List<string> loaded_lists = new List<string>();
            public List<string> Loaded_Lists
                    { get { return loaded_lists; } set { loaded_lists = value; } }

            public bool Save = true;
            public bool logBlocked = false;
            public bool blockIncoming = false;
        }

        public GuardData data;
        public MultilingualStringManager multistring = new MultilingualStringManager();

        /// <summary>
        /// chuck out bad packets
        /// </summary>
        /// <param name="in_packet"></param>
        /// <returns></returns>
        public override PacketMainReturnType interiorMain(ref Packet in_packet)
        {
            try
            {
                LogEvent le;
                PacketMainReturnType pmr;
                if (in_packet.ContainsLayer(Protocol.TCP))
                {
                    // cast the packet and check for SYN/outbound
                    TCPPacket packet = (TCPPacket)in_packet;
                    if (packet.SYN && packet.Outbound)
                    {
                        // check if it's blocked
                        for (int i = 0; i < block_ranges.Count; ++i)
                        {
                            // if its heading towards a blacklisted IP
                            if (block_ranges[i].IsInRange(packet.DestIP))
                            {
                                pmr = PacketMainReturnType.Drop;
                                // check if we should log it
                                if (this.data.logBlocked)
                                {
                                    pmr |= PacketMainReturnType.Log;
                                    le = new LogEvent(String.Format(multistring.GetString("Blocked Outgoing"), packet.DestIP.ToString()), this);
                                    le.PMR = PacketMainReturnType.Drop | PacketMainReturnType.Log;
                                    LogCenter.Instance.LogEvent(le);
                                }
                                return pmr;
                            }
                        }
                    }
                    // check if they want to block incoming packets from these addresses
                    // as well.
                    if (this.data.blockIncoming && !(packet.Outbound))
                    {
                        for (int i = 0; i < block_ranges.Count; ++i)
                        {
                            if (block_ranges[i].IsInRange(packet.SourceIP))
                            {
                                pmr = PacketMainReturnType.Drop;
                                // check if we should log it
                                if (this.data.logBlocked)
                                {
                                    pmr |= PacketMainReturnType.Log;
                                    le = new LogEvent(String.Format(multistring.GetString("Blocked Incoming"), packet.SourceIP.ToString()), this);
                                    LogCenter.Instance.LogEvent(le);
                                }
                                return pmr;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogCenter.Instance.LogException(e);
            }
            return PacketMainReturnType.Allow;
        }
        
        /// <summary>
        /// reads in the block list and builds the obj.  Hopefully the user is smart and either
        /// downloaded the list from the de facto location, or has it formatted properly.
        /// Documented that in the help string.
        /// </summary>
        public void buildRanges(String file)
        {
            // open the file
            try
            {
                // make sure the file wasn't removed sometime between loading
                // and adding
                if (!File.Exists(file))
                    return;

                using (StreamReader sr = new StreamReader(file))
                {
                    String line;
                    
                    while ((line = sr.ReadLine()) != null)
                    {
                        // PARSINGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG
                        // if the line isn't a comment, has stuff on it, and is formatted with a -
                        if (!line.StartsWith("#") && line.Length > 2 && 
                             line.Contains("-"))
                        {
                            string addrs = line.Substring(
                                line.LastIndexOf(":")+1, (line.Length - line.LastIndexOf(":")-1));
                            block_ranges.Add(new IPAddressRange(IPAddress.Parse(addrs.Substring(0, addrs.IndexOf("-"))),
                                                                IPAddress.Parse(addrs.Substring(addrs.IndexOf("-")+1, (addrs.Length - addrs.IndexOf("-")-1)))));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogCenter.Instance.LogException(e);
            }
        }

        /// <summary>
        /// method used to rebuild the blocked IP ranges when lists are removed.  
        /// there's really no 'quick' way to discern which block matches to which list
        /// aside from either a) dumping the entire thing when a list is removed and rebuilding
        /// it or b) using a hash mapping of file -> list of ranges, but then that makes 
        /// iterating through the block ranges slower. 
        /// </summary>
        public void rebuild()
        {
            // clear out the current list
            block_ranges.Clear();
            // REBULID IT
            foreach (Object s in data.Loaded_Lists)
                buildRanges(s.ToString());
        }

        // block lists are in ranges; use this to quickly discover
        // if a given IP is within the blocked range
        private class IPAddressRange
        {
            private AddressFamily addressFamily;
            private byte[] lowerBytes;
            private byte[] upperBytes;

            public IPAddressRange(IPAddress lower, IPAddress upper)
            {
                this.addressFamily = lower.AddressFamily;
                this.lowerBytes = lower.GetAddressBytes();
                this.upperBytes = upper.GetAddressBytes();
            }

            /// <summary>
            /// Check if an IP address falls within the given range
            /// </summary>
            /// <param name="addr">address to check</param>
            /// <returns>true/false</returns>
            public bool IsInRange(IPAddr addr)
            {
                byte[] addrBytes = addr.GetAddressBytes();

                bool lBound = true;
                bool uBound = true;

                // iterate over ip bytes in the range
                for (int i = 0; i < this.lowerBytes.Length &&
                        (lBound || uBound); ++i)
                {
                    if ((lBound && addrBytes[i] < lowerBytes[i]) ||
                        (uBound && addrBytes[i] > upperBytes[i]))
                    {
                        return false;
                    }

                    lBound &= (addrBytes[i] == lowerBytes[i]);
                    uBound &= (addrBytes[i] == upperBytes[i]);
                }
                return true;
            }

            /// <summary>
            /// Return the lower IP addr
            /// </summary>
            /// <returns>IPAddress</returns>
            public IPAddress getLower()
            {
                return new IPAddress(lowerBytes);
            }

            /// <summary>
            /// Return the upper IP addr
            /// </summary>
            /// <returns>IPAddress</returns>
            public IPAddress getUpper()
            {
                return new IPAddress(upperBytes);
            }
        }

        /// <summary>
        /// metadata 
        /// </summary>
        private void Help()
        {
            ModuleMeta.Meta m = new ModuleMeta.Meta();
            m.Name = "IPGuard";
            m.Author = "Bryan A.";
            m.Contact = "shodivine@gmail.com";
            m.Description = "Blocks IPs from given lists.";
            m.Version = "1.1.0.0";
            m.Help = "IPGuard is a module that mimics the behavior of other blocklist applications such as PeerBlock, or its predecessor PeerGuardian.  Given a correctly formatted list,"
                                  + "  IPGuard can block TCP packets, both incoming and outgoing, to a wide range of IPs.  The most widely distributed lists are typically those found on"
                                  + "www.iblocklist.com.  \n\nThese lists need to be downloaded and added to your /firebwall/modules/IPGuard folder, and then enabled in the module's GUI."
                                  + "  These lists need to be formatted in the following way: <string>:ip-ip.  If you, for example, wanted to block a single IP address, it would be"
                                  + " required to be in the following form: firebwall:66.172.10.29-66.172.10.29";

            MetaData = new ModuleMeta(m);

            Language lang = Language.ENGLISH;
            multistring.SetString(lang, "Blocked Outgoing", "Blocked outgoing packets from {0}");
            multistring.SetString(lang, "Blocked Incoming", "Blocked incoming packets from {0}");

            lang = Language.CHINESE;
            multistring.SetString(lang, "Blocked Outgoing", "阻止从传出数据包 {0}");
            multistring.SetString(lang, "Blocked Incoming", "被阻止传入的数据包 {0}");

            lang = Language.DUTCH;
            multistring.SetString(lang, "Blocked Outgoing", "De uitgaande pakketten van geblokkeerd {0}");
            multistring.SetString(lang, "Blocked Incoming", "Binnenkomende pakketten van geblokkeerd {0}");

            lang = Language.FRENCH;
            multistring.SetString(lang, "Blocked Outgoing", "Bloqué des paquets sortants de {0}");
            multistring.SetString(lang, "Blocked Incoming", "Bloque les paquets entrants de {0}");

            lang = Language.GERMAN;
            multistring.SetString(lang, "Blocked Outgoing", "Blockierte ausgehende Pakete aus {0}");
            multistring.SetString(lang, "Blocked Incoming", "Blockiert eingehende Pakete von {0}");

            lang = Language.ITALIAN;
            multistring.SetString(lang, "Blocked Outgoing", "Bloccati i pacchetti in uscita dal {0}");
            multistring.SetString(lang, "Blocked Incoming", "Bloccati i pacchetti in ingresso da {0}");

            lang = Language.JAPANESE;
            multistring.SetString(lang, "Blocked Outgoing", "発信パケットをブロック {0}");
            multistring.SetString(lang, "Blocked Incoming", "着信パケットをブロック {0}");

            lang = Language.PORTUGUESE;
            multistring.SetString(lang, "Blocked Outgoing", "Bloqueados pacotes de saída de {0}");
            multistring.SetString(lang, "Blocked Incoming", "Bloqueados pacotes de entrada da {0}");

            lang = Language.RUSSIAN;
            multistring.SetString(lang, "Blocked Outgoing", "Заблокированы исходящие пакеты от {0}");
            multistring.SetString(lang, "Blocked Incoming", "Заблокировано входящих пакетов {0}");

            lang = Language.SPANISH;
            multistring.SetString(lang, "Blocked Outgoing", "Bloquea los paquetes salientes desde {0}");
            multistring.SetString(lang, "Blocked Incoming", "Bloquea los paquetes entrantes de {0}");
        }
    }
}
