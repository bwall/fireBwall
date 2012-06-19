using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

using fireBwall.Configuration;
using fireBwall.Logging;
using fireBwall.Modules;
using fireBwall.Packets;
using fireBwall.UI;
using fireBwall.Utils;

namespace MacFilter
{
    public class MacFilterModule : NDISModule
    {
        [Serializable]
        public enum PacketStatus
        {
            UNDETERMINED,
            BLOCKED,
            ALLOWED
        }

        [Serializable]
        [Flags]
        public enum Direction
        {
            IN = 1,
            OUT = 1 << 1
        }

        [Serializable]
        [XmlRoot("macrule")]
        public class MacRule: IXmlSerializable
        {
            public PacketStatus ps;
            public byte[] mac;
            public Direction direction;
            public bool log = true;
            public bool notify = true;

            public string String
            {
                get { return ToString(); }
            }

            public override string ToString()
            {
                string ret = "";
                if (ps == PacketStatus.ALLOWED)
                {
                    ret = "Allows";
                }
                else
                {
                    ret = "Blocks";
                }
                if (mac != null)
                    ret += " MAC " + new PhysicalAddress(mac).ToString();
                else
                    ret += " all MACs ";
                if (direction == (Direction.IN | Direction.OUT))
                {
                    ret += " in and out";
                }
                else if (direction == Direction.OUT)
                {
                    ret += " out";
                }
                else if (direction == Direction.IN)
                {
                    ret += " in";
                }
                if (notify)
                    ret += " and notifies";
                if (log)
                    ret += " and logs";
                return ret;
            }

            public MacRule() { }

            public MacRule(PacketStatus ps, Direction direction, bool log, bool notify)
            {
                this.ps = ps;
                this.mac = null;
                this.direction = direction;
                this.log = log;
                this.notify = notify;
            }

            public MacRule(PacketStatus ps, PhysicalAddress mac, Direction direction, bool log, bool notify)
            {
                this.ps = ps;
                this.mac = mac.GetAddressBytes();
                this.direction = direction;
                this.log = log;
                this.notify = notify;
            }

            public PacketStatus GetStatus(Packet pkt)
            {
                EthPacket epkt = (EthPacket)pkt;
                if (pkt.Outbound && (direction & Direction.OUT) == Direction.OUT)
                {
                    if ( mac == null || Utility.ByteArrayEq(mac, epkt.ToMac))
                    {
                        if (log)
                            message = "packet from " + new PhysicalAddress(epkt.FromMac).ToString() +
                                " to " + new PhysicalAddress(epkt.ToMac).ToString();
                        return ps;
                    }
                }
                else if (!pkt.Outbound && (direction & Direction.IN) == Direction.IN)
                {
                    if ( mac == null || Utility.ByteArrayEq(mac, epkt.FromMac))
                    {
                        if (log)
                            message = "packet from " + new PhysicalAddress(epkt.FromMac).ToString() +
                                " to " + new PhysicalAddress(epkt.ToMac).ToString();
                        return ps;
                    }
                }
                return PacketStatus.UNDETERMINED;
            }

            string message = null;
            public string GetLogMessage()
            {
                if (!log)
                    return null;
                if (ps == PacketStatus.ALLOWED)
                {
                    return "Allowed " + message;
                }
                return "Blocked " + message;
            }

            public string ToFileString()
            {
                return null;
            }

            public System.Xml.Schema.XmlSchema GetSchema() { return null; }
            public virtual void ReadXml(XmlReader reader) { }
            public virtual void WriteXml(XmlWriter writer) { }
        }

