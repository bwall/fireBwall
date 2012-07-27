using System;
using System.Collections.Generic;
using System.Text;
using fireBwall.Modules;
using fireBwall.Packets;
using fireBwall.Utils;
using fireBwall.Configuration;
using fireBwall.Logging;
using System.Net;
using System.Net.NetworkInformation;

namespace ARPPoisoningProtection
{
    public class ARPPoisoningProtectionModule : NDISModule
    {
        public ARPPoisoningProtectionModule()
            : base()
        {
            ModuleMeta.Meta m = new ModuleMeta.Meta();
            m.Name = "ARP Poisoning Protection";
            m.Version = "1.2.0.1";
            m.Author = "Brian W. (schizo)";
            m.Description = "Protects against ARP Poisoning attacks";
            m.Help = "ARP Poisoning:  A local network attack that redirects communications through the attacker.  This allows them see, modify, or drop your traffic.  There is very little for protection against these attacks currently out on the market."
                + "\r\n\r\nHow we prevent it is by ignoring any ARP response packet that reaches the firewall that you computer didn't ask for.  On top of that, we also save solicited responses so if a conflicting response is returned, we can tell an attack is happening, and the firewall attempts to tell the other computer being attacked the truth."
                + "\r\n\r\nThe module configuration is quite simple.  It displays the known MAC address to IP correlations that it knows.  If one of these values is incorrect, you can remove it by clicking on it, and hitting the 'Remove Entry' button.  You can also clear the entire cache, if you switch networks or something like that.  The Save ARP Cache check box will allow you to save or not save the values for the next time fireBwall runs.  The Log Unsolicited check box lets you choose if you want to be notified of ARP Responses that your computer did not request.  The Log Attacks check box lets you choose if you want to be notified of ARP Poisoning Attacks that have been detected and defeated.  The Rectify Attack checkbox lets you choose if fireBwall tries to fix the attack on the other victim, this is extremely effective for simple attacks.";
            m.Contact = "nightstrike9809@gmail.com";
            MetaData = new ModuleMeta(m);

            Language lang = Language.ENGLISH;
            multistring.SetString(lang, "Response does not equal cache", "ARP Response from {0} for {1} does not match the ARP cache.");
            multistring.SetString(lang, "Unsolicited", "Unsolicited ARP Response from {0} for {1}.");

            lang = Language.CHINESE;
            multistring.SetString(lang, "Response does not equal cache", "从 {0} {1} ARP 回应与 ARP 缓存不匹配。");
            multistring.SetString(lang, "Unsolicited", "从 {0} {1} 未经请求的 ARP 回应。");

            lang = Language.DUTCH;
            multistring.SetString(lang, "Response does not equal cache", "ARP reactie van {0} {1} komt niet overeen met de ARP-cache.");
            multistring.SetString(lang, "Unsolicited", "Ongevraagde ARP reactie van {0} {1}.");

            lang = Language.FRENCH;
            multistring.SetString(lang, "Response does not equal cache", "Réponse ARP de {0} de {1} ne correspond pas le cache ARP.");
            multistring.SetString(lang, "Unsolicited", "Réponse de ARP non sollicité de {0} de {1}.");

            lang = Language.GERMAN;
            multistring.SetString(lang, "Response does not equal cache", "ARP-Antwort von {0} {1} entspricht nicht den ARP-Cache.");
            multistring.SetString(lang, "Unsolicited", "Unaufgeforderte ARP-Antwort von {0} {1}.");

            lang = Language.HEBREW;
            multistring.SetString(lang, "Response does not equal cache", "תגובת ARP {0} עבור {1} אינו תואם את מטמון ARP.");
            multistring.SetString(lang, "Unsolicited", "תגובת ARP שלא ביקשת מראש מ- {0} עבור {1}.");

            lang = Language.ITALIAN;
            multistring.SetString(lang, "Response does not equal cache", "ARP risposta da {0} per {1} non corrisponde la cache ARP.");
            multistring.SetString(lang, "Unsolicited", "Risposta di ARP non richiesto da {0} per {1}.");

            lang = Language.JAPANESE;
            multistring.SetString(lang, "Response does not equal cache", "ARP 応答から {0} {1} の ARP キャッシュが一致しません。");
            multistring.SetString(lang, "Unsolicited", "不要な ARP 応答から {0} を {1}。");

            lang = Language.PORTUGUESE;
            multistring.SetString(lang, "Response does not equal cache", "Resposta ARP de {0} para {1} não coincide com o cache do ARP.");
            multistring.SetString(lang, "Unsolicited", "Resposta ARP não solicitadas de {0} para {1}.");

            lang = Language.RUSSIAN;
            multistring.SetString(lang, "Response does not equal cache", "ARP ответ от {0} {1}, не соответствует кэш ARP.");
            multistring.SetString(lang, "Unsolicited", "Незапрошенные ARP ответ от {0} {1}.");
            
            lang = Language.SPANISH;
            multistring.SetString(lang, "Response does not equal cache", "Respuesta de ARP de {0} {1} no coincide con la caché ARP.");
            multistring.SetString(lang, "Unsolicited", "Respuesta de ARP no solicitada de {0} {1}.");
        }

