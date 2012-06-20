using System;
using System.Collections.Generic;
using System.Text;
using fireBwall.Modules;
using fireBwall.Packets;
using fireBwall.Logging;
using fireBwall.Utils;
using System.Net;
using System.Net.NetworkInformation;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace BasicFirewall
{
    public class BasicFirewallModule : NDISModule
    {
        public BasicFirewallModule()
            : base()
        {
            ModuleMeta.Meta m = new ModuleMeta.Meta();
            m.Name = "BasicFirewall";
            m.Version = "1.2.0.1";
            m.Author = "Brian W. (schizo)";
            m.Description = "Blocks or allows packets based on IP/Port";
            m.Help = "Computers on networks often communicate using protocols that have ports.  TCP and UDP are the most common protocols.  You will see TCP being used very often, as it allows for stable and reliable connections.  UDP is less reliable and is used a bit less.  Both of these use ports to communicate.  Certain ports are used for certain things.  Some you may want open so you can share files or remotely control you computer, but in most cases, you want them closed."
                + "\r\n\r\nThis module uses rules to Allow or Drop packets depending on what port or ip they are for.  It is the one part that is in about every firewall."
                + "\r\n\r\nThis module works based on ordered rules.  The rules are displayed from top to bottom, and the order can be changed by clicking and dragging the rule.  Rules can be added with the Add Rule button, and removed with the Remove Rule button."
                + "\n\nArguments are designated on a rule-by-rule basis.  Some have required arguments, others do not.  If the arguments box is greyed out, "
                + "that particular rule has no arguments.  Otherwise, the required arguments will be denoted by the line of text above the arguments box.\n\n"
                + "BasicFirewall port rules also allow multiple ports or port ranges.  Port ranges should be spaced with a '-'.  For example, to block ports 22 and 80 and range 100 to 200,"
                + " you would use the following: 22 80 100-200";
            m.Contact = "nightstrike9809@gmail.com";
            MetaData = new ModuleMeta(m);
        }

        public class RuleFactory
        {
            public static Rule MakeRule(RuleType ruleType, PacketStatus ps, Direction dir, string args, bool log, bool notify)
            {
                switch (ruleType)
                {
                    case RuleType.IP:
                        return GenIPRule(ps, args, dir, log, notify);
                    case RuleType.TCPALL:
                        return new TCPAllRule(ps, dir, log, notify);
                    case RuleType.TCPIPPORT:
                        return new TCPIPPortRule(ps, IPAddr.Parse(args.Split(' ')[0]), int.Parse(args.Split(' ')[1]), dir, log, notify);
                    case RuleType.TCPPORT:
                        return GenTCPPORT(ps, args, dir, log, notify);
                    case RuleType.UDPALL:
                        return new UDPAllRule(ps, dir, log, notify);
                    case RuleType.UDPPORT:
                        return GenUDPPORT(ps, args, dir, log, notify);
                    case RuleType.ALL:
                        return new AllRule(ps, dir, log, notify);
                }
                return null;
            }
        }

        /// <summary>
        /// Parses out the arguments of a UDPPort rule
        /// 
        /// We accept single or multiple ports and port ranges
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static UDPPortRule GenUDPPORT(PacketStatus ps, string args, Direction dir, bool log, bool notify)
        {
            // first tokenize the args
            List<string> tmp = new List<string>(args.Split(' '));
            UDPPortRule rule = new UDPPortRule();
            rule.port_ranges = new List<PortRange>();
            try
            {
                // iterate through the given arguments
                foreach (string s in tmp)
                {
                    string loc = s;
                    // parse the port range
                    if (loc.Contains("-"))
                    {
                        // parse the start/end ports out of the arguments
                        PortRange p = new PortRange();
                        string[] split = loc.Split('-');
                        p.start = Convert.ToInt32(split[0]);
                        p.end = Convert.ToInt32(split[1]);

                        // instead of an error message, we can just swap them
                        if (p.start > p.end)
                        {
                            int temp = p.start;
                            p.start = p.end;
                            p.end = temp;
                        }

                        // add it to the rule port ranges list
                        rule.port_ranges.Add(p);
                    }
                    // else it's just a port, add it as usual
                    else
                    {
                        rule.port.Add(Convert.ToInt32(s));
                    }
                }

                // set the other rule stuff
                rule.ps = ps;
                rule.direction = dir;
                rule.log = log;
                rule.notify = notify;
            }
            catch (Exception e)
            {
                // probably a parsing error; log it and throw an 
                // exception so the rule isn't touched
                //LogCenter.WriteErrorLog(e);
                throw new Exception();
            }
            return rule;
        }

        /// <summary>
        /// Parses out the arguments of a TCPPort rule
        /// 
        /// We accept single or multiple ports and port ranges
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static TCPPortRule GenTCPPORT(PacketStatus ps, string args, Direction dir, bool log, bool notify)
        {
            // first tokenize the args
            List<string> tmp = new List<string>(args.Split(' '));
            TCPPortRule rule = new TCPPortRule();
            rule.port_ranges = new List<PortRange>();

            try
            {
                // iterate through the given arguments
                foreach (string s in tmp)
                {
                    string loc = s;
                    // parse the port range
                    if (loc.Contains("-"))
                    {
                        // parse the start/end ports out of the arguments
                        PortRange p = new PortRange();
                        string[] split = loc.Split('-');
                        p.start = Convert.ToInt32(split[0]);
                        p.end = Convert.ToInt32(split[1]);

                        // someday we'll give more meaningful error messages, but 
                        // for now just throw an exception if they're doing something
                        // dumb like range 200-50
                        if (p.start > p.end)
                            throw new Exception();

                        // add it to the rule port ranges list
                        rule.port_ranges.Add(p);
                    }
                    // else it's just a port, add it as usual
                    else
                    {
                        rule.port.Add(Convert.ToInt32(s));
                    }
                }

                // set the other rule stuff
                rule.ps = ps;
                rule.direction = dir;
                rule.log = log;
                rule.notify = notify;
            }
            catch (Exception)
            {
                // probably a parsing error; log it
                // and throw an exception so the rule isn't changed
                //LogCenter.WriteErrorLog(e);
                throw new Exception();
            }
            return rule;
        }

        /// <summary>
        /// Used to generate a new IPRule
        /// </summary>
        /// <returns></returns>
        private static IPRule GenIPRule(PacketStatus ps, string args, Direction dir, bool log, bool notify)
        {
            List<string> tokens = new List<string>(args.Split(' '));
            IPRule rule = new IPRule();
            rule.ips = new List<IPAddr>();

            try
            {
                // iterate through the tokens and try and parse them into 
                // an IP address
                foreach (string s in tokens)
                {
                    IPAddr tmp = IPAddr.Parse(s);
                    rule.ips.Add(tmp);
                }

                // set the rest of the fields
                rule.ps = ps;
                rule.direction = dir;
                rule.log = log;
                rule.notify = notify;
            }
            catch (Exception)
            {
                // probably a parse error
                throw new Exception();
            }
            return rule;
        }

        readonly object padlock = new object();
        public List<Rule> rules = new List<Rule>();

        public override fireBwall.UI.DynamicUserControl GetUserInterface()
        {
            return new BasicFirewallControl(this) { Dock = System.Windows.Forms.DockStyle.Fill };
        }

        public override bool ModuleStart()
        {            
            lock (padlock)
            {
                RuleSet temp = Load<RuleSet>();
                if (temp == null)
                {
                    rules = new List<Rule>();
                    rules.Add(new TCPAllRule(PacketStatus.BLOCKED, Direction.IN, true, true));
                }
                else
                {
                    rules = new List<Rule>();
                    XmlSerializer deserializer = new XmlSerializer(typeof(RuleSet));
                    rules.AddRange(temp.Rules);
                }
            }
            return true;
        }

        public override bool ModuleStop()
        {
            lock (padlock)
            {
                if (rules != null)
                {
                    RuleSet rs = new RuleSet();
                    rs.Rules = rules.ToArray();
                    Save<RuleSet>(rs);
                }
            }
            return true;
        }

        public override PacketMainReturnType interiorMain(ref Packet in_packet)
        {
            lock (padlock)
            {
                PacketStatus status = PacketStatus.UNDETERMINED;
                foreach (Rule r in rules)
                {
                    status = r.GetStatus(in_packet);
                    if (status == PacketStatus.BLOCKED)
                    {
                        PacketMainReturnType pmr;
                        pmr = PacketMainReturnType.Drop | PacketMainReturnType.LogPacket;                        
                        if (r.GetLogMessage() != null)
                        {
                            LogEvent le = new LogEvent(r.GetLogMessage(), this);
                            le.PMR = PacketMainReturnType.Log;    
                            if (r.Notify())
                            {
                                le.PMR |= PacketMainReturnType.Popup;
                            }
                            LogCenter.Instance.LogEvent(le);
                        }                        
                        return pmr;
                    }
                    else if (status == PacketStatus.ALLOWED)
                    {
                        return 0;
                    }
                }
            }
            return 0;
        }

        public void InstanceGetRuleUpdates(List<Rule> r)
        {
            lock (padlock)
            {
                rules = new List<Rule>(r);
            }
        }

        Quad MakeQuad(Packet in_packet)
        {
            if (in_packet.ContainsLayer(Protocol.TCP))
            {
                TCPPacket tcp = (TCPPacket)in_packet;
                Quad q = new Quad
                {
                    dstIP = tcp.DestIP,
                    dstPort = tcp.DestPort,
                    srcPort = tcp.SourcePort,
                    srcIP = tcp.SourceIP
                };
                return q;
            }
            return null;
        }
    }

    [Serializable]
    public class RuleSet : IXmlSerializable
    {
        [XmlArray("Rules")]
        public Rule[] Rules = new Rule[0];

        public RuleSet() { }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            List<Rule> rules = new List<Rule>();
            reader.ReadStartElement("RuleSet");
            if (!reader.IsEmptyElement)
            {
                reader.ReadStartElement("Rules");
                while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
                {
                    reader.ReadStartElement("Rule");
                    XmlSerializer serializer;
                    switch (reader.Name)
                    {
                        case "tcpallrule":
                            serializer = new XmlSerializer(typeof(TCPAllRule));
                            rules.Add((TCPAllRule)serializer.Deserialize(reader));
                            break;
                        case "tcpportrule":
                            serializer = new XmlSerializer(typeof(TCPPortRule));
                            rules.Add((TCPPortRule)serializer.Deserialize(reader));
                            break;
                        case "tcpipportrule":
                            serializer = new XmlSerializer(typeof(TCPIPPortRule));
                            rules.Add((TCPIPPortRule)serializer.Deserialize(reader));
                            break;
                        case "udpportrule":
                            serializer = new XmlSerializer(typeof(UDPPortRule));
                            rules.Add((UDPPortRule)serializer.Deserialize(reader));
                            break;
                        case "allrule":
                            serializer = new XmlSerializer(typeof(AllRule));
                            rules.Add((AllRule)serializer.Deserialize(reader));
                            break;
                        case "iprule":
                            serializer = new XmlSerializer(typeof(IPRule));
                            rules.Add((IPRule)serializer.Deserialize(reader));
                            break;
                        case "udpallrule":
                            serializer = new XmlSerializer(typeof(UDPAllRule));
                            rules.Add((UDPAllRule)serializer.Deserialize(reader));
                            break;
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
            foreach (Rule r in Rules)
            {
                writer.WriteStartElement("Rule");

                XmlSerializer serializer = new XmlSerializer(r.GetType());
                serializer.Serialize(writer, r);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
    }

    [Serializable]
    public enum PacketStatus
    {
        UNDETERMINED = 1,
        BLOCKED = 1 << 1,
        ALLOWED = 1 << 2
    }

    [Serializable]
    [Flags]
    public enum Direction
    {
        IN = 1,
        OUT = 1 << 1
    }

    [XmlInclude(typeof(AllRule))]
    [XmlInclude(typeof(IPRule))]
    [XmlInclude(typeof(TCPAllRule))]
    [XmlInclude(typeof(TCPIPPortRule))]
    [XmlInclude(typeof(TCPPortRule))]
    [XmlInclude(typeof(UDPAllRule))]
    [XmlInclude(typeof(UDPPortRule))]
    public class Rule : IXmlSerializable
    {
        public PacketStatus ps = PacketStatus.UNDETERMINED;
        public Direction direction = Direction.IN;

        public virtual PacketStatus GetStatus(Packet pkt)
        {
            return PacketStatus.UNDETERMINED;
        }
        public virtual string GetLogMessage()
        {
            return "";
        }
        public virtual bool Notify()
        {
            return false;
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }


        public virtual void ReadXml(XmlReader reader)
        {
            
        }

        public virtual void WriteXml(XmlWriter writer)
        {
            
        }
    }

    [Serializable]
    public enum RuleType
    {
        UDPALL,
        UDPPORT,
        TCPIPPORT,
        TCPPORT,
        TCPALL,
        IP,
        ALL
    }

    // object for storing port ranges
    [Serializable]
    public class PortRange
    {
        public int start;
        public int end;

        // check if the queried port is within range
        public Boolean isInRange(int port)
        {
            if ((port >= start) && (port < end))
                return true;

            return false;
        }

        public override string ToString()
        {
            return start.ToString() + "-" + end.ToString();
        }
    }

    #region Rules

    [Serializable]
    [XmlRoot("tcpallrule")]
    public class TCPAllRule : Rule
    {
        public bool log = true;
        public bool notify = true;

        public TCPAllRule() { }

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
            ret += " all TCP ports";
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

        public TCPAllRule(PacketStatus ps, Direction direction, bool log, bool notify)
        {
            this.ps = ps;
            this.direction = direction;
            this.log = log;
            this.notify = notify;
        }

        public override PacketStatus GetStatus(Packet pkt)
        {
            if (pkt.ContainsLayer(Protocol.TCP))
            {
                TCPPacket tcppkt = (TCPPacket)pkt;
                if (tcppkt.SYN && !(tcppkt.ACK))
                {
                    if (pkt.Outbound && (direction & Direction.OUT) == Direction.OUT)
                    {
                        if (log)
                            message = " TCP packet from " + tcppkt.SourceIP.ToString() + ":" +
                                      tcppkt.SourcePort.ToString() + " to " + tcppkt.DestIP.ToString() +
                                      ":" + tcppkt.DestPort.ToString();
                        return ps;
                    }
                    else if (!pkt.Outbound && (direction & Direction.IN) == Direction.IN)
                    {
                        if (log)
                            message = " TCP packet from " + tcppkt.SourceIP.ToString() + ":" +
                                tcppkt.SourcePort.ToString() + " to " + tcppkt.DestIP.ToString() + ":" +
                                tcppkt.DestPort.ToString();
                        return ps;
                    }
                }
            }
            return PacketStatus.UNDETERMINED;
        }

        [XmlIgnore()]
        string message = "";

        public override string GetLogMessage()
        {
            if (!log)
                return null;
            if (ps == PacketStatus.ALLOWED)
            {
                return "Allowed " + message;
            }
            return "Blocked " + message;
        }

        public override bool Notify()
        {
            return notify;
        }

        public override void WriteXml(XmlWriter writer)
        {
            XmlSerializer logSerializer = new XmlSerializer(typeof(bool));
            XmlSerializer notifySerializer = new XmlSerializer(typeof(bool));            

            writer.WriteStartElement("log");
            logSerializer.Serialize(writer, log);
            writer.WriteEndElement();

            writer.WriteStartElement("notify");
            notifySerializer.Serialize(writer, notify);
            writer.WriteEndElement();

            XmlSerializer dirSerializer = new XmlSerializer(typeof(Direction));
            writer.WriteStartElement("direction");
            dirSerializer.Serialize(writer, direction);
            writer.WriteEndElement();

            XmlSerializer statusSerializer = new XmlSerializer(typeof(PacketStatus));
            writer.WriteStartElement("status");
            statusSerializer.Serialize(writer, ps);
            writer.WriteEndElement();
        }

        public override void ReadXml(XmlReader reader)
        {
            XmlSerializer logSerializer = new XmlSerializer(typeof(bool));
            XmlSerializer notifySerializer = new XmlSerializer(typeof(bool));

            reader.ReadStartElement("tcpallrule");
            
            reader.ReadStartElement("log");
            log = (bool)logSerializer.Deserialize(reader);
            reader.ReadEndElement();

            reader.ReadStartElement("notify");
            notify = (bool)notifySerializer.Deserialize(reader);
            reader.ReadEndElement();

            XmlSerializer dirSerializer = new XmlSerializer(typeof(Direction));
            reader.ReadStartElement("direction");
            direction = (Direction)dirSerializer.Deserialize(reader);
            reader.ReadEndElement();

            XmlSerializer statusSerializer = new XmlSerializer(typeof(PacketStatus));
            reader.ReadStartElement("status");
            ps = (PacketStatus)statusSerializer.Deserialize(reader);
            reader.ReadEndElement();

            reader.ReadEndElement();
        }
    }

    [Serializable]
    [XmlRoot("tcpportrule")]
    public class TCPPortRule : Rule
    {
        public List<int> port = new List<int>();
        public bool log = true;
        public bool notify = true;

        public override void WriteXml(XmlWriter writer)
        {
            XmlSerializer logSerializer = new XmlSerializer(typeof(bool));
            XmlSerializer notifySerializer = new XmlSerializer(typeof(bool));
            XmlSerializer intSerializer = new XmlSerializer(typeof(int));
            XmlSerializer prSerializer = new XmlSerializer(typeof(PortRange));

            writer.WriteStartElement("log");
            logSerializer.Serialize(writer, log);
            writer.WriteEndElement();

            writer.WriteStartElement("notify");
            notifySerializer.Serialize(writer, notify);
            writer.WriteEndElement();

            XmlSerializer dirSerializer = new XmlSerializer(typeof(Direction));
            writer.WriteStartElement("direction");
            dirSerializer.Serialize(writer, direction);
            writer.WriteEndElement();

            XmlSerializer statusSerializer = new XmlSerializer(typeof(PacketStatus));
            writer.WriteStartElement("status");
            statusSerializer.Serialize(writer, ps);
            writer.WriteEndElement();

            writer.WriteStartElement("ports");
            foreach (int p in port)
            {
                writer.WriteStartElement("port");
                intSerializer.Serialize(writer, p);
                writer.WriteEndElement(); 
            }
            writer.WriteEndElement();

            writer.WriteStartElement("portranges");
            foreach (PortRange p in port_ranges)
            {
                writer.WriteStartElement("portrange");
                prSerializer.Serialize(writer, p);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        public override void ReadXml(XmlReader reader)
        {
            XmlSerializer logSerializer = new XmlSerializer(typeof(bool));
            XmlSerializer notifySerializer = new XmlSerializer(typeof(bool));
            XmlSerializer intSerializer = new XmlSerializer(typeof(int));
            XmlSerializer prSerializer = new XmlSerializer(typeof(PortRange));

            port_ranges = new List<PortRange>();
            port = new List<int>();

            reader.ReadStartElement("tcpportrule");

            reader.ReadStartElement("log");
            log = (bool)logSerializer.Deserialize(reader);
            reader.ReadEndElement();

            reader.ReadStartElement("notify");
            notify = (bool)notifySerializer.Deserialize(reader);
            reader.ReadEndElement();

            XmlSerializer dirSerializer = new XmlSerializer(typeof(Direction));
            reader.ReadStartElement("direction");
            direction = (Direction)dirSerializer.Deserialize(reader);
            reader.ReadEndElement();

            XmlSerializer statusSerializer = new XmlSerializer(typeof(PacketStatus));
            reader.ReadStartElement("status");
            ps = (PacketStatus)statusSerializer.Deserialize(reader);
            reader.ReadEndElement();

            if (!reader.IsEmptyElement)
            {
                reader.ReadStartElement("ports");
                while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
                {
                    reader.ReadStartElement("port");
                    port.Add((int)intSerializer.Deserialize(reader));
                    reader.ReadEndElement();

                    reader.MoveToContent();
                }
                reader.ReadEndElement();
            }
            else
                reader.ReadStartElement("ports");

            if (!reader.IsEmptyElement)
            {
                reader.ReadStartElement("portranges");
                while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
                {
                    reader.ReadStartElement("portrange");
                    port_ranges.Add((PortRange)prSerializer.Deserialize(reader));
                    reader.ReadEndElement();

                    reader.MoveToContent();
                }
                reader.ReadEndElement();
            }
            else
                reader.ReadStartElement("portranges");

            reader.ReadEndElement();
        }

        public List<PortRange> port_ranges
        {
            get; set;
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
            ret += " TCP port(s) " + GetPortString();
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

        public TCPPortRule(PacketStatus ps, List<int> port, Direction direction, bool log, bool notify)
        {
            this.ps = ps;
            this.direction = direction;
            this.port = port;
            this.log = log;
        }

        public TCPPortRule()
        {
            port = new List<int>();
            port_ranges = new List<PortRange>();
        }

        public override PacketStatus GetStatus(Packet pkt)
        {
            if (pkt.ContainsLayer(Protocol.TCP))
            {
                TCPPacket tcppkt = (TCPPacket)pkt;
                if (tcppkt.SYN && !(tcppkt.ACK))
                {
                    if (pkt.Outbound && (direction & Direction.OUT) == Direction.OUT)
                    {
                        if (port.Contains(tcppkt.DestPort) ||
                             inPortRange(tcppkt.DestPort))
                        {
                            if (log)
                                message = " TCP packet from " + tcppkt.SourceIP.ToString() +
                                    ":" + tcppkt.SourcePort.ToString() + " to " +
                                    tcppkt.DestIP.ToString() + ":" + tcppkt.DestPort.ToString();
                            return ps;
                        }
                    }
                    else if (!pkt.Outbound && (direction & Direction.IN) == Direction.IN)
                    {
                        if (port.Contains(tcppkt.DestPort) ||
                            inPortRange(tcppkt.DestPort))
                        {
                            if (log)
                                message = " TCP packet from " + tcppkt.SourceIP.ToString() +
                                    ":" + tcppkt.SourcePort.ToString() + " to " + tcppkt.DestIP.ToString() +
                                    ":" + tcppkt.DestPort.ToString();
                            return ps;
                        }
                    }
                }
            }
            return PacketStatus.UNDETERMINED;
        }

        /// <summary>
        /// iterate through the port_ranges object to find
        /// if the incoming port is allowed
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        private bool inPortRange(int port)
        {
            foreach (PortRange p in port_ranges)
            {
                if (p.isInRange(port))
                    return true;
            }
            return false;
        }

        [XmlIgnore()]
        string message = "";

        public override string GetLogMessage()
        {
            if (!log)
                return null;
            if (ps == PacketStatus.ALLOWED)
            {
                return "Allowed " + message;
            }
            return "Blocked " + message;
        }

        /// <summary>
        /// Returns the array of ports as a single string.
        /// 
        /// This takes the list of ints and port ranges, converts it to a list of strings, returns it as an array
        /// and finally joins all elements together with a space.
        /// </summary>
        /// <returns></returns>
        public string GetPortString()
        {
            string tmp = String.Join(" ", port.ConvertAll<string>(delegate(int i) { return i.ToString(); }).ToArray());
            // for formatting sake, add a space between ports and ranges
            if (tmp.Length > 0 && port_ranges.Count > 0)
                tmp += " ";
            tmp += String.Join(" ", port_ranges.ConvertAll<string>(delegate(PortRange i) { return i.ToString(); }).ToArray());
            return tmp;
        }

        public override bool Notify()
        {
            return notify;
        }
    }

    [Serializable]
    [XmlRoot("tcpipportrule")]
    public class TCPIPPortRule : Rule
    {
        public int port = -1;
        public IPAddr ip = new IPAddr();
        public bool log = true;
        public bool notify = true;

        public override void ReadXml(XmlReader reader)
        {
            XmlSerializer logSerializer = new XmlSerializer(typeof(bool));
            XmlSerializer notifySerializer = new XmlSerializer(typeof(bool));
            XmlSerializer intSerializer = new XmlSerializer(typeof(int));
            XmlSerializer byteSerializer = new XmlSerializer(typeof(IPAddr));

            reader.ReadStartElement("tcpipportrule");

            reader.ReadStartElement("log");
            log = (bool)logSerializer.Deserialize(reader);
            reader.ReadEndElement();

            reader.ReadStartElement("notify");
            notify = (bool)notifySerializer.Deserialize(reader);
            reader.ReadEndElement();

            XmlSerializer dirSerializer = new XmlSerializer(typeof(Direction));
            reader.ReadStartElement("direction");
            direction = (Direction)dirSerializer.Deserialize(reader);
            reader.ReadEndElement();

            XmlSerializer statusSerializer = new XmlSerializer(typeof(PacketStatus));
            reader.ReadStartElement("status");
            ps = (PacketStatus)statusSerializer.Deserialize(reader);
            reader.ReadEndElement();

            reader.ReadStartElement("port");
            port = ((int)intSerializer.Deserialize(reader));
            reader.ReadEndElement();

            reader.ReadStartElement("ip");
            ip = ((IPAddr)byteSerializer.Deserialize(reader));
            reader.ReadEndElement();

            reader.ReadEndElement();
        }

        public override void WriteXml(XmlWriter writer)
        {
            XmlSerializer logSerializer = new XmlSerializer(typeof(bool));
            XmlSerializer notifySerializer = new XmlSerializer(typeof(bool));
            XmlSerializer intSerializer = new XmlSerializer(typeof(int));
            XmlSerializer byteSerializer = new XmlSerializer(typeof(IPAddr));

            writer.WriteStartElement("log");
            logSerializer.Serialize(writer, log);
            writer.WriteEndElement();  

            writer.WriteStartElement("notify");
            notifySerializer.Serialize(writer, notify);
            writer.WriteEndElement();

            XmlSerializer dirSerializer = new XmlSerializer(typeof(Direction));
            writer.WriteStartElement("direction");
            dirSerializer.Serialize(writer, direction);
            writer.WriteEndElement();

            XmlSerializer statusSerializer = new XmlSerializer(typeof(PacketStatus));
            writer.WriteStartElement("status");
            statusSerializer.Serialize(writer, ps);
            writer.WriteEndElement();

            writer.WriteStartElement("port");
            intSerializer.Serialize(writer, port);
            writer.WriteEndElement();

            writer.WriteStartElement("ip");
            byteSerializer.Serialize(writer, ip);
            writer.WriteEndElement();  
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
            ret += " TCP " + ip.ToString() + ":" + port.ToString();
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

        public TCPIPPortRule() { }

        public TCPIPPortRule(PacketStatus ps, IPAddr ip, int port, Direction direction, bool log, bool notify)
        {
            this.ps = ps;
            this.direction = direction;
            this.port = port;
            this.ip = ip;
            this.log = log;
            this.notify = notify;
        }

        public override PacketStatus GetStatus(Packet pkt)
        {
            if (pkt.ContainsLayer(Protocol.TCP))
            {
                TCPPacket tcppkt = (TCPPacket)pkt;
                if (tcppkt.SYN && !(tcppkt.ACK))
                {
                    if (pkt.Outbound && (direction & Direction.OUT) == Direction.OUT)
                    {
                        if ((tcppkt.DestPort == port) && (Utility.ByteArrayEq(tcppkt.DestIP.GetAddressBytes(), ip.AddressBytes)))
                        {
                            if (log)
                                message = " TCP packet from " + tcppkt.SourceIP.ToString() + ":" +
                                    tcppkt.SourcePort.ToString() + " to " + tcppkt.DestIP.ToString() +
                                    ":" + tcppkt.DestPort.ToString();
                            return ps;
                        }
                    }
                    else if (!pkt.Outbound && (direction & Direction.IN) == Direction.IN)
                    {
                        if ((tcppkt.DestPort == port) && (Utility.ByteArrayEq(tcppkt.DestIP.GetAddressBytes(), ip.AddressBytes)))
                        {
                            if (log)
                                message = " TCP packet from " + tcppkt.SourceIP.ToString() +
                                    ":" + tcppkt.SourcePort.ToString() + " to " + tcppkt.DestIP.ToString() +
                                    ":" + tcppkt.DestPort.ToString();
                            return ps;
                        }
                    }
                }
            }
            return PacketStatus.UNDETERMINED;
        }

        [XmlIgnore()]
        string message = "";
        
        public override string GetLogMessage()
        {
            if (!log)
                return null;
            if (ps == PacketStatus.ALLOWED)
            {
                return "Allowed " + message;
            }
            return "Blocked " + message;
        }

        public override bool Notify()
        {
            return notify;
        }
    }

    [Serializable]
    [XmlRoot("udpportrule")]
    public class UDPPortRule : Rule
    {
        public List<int> port = new List<int>();
        public bool log = true;
        public bool notify = true;

        public override void ReadXml(XmlReader reader)
        {
            XmlSerializer logSerializer = new XmlSerializer(typeof(bool));
            XmlSerializer notifySerializer = new XmlSerializer(typeof(bool));
            XmlSerializer intSerializer = new XmlSerializer(typeof(int));
            XmlSerializer prSerializer = new XmlSerializer(typeof(PortRange));

            port_ranges = new List<PortRange>();
            port = new List<int>();

            reader.ReadStartElement("udpportrule");

            reader.ReadStartElement("log");
            log = (bool)logSerializer.Deserialize(reader);
            reader.ReadEndElement();

            reader.ReadStartElement("notify");
            notify = (bool)notifySerializer.Deserialize(reader);
            reader.ReadEndElement();

            XmlSerializer dirSerializer = new XmlSerializer(typeof(Direction));
            reader.ReadStartElement("direction");
            direction = (Direction)dirSerializer.Deserialize(reader);
            reader.ReadEndElement();

            XmlSerializer statusSerializer = new XmlSerializer(typeof(PacketStatus));
            reader.ReadStartElement("status");
            ps = (PacketStatus)statusSerializer.Deserialize(reader);
            reader.ReadEndElement();

            if (!reader.IsEmptyElement)
            {
                reader.ReadStartElement("ports");
                while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
                {
                    reader.ReadStartElement("port");
                    port.Add((int)intSerializer.Deserialize(reader));
                    reader.ReadEndElement();

                    reader.MoveToContent();
                }
                reader.ReadEndElement();
            }
            else
                reader.ReadStartElement("ports");

            if (!reader.IsEmptyElement)
            {
                reader.ReadStartElement("portranges");
                while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
                {
                    reader.ReadStartElement("portrange");
                    port_ranges.Add((PortRange)prSerializer.Deserialize(reader));
                    reader.ReadEndElement();

                    reader.MoveToContent();
                }
                reader.ReadEndElement();
            }
            else
                reader.ReadStartElement("portranges");

            reader.ReadEndElement();
        }

        public override void WriteXml(XmlWriter writer)
        {
            XmlSerializer logSerializer = new XmlSerializer(typeof(bool));
            XmlSerializer notifySerializer = new XmlSerializer(typeof(bool));
            XmlSerializer intSerializer = new XmlSerializer(typeof(int));
            XmlSerializer prSerializer = new XmlSerializer(typeof(PortRange));

            writer.WriteStartElement("log");
            logSerializer.Serialize(writer, log);
            writer.WriteEndElement();

            writer.WriteStartElement("notify");
            notifySerializer.Serialize(writer, notify);
            writer.WriteEndElement();

            XmlSerializer dirSerializer = new XmlSerializer(typeof(Direction));
            writer.WriteStartElement("direction");
            dirSerializer.Serialize(writer, direction);
            writer.WriteEndElement();

            XmlSerializer statusSerializer = new XmlSerializer(typeof(PacketStatus));
            writer.WriteStartElement("status");
            statusSerializer.Serialize(writer, ps);
            writer.WriteEndElement();

            writer.WriteStartElement("ports");
            foreach (int p in port)
            {
                writer.WriteStartElement("port");
                intSerializer.Serialize(writer, p);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement("portranges");
            foreach (PortRange p in port_ranges)
            {
                writer.WriteStartElement("portrange");
                prSerializer.Serialize(writer, p);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        public List<PortRange> port_ranges
        {
            get;
            set;
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
            ret += " UDP port(s) " + GetPortString();
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

        public UDPPortRule(PacketStatus ps, List<int> port, Direction direction, bool log, bool notify)
        {
            this.ps = ps;
            this.direction = direction;
            this.port = port;
            this.log = log;
            this.notify = notify;
        }

        public UDPPortRule()
        {
            port = new List<int>();
            port_ranges = new List<PortRange>();
        }

        public override PacketStatus GetStatus(Packet pkt)
        {
            if (pkt.ContainsLayer(Protocol.UDP))
            {
                UDPPacket udppkt = (UDPPacket)pkt;
                if (pkt.Outbound && (direction & Direction.OUT) == Direction.OUT)
                {
                    if (port.Contains(udppkt.DestPort) ||
                        inPortRange(udppkt.DestPort) ||
                        port.Contains(udppkt.SourcePort) ||
                        inPortRange(udppkt.SourcePort))
                    {
                        if (log)
                            message = " UDP packet from " + udppkt.SourceIP.ToString() +
                                ":" + udppkt.SourcePort.ToString() + " to " + udppkt.DestIP.ToString() +
                                ":" + udppkt.DestPort.ToString();
                        return ps;
                    }
                }
                else if (!pkt.Outbound && (direction & Direction.IN) == Direction.IN)
                {
                    if (port.Contains(udppkt.DestPort) ||
                        inPortRange(udppkt.DestPort) ||
                        port.Contains(udppkt.SourcePort) ||
                        inPortRange(udppkt.SourcePort))
                    {
                        if (log)
                            message = " UDP packet from " + udppkt.SourceIP.ToString() +
                                ":" + udppkt.SourcePort.ToString() + " to " + udppkt.DestIP.ToString() +
                                ":" + udppkt.DestPort.ToString();
                        return ps;
                    }
                }
            }
            return PacketStatus.UNDETERMINED;
        }

        /// <summary>
        /// iterates through the rule's port ranges and checks if the given
        /// port is within that range
        /// </summary>
        /// <param name="port">the port to check</param>
        /// <returns></returns>
        private bool inPortRange(int port)
        {
            foreach (PortRange p in port_ranges)
            {
                if (p.isInRange(port))
                    return true;
            }
            return false;
        }

        [XmlIgnore()]
        string message = "";

        public override string GetLogMessage()
        {
            if (!log)
                return null;
            if (ps == PacketStatus.ALLOWED)
            {
                return "Allowed " + message;
            }
            return "Blocked " + message;
        }

        /// <summary>
        /// Returns the array of ports as a single string.
        /// 
        /// This takes the list of ints and port ranges, converts it to a list of strings, returns it as an array
        /// and finally joins all elements together with a space.
        /// </summary>
        /// <returns></returns>
        public string GetPortString()
        {
            string tmp = String.Join(" ", port.ConvertAll<string>(delegate(int i) { return i.ToString(); }).ToArray());
            // for formatting sake, add a space between ports and ranges
            if (tmp.Length > 0 && port_ranges.Count > 0)
                tmp += " ";
            tmp += String.Join(" ", port_ranges.ConvertAll<string>(delegate(PortRange i) { return i.ToString(); }).ToArray());
            return tmp;
        }

        public override bool Notify()
        {
            return notify;
        }
    }

    [Serializable]
    [XmlRoot("udpallrule")]
    public class UDPAllRule : Rule
    {
        public bool log = true;
        public bool notify = true;

        public override void WriteXml(XmlWriter writer)
        {
            XmlSerializer logSerializer = new XmlSerializer(typeof(bool));
            XmlSerializer notifySerializer = new XmlSerializer(typeof(bool));

            writer.WriteStartElement("log");
            logSerializer.Serialize(writer, log);
            writer.WriteEndElement();

            writer.WriteStartElement("notify");
            notifySerializer.Serialize(writer, notify);
            writer.WriteEndElement();

            XmlSerializer dirSerializer = new XmlSerializer(typeof(Direction));
            writer.WriteStartElement("direction");
            dirSerializer.Serialize(writer, direction);
            writer.WriteEndElement();

            XmlSerializer statusSerializer = new XmlSerializer(typeof(PacketStatus));
            writer.WriteStartElement("status");
            statusSerializer.Serialize(writer, ps);
            writer.WriteEndElement();
        }

        public override void ReadXml(XmlReader reader)
        {
            XmlSerializer logSerializer = new XmlSerializer(typeof(bool));
            XmlSerializer notifySerializer = new XmlSerializer(typeof(bool));
            
            reader.ReadStartElement("udpallrule");

            reader.ReadStartElement("log");
            log = (bool)logSerializer.Deserialize(reader);
            reader.ReadEndElement();

            reader.ReadStartElement("notify");
            notify = (bool)notifySerializer.Deserialize(reader);
            reader.ReadEndElement();

            XmlSerializer dirSerializer = new XmlSerializer(typeof(Direction));
            reader.ReadStartElement("direction");
            direction = (Direction)dirSerializer.Deserialize(reader);
            reader.ReadEndElement();

            XmlSerializer statusSerializer = new XmlSerializer(typeof(PacketStatus));
            reader.ReadStartElement("status");
            ps = (PacketStatus)statusSerializer.Deserialize(reader);
            reader.ReadEndElement();

            reader.ReadEndElement();
        }

        public UDPAllRule() { }

        public UDPAllRule(PacketStatus ps, Direction direction, bool log, bool notify)
        {
            this.ps = ps;
            this.direction = direction;
            this.log = log;
            this.notify = notify;
        }

        public override PacketStatus GetStatus(Packet pkt)
        {
            if (pkt.ContainsLayer(Protocol.UDP))
            {
                UDPPacket udppkt = (UDPPacket)pkt;
                if (pkt.Outbound && (direction & Direction.OUT) == Direction.OUT)
                {
                    if (log)
                        message = " UDP packet from " + udppkt.SourceIP.ToString() + ":" +
                            udppkt.SourcePort.ToString() + " to " + udppkt.DestIP.ToString() +
                            ":" + udppkt.DestPort.ToString();
                    return ps;
                }
                else if (!pkt.Outbound && (direction & Direction.IN) == Direction.IN)
                {
                    if (log)
                        message = " UDP packet from " + udppkt.SourceIP.ToString() + ":" +
                            udppkt.SourcePort.ToString() + " to " + udppkt.DestIP.ToString() + ":" + udppkt.DestPort.ToString();
                    return ps;
                }
            }
            return PacketStatus.UNDETERMINED;
        }

        [XmlIgnore()]
        string message = "";

        public override string GetLogMessage()
        {
            if (!log) return null;
            if (ps == PacketStatus.ALLOWED)
            {
                return "Allowed " + message;
            }
            return "Blocked " + message;
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
            ret += " all UDP ports";
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

        public override bool Notify()
        {
            return notify;
        }
    }

    [Serializable]
    [XmlRoot("allrule")]
    public class AllRule : Rule
    {
        public bool log = true;
        public bool notify = true;

        public override void WriteXml(XmlWriter writer)
        {
            XmlSerializer logSerializer = new XmlSerializer(typeof(bool));
            XmlSerializer notifySerializer = new XmlSerializer(typeof(bool));

            writer.WriteStartElement("log");
            logSerializer.Serialize(writer, log);
            writer.WriteEndElement();

            writer.WriteStartElement("notify");
            notifySerializer.Serialize(writer, notify);
            writer.WriteEndElement();

            XmlSerializer dirSerializer = new XmlSerializer(typeof(Direction));
            writer.WriteStartElement("direction");
            dirSerializer.Serialize(writer, direction);
            writer.WriteEndElement();

            XmlSerializer statusSerializer = new XmlSerializer(typeof(PacketStatus));
            writer.WriteStartElement("status");
            statusSerializer.Serialize(writer, ps);
            writer.WriteEndElement();
        }

        public override void ReadXml(XmlReader reader)
        {
            XmlSerializer logSerializer = new XmlSerializer(typeof(bool));
            XmlSerializer notifySerializer = new XmlSerializer(typeof(bool));

            reader.ReadStartElement("allrule");

            reader.ReadStartElement("log");
            log = (bool)logSerializer.Deserialize(reader);
            reader.ReadEndElement();

            reader.ReadStartElement("notify");
            notify = (bool)notifySerializer.Deserialize(reader);
            reader.ReadEndElement();

            XmlSerializer dirSerializer = new XmlSerializer(typeof(Direction));
            reader.ReadStartElement("direction");
            direction = (Direction)dirSerializer.Deserialize(reader);
            reader.ReadEndElement();

            XmlSerializer statusSerializer = new XmlSerializer(typeof(PacketStatus));
            reader.ReadStartElement("status");
            ps = (PacketStatus)statusSerializer.Deserialize(reader);
            reader.ReadEndElement();

            reader.ReadEndElement();
        }

        public AllRule() { }

        public AllRule(PacketStatus ps, Direction direction, bool log, bool notify)
        {
            this.ps = ps;
            this.direction = direction;
            this.log = log;
            this.notify = notify;
        }

        public override PacketStatus GetStatus(Packet pkt)
        {
            if (pkt.Outbound && (direction & Direction.OUT) == Direction.OUT)
            {
                if (log)
                    message = " packet";
                return ps;
            }
            else if (!pkt.Outbound && (direction & Direction.IN) == Direction.IN)
            {
                if (log)
                    message = " packet";
                return ps;
            }
            return PacketStatus.UNDETERMINED;
        }

        [XmlIgnore()]
        string message = "";

        public override string GetLogMessage()
        {
            if (!log) return null;
            if (ps == PacketStatus.ALLOWED)
            {
                return "Allowed " + message;
            }
            return "Blocked " + message;
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
            ret += " all";
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

        public override bool Notify()
        {
            return notify;
        }
    }

    [Serializable]
    [XmlRoot("iprule")]
    public class IPRule : Rule
    {
        public bool log = true;
        public bool notify = true;

        public override void ReadXml(XmlReader reader)
        {
            XmlSerializer logSerializer = new XmlSerializer(typeof(bool));
            XmlSerializer notifySerializer = new XmlSerializer(typeof(bool));

            reader.ReadStartElement("iprule");

            reader.ReadStartElement("log");
            log = (bool)logSerializer.Deserialize(reader);
            reader.ReadEndElement();

            reader.ReadStartElement("notify");
            notify = (bool)notifySerializer.Deserialize(reader);
            reader.ReadEndElement();

            XmlSerializer dirSerializer = new XmlSerializer(typeof(Direction));
            reader.ReadStartElement("direction");
            direction = (Direction)dirSerializer.Deserialize(reader);
            reader.ReadEndElement();

            XmlSerializer statusSerializer = new XmlSerializer(typeof(PacketStatus));
            reader.ReadStartElement("status");
            ps = (PacketStatus)statusSerializer.Deserialize(reader);
            reader.ReadEndElement();

            ips = new List<IPAddr>();
            XmlSerializer byteSerializer = new XmlSerializer(typeof(IPAddr));
            reader.ReadStartElement("ips");
            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("ip");
                ips.Add(((IPAddr)byteSerializer.Deserialize(reader)));
                reader.ReadEndElement();

                reader.MoveToContent();
            }
            reader.ReadEndElement();

            reader.ReadEndElement();
        }

        public override void WriteXml(XmlWriter writer)
        {
            XmlSerializer logSerializer = new XmlSerializer(typeof(bool));
            XmlSerializer notifySerializer = new XmlSerializer(typeof(bool));
            XmlSerializer intSerializer = new XmlSerializer(typeof(IPAddr));

            writer.WriteStartElement("log");
            logSerializer.Serialize(writer, log);
            writer.WriteEndElement();

            writer.WriteStartElement("notify");
            notifySerializer.Serialize(writer, notify);
            writer.WriteEndElement();

            XmlSerializer dirSerializer = new XmlSerializer(typeof(Direction));
            writer.WriteStartElement("direction");
            dirSerializer.Serialize(writer, direction);
            writer.WriteEndElement();

            XmlSerializer statusSerializer = new XmlSerializer(typeof(PacketStatus));
            writer.WriteStartElement("status");
            statusSerializer.Serialize(writer, ps);
            writer.WriteEndElement();

            writer.WriteStartElement("ips");
            foreach (IPAddr p in ips)
            {
                writer.WriteStartElement("ip");
                intSerializer.Serialize(writer, p);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        [XmlIgnore()]
        public List<IPAddr> ips
        {
            get;
            set;
        }

        [XmlIgnore()]
        string message = "";

        public IPRule() { }

        public IPRule(PacketStatus ps, List<IPAddr> ip, Direction direction, bool log, bool notify)
        {
            this.ps = ps;
            this.direction = direction;
            this.ips = ip;
            this.log = log;
            this.notify = notify;
        }

        private bool Contains(IPAddr ip)
        {
            return ips.Contains(ip);
        }

        public override PacketStatus GetStatus(Packet pkt)
        {
            if (pkt.ContainsLayer(Protocol.IP))
            {
                IPPacket tcppkt = (IPPacket)pkt;
                if (pkt.Outbound && (direction & Direction.OUT) == Direction.OUT)
                {
                    if (Contains(tcppkt.DestIP))
                    {
                        if (log)
                            message = " IP packet from " + tcppkt.SourceIP.ToString() + " to " + tcppkt.DestIP.ToString();
                        return ps;
                    }
                }
                else if (!pkt.Outbound && (direction & Direction.IN) == Direction.IN)
                {
                    if (Contains(tcppkt.DestIP))
                    {
                        if (log)
                            message = " IP packet from " + tcppkt.SourceIP.ToString() + " to " + tcppkt.DestIP.ToString();
                        return ps;
                    }
                }
            }
            return PacketStatus.UNDETERMINED;
        }

        public override string GetLogMessage()
        {
            if (!log) return null;
            if (ps == PacketStatus.ALLOWED)
            {
                return "Allowed " + message;
            }
            return "Blocked " + message;
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
            ret += " IP " + GetIPString();
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
                ret += "and notifies";
            if (log)
                ret += " and logs";
            return ret;
        }

        public override bool Notify()
        {
            return notify;
        }


        /// <summary>
        /// Returns the array of IPs as a single string.
        /// 
        /// This takes the list of IPAddress's, converts it to a list of strings, returns it as an array
        /// and finally joins all elements together with a space.
        /// </summary>
        /// <returns></returns>
        public string GetIPString()
        {
            return String.Join(" ", ips.ConvertAll<string>(delegate(IPAddr i) { return i.ToString(); }).ToArray());
        }
    }

    #endregion
}