        public MacFilterModule()
            : base()
        {
            ModuleMeta.Meta m = new ModuleMeta.Meta();
            m.Name = "MAC Address Filter";
            m.Version = "1.1.0.0";
            m.Help = "Each network adapter has a MAC address.  It can only be changed or faked in rare circumstances."
                + "Each packet sent over the network says the MAC its from and the MAC its to."
                + "This module allows you to control which MAC you will send or recieve data from.  Similarly to the Basic Firewall, the rules are processed in order from top to bottom.  You can also reorder the rules by clicking move up and move down.  To add a rule, click on Add Rule, and to remove one, click Remove Rule.";
            m.Description = "Blocks or allows packets based on MAC address";
            m.Contact = "nightstrike9809@gmail.com";
            m.Author = "Brian W. (schizo)";

            MetaData = new ModuleMeta(m);

            Language lang = Language.ENGLISH;
            multistring.SetString(lang, "Add Rule", "Add Rule");
            multistring.SetString(lang, "Remove Rule", "Remove Rule");
            multistring.SetString(lang, "Move Up", "Move Up");
            multistring.SetString(lang, "Move Down", "Move Down");

            lang = Language.CHINESE;
            multistring.SetString(lang, "Add Rule", "新增规则");
            multistring.SetString(lang, "Remove Rule", "删除规则");
            multistring.SetString(lang, "Move Up", "动起来");
            multistring.SetString(lang, "Move Down", "下移");

            lang = Language.DUTCH;
            multistring.SetString(lang, "Add Rule", "Regel toevoegen");
            multistring.SetString(lang, "Remove Rule", "Regel verwijderen");
            multistring.SetString(lang, "Move Up", "Omhoog");
            multistring.SetString(lang, "Move Down", "Omlaag verplaatsen");

            lang = Language.FRENCH;
            multistring.SetString(lang, "Add Rule", "Ajoutez la règle");
            multistring.SetString(lang, "Remove Rule", "Supprimer la règle");
            multistring.SetString(lang, "Move Up", "Déplacez vers le haut");
            multistring.SetString(lang, "Move Down", "Déplacer vers le bas");

            lang = Language.GERMAN;
            multistring.SetString(lang, "Add Rule", "Regel hinzufügen");
            multistring.SetString(lang, "Remove Rule", "Regel entfernen");
            multistring.SetString(lang, "Move Up", "Nach oben");
            multistring.SetString(lang, "Move Down", "Nach unten");

            lang = Language.HEBREW;
            multistring.SetString(lang, "Add Rule", "הוספת כלל");
            multistring.SetString(lang, "Remove Rule", "הסרת כלל");
            multistring.SetString(lang, "Move Up", "הזז למעלה");
            multistring.SetString(lang, "Move Down", "הזז למטה");

            lang = Language.ITALIAN;
            multistring.SetString(lang, "Add Rule", "Aggiungi regola");
            multistring.SetString(lang, "Remove Rule", "Rimuovere regola");
            multistring.SetString(lang, "Move Up", "Spostarsi verso l'alto");
            multistring.SetString(lang, "Move Down", "Spostare verso il basso");

            lang = Language.JAPANESE;
            multistring.SetString(lang, "Add Rule", "ルールを追加します。");
            multistring.SetString(lang, "Remove Rule", "規則を削除します。");
            multistring.SetString(lang, "Move Up", "上に移動します。");
            multistring.SetString(lang, "Move Down", "下に移動します。");

            lang = Language.PORTUGUESE;
            multistring.SetString(lang, "Add Rule", "Adicionar regra");
            multistring.SetString(lang, "Remove Rule", "remover Regra");
            multistring.SetString(lang, "Move Up", "mover para cima");
            multistring.SetString(lang, "Move Down", "mover para Baixo");

            lang = Language.RUSSIAN;
            multistring.SetString(lang, "Add Rule", "Добавить правило");
            multistring.SetString(lang, "Remove Rule", "Удалить правило");
            multistring.SetString(lang, "Move Up", "вверх");
            multistring.SetString(lang, "Move Down", "спускать");

            lang = Language.SPANISH;
            multistring.SetString(lang, "Add Rule", "Añadir regla");
            multistring.SetString(lang, "Remove Rule", "Eliminar la regla");
            multistring.SetString(lang, "Move Up", "Subir");
            multistring.SetString(lang, "Move Down", "Bajar");
        }