        [Serializable]
        public class ArpData
        {
            public SerializableDictionary<IPAddr, MACAddr> arpCache = new SerializableDictionary<IPAddr, MACAddr>();
            public bool Save = true;
            public bool LogUnsolic = true;
            public bool LogAttacks = true;
            public bool RectifyAttacks = false;
        }

        public ArpData data;
        MultilingualStringManager multistring = new MultilingualStringManager();
        List<IPAddr> requestedIPs = new List<IPAddr>();
        object padlock = new object();

        public event System.Threading.ThreadStart UpdatedArpCache;

        public SerializableDictionary<IPAddr, MACAddr> GetCache()
        {
            lock (padlock)
            {
                return new SerializableDictionary<IPAddr, MACAddr>(data.arpCache);
            }
        }

        public void UpdateCache(SerializableDictionary<IPAddr, MACAddr> cache)
        {
            lock (padlock)
            {
                data.arpCache = new SerializableDictionary<IPAddr, MACAddr>(cache);
            }
        }

        public override fireBwall.UI.DynamicUserControl GetUserInterface()
        {
            return new ArpPoisoningProtection(this) { Dock = System.Windows.Forms.DockStyle.Fill };
        }

        public override bool ModuleStart()
        {
            data = Load<ArpData>();
            if (data == null)
            {
                data = new ArpData();
            }
            return true;
        }

        public override bool ModuleStop()
        {
            if (data != null)
            {
                ArpData d = data;
                if (!d.Save)
                {
                    d.arpCache = new SerializableDictionary<IPAddr, MACAddr>();
                }
                Save<ArpData>(d);
            }
            return true;
        }