        readonly object padlock = new object();
        public MultilingualStringManager multistring = new MultilingualStringManager();
        public List<MacRule> rules = new List<MacRule>();

        public override bool ModuleStart()
        {
            try
            {
                RuleSet rs = Load<RuleSet>();
                lock (padlock)
                {
                    if (null == rs)
                        rules = new List<MacRule>();
                    else
                    {
                        rules = new List<MacRule>();
                        XmlSerializer deserializer = new XmlSerializer(typeof(RuleSet));
                        rules.AddRange(rs.Rules);
                    }
                }
            }
            catch (Exception e)
            {
                LogCenter.Instance.LogException(e);
                return false;
            }
            return true;
        }

        public override bool ModuleStop()
        {
            try
            {
                lock (padlock)
                {
                    if (rules.Count > 0)
                    {
                        RuleSet rs = new RuleSet();
                        rs.Rules = rules.ToArray();
                        Save<RuleSet>(rs);
                    }
                }
            }
            catch (Exception e)
            {
                LogCenter.Instance.LogException(e);
                return false;
            }
            return true;
        }

        public override DynamicUserControl  GetUserInterface()
        {
            return new MacFilterControl(this) { Dock = System.Windows.Forms.DockStyle.Fill };
        }

        public override PacketMainReturnType interiorMain(ref Packet in_packet)
        {
            LogEvent le;
            lock (padlock)
            {
                PacketStatus status = PacketStatus.UNDETERMINED;
                foreach (MacRule r in rules)
                {
                    status = r.GetStatus(in_packet);
                    if (status == PacketStatus.BLOCKED)
                    {
                        PacketMainReturnType pmr = PacketMainReturnType.Drop;
                        if (r.GetLogMessage() != null)
                        {
                            pmr |= PacketMainReturnType.Log;
                            le = new LogEvent(String.Format(r.GetLogMessage()), this);
                            LogCenter.Instance.LogEvent(le);
                        }
                        if (r.notify)
                        {
                            pmr |= PacketMainReturnType.Popup;
                        }
                        return pmr;
                    }
                    else if (status == PacketStatus.ALLOWED)
                    {
                        return PacketMainReturnType.Allow;
                    }
                }
            }
            return PacketMainReturnType.Allow;
        }

        public void InstanceGetRuleUpdates(List<MacRule> r)
        {
            lock (padlock)
            {
                rules = new List<MacRule>(r);
            }
        }

        [Serializable]
        public class RuleSet : IXmlSerializable
        {
            [XmlArray("Rules")]
            public MacRule[] Rules = new MacRule[0];

            public RuleSet() { }

            public System.Xml.Schema.XmlSchema GetSchema()
            {
                return null;
            }

            public void ReadXml(XmlReader reader)
            {
                List<MacRule> rules = new List<MacRule>();
                reader.ReadStartElement("RuleSet");
                if (!reader.IsEmptyElement)
                {
                    reader.ReadStartElement("Rules");
                    while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
                    {
                        reader.ReadStartElement("Rule");
                        XmlSerializer serializer;

                        if (reader.Name.Equals("macrule"))
                        {
                            serializer = new XmlSerializer(typeof(MacRule));
                            rules.Add((MacRule)serializer.Deserialize(reader));
                        }
                        reader.ReadEndElement();
                        Rules = rules.ToArray();
                        reader.MoveToContent();
                    }
                    reader.ReadEndElement();
                }
                else
                    reader.ReadStartElement("Rules");
                reader.ReadEndElement();
            }

            public void WriteXml(XmlWriter writer)
            {
                writer.WriteStartElement("Rules");
                foreach (MacRule r in Rules)
                {
                    writer.WriteStartElement("Rule");

                    XmlSerializer serializer = new XmlSerializer(r.GetType());
                    serializer.Serialize(writer, r);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
        }
    }
}