        public override PacketMainReturnType interiorMain(ref Packet in_packet)
        {
            if (in_packet.GetHighestLayer() == Protocol.ARP)
            {
                ARPPacket arpp = (ARPPacket)in_packet;
                if (arpp.isRequest && arpp.Outbound)
                {
                    IPAddr ip = new IPAddr(arpp.ATargetIP.GetAddressBytes());
                    if (!requestedIPs.Contains(ip))
                        requestedIPs.Add(ip);
                }
                else if (!arpp.Outbound)
                {
                    IPAddr ip = new IPAddr(arpp.ASenderIP.GetAddressBytes());
                    if (!arpp.isRequest)
                    {
                        if (requestedIPs.Contains(ip))
                        {
                            lock (padlock)
                            {
                                if (data.arpCache.ContainsKey(new IPAddr(arpp.ASenderIP.GetAddressBytes())))
                                {
                                    if (!Utility.ByteArrayEq(data.arpCache[new IPAddr(arpp.ASenderIP.GetAddressBytes())].AddressBytes, arpp.ASenderMac))
                                    {
                                        PacketMainReturnType pmr = 0;
                                        if (data.RectifyAttacks)
                                            pmr = PacketMainReturnType.Edited;
                                        else
                                            pmr = PacketMainReturnType.Drop;
                                        if (data.LogAttacks)
                                        {
                                            LogEvent le = new LogEvent(String.Format(multistring.GetString("Response does not equal cache"), new PhysicalAddress(arpp.ASenderMac).ToString(), arpp.ASenderIP.ToString()), this);
                                            le.PMR = PacketMainReturnType.Log | PacketMainReturnType.Popup;
                                            LogCenter.Instance.LogEvent(le);
                                        }
                                        if (data.RectifyAttacks)
                                        {
                                            arpp.ATargetIP = arpp.ASenderIP;
                                            arpp.ATargetMac = data.arpCache[new IPAddr(arpp.ATargetIP.GetAddressBytes())].AddressBytes;
                                            arpp.ASenderMac = this.Adapter.GetAdapterInformation().InterfaceInformation.GetPhysicalAddress().GetAddressBytes();
                                            arpp.FromMac = arpp.ASenderMac;
                                            arpp.ToMac = arpp.ATargetMac;
                                            arpp.ASenderIP = Adapter.GetAdapterInformation().IPv4;
                                            arpp.Outbound = true;
                                            in_packet = arpp;
                                        }
                                        return pmr;
                                    }
                                    else
                                    {
                                        requestedIPs.Remove(ip);
                                    }
                                }
                                else
                                {
                                    data.arpCache[new IPAddr(arpp.ASenderIP.GetAddressBytes())] = new MACAddr(arpp.ASenderMac);
                                    if (UpdatedArpCache != null)
                                        UpdatedArpCache();
                                    requestedIPs.Remove(ip);
                                }
                            }
                        }
                        else
                        {
                            lock (padlock)
                            {
                                if (data.arpCache.ContainsKey(new IPAddr(arpp.ASenderIP.GetAddressBytes())))
                                {
                                    if (!Utility.ByteArrayEq(data.arpCache[new IPAddr(arpp.ASenderIP.GetAddressBytes())].AddressBytes, arpp.ASenderMac))
                                    {
                                        PacketMainReturnType pmra = 0;
                                        if (data.RectifyAttacks)
                                            pmra = PacketMainReturnType.Edited;
                                        else
                                            pmra = PacketMainReturnType.Drop;
                                        if (data.LogAttacks)
                                        {
                                            LogEvent le = new LogEvent(String.Format(multistring.GetString("Response does not equal cache"), new PhysicalAddress(arpp.ASenderMac).ToString(), arpp.ASenderIP.ToString()), this);
                                            le.PMR = PacketMainReturnType.Log | PacketMainReturnType.Popup;
                                            LogCenter.Instance.LogEvent(le);
                                        }
                                        if (data.RectifyAttacks)
                                        {
                                            arpp.ATargetIP = arpp.ASenderIP;
                                            arpp.ATargetMac = data.arpCache[new IPAddr(arpp.ATargetIP.GetAddressBytes())].AddressBytes;
                                            arpp.ASenderMac = this.Adapter.GetAdapterInformation().InterfaceInformation.GetPhysicalAddress().GetAddressBytes();
                                            arpp.FromMac = arpp.ASenderMac;
                                            arpp.ToMac = arpp.ATargetMac;
                                            arpp.ASenderIP = Adapter.GetAdapterInformation().IPv4;
                                            arpp.Outbound = true;
                                            in_packet = arpp;
                                        }
                                        return pmra;
                                    }
                                }
                            }
                            PacketMainReturnType pmr = 0;
                            pmr = PacketMainReturnType.Drop;
                            if (data.LogUnsolic)
                            {
                                LogEvent le2 = new LogEvent(String.Format(multistring.GetString("Unsolicited"), new PhysicalAddress(arpp.ASenderMac).ToString(), arpp.ASenderIP.ToString()), this);
                                le2.PMR = PacketMainReturnType.Log;
                            }
                            return pmr;
                        }
                    }
                    else
                    {
                        lock (padlock)
                        {
                            if (data.arpCache.ContainsKey(new IPAddr(arpp.ASenderIP.GetAddressBytes())))
                            {
                                if (!Utility.ByteArrayEq(data.arpCache[new IPAddr(arpp.ASenderIP.GetAddressBytes())].AddressBytes, arpp.ASenderMac))
                                {
                                    PacketMainReturnType pmr = PacketMainReturnType.Drop;
                                    if (data.LogAttacks)
                                    {
                                        LogEvent le = new LogEvent(String.Format(multistring.GetString("Response does not equal cache"), new PhysicalAddress(arpp.ASenderMac).ToString(), arpp.ASenderIP.ToString()), this);
                                        le.PMR = PacketMainReturnType.Log | PacketMainReturnType.Popup;
                                        LogCenter.Instance.LogEvent(le);
                                    }
                                    return pmr;
                                }
                            }
                        }
                    }
                    return 0;
                }
                return 0;
            }
            return 0;
        }
    }
}
