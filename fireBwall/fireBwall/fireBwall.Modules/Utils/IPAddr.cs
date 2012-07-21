using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Net;

using fireBwall.Logging;

namespace fireBwall.Utils
{
    public class IPAddr : IEquatable<IPAddr>, IXmlSerializable
    {
        #region Variables

        public byte[] AddressBytes = new byte[0];

        public byte[] GetAddressBytes()
        {
            return AddressBytes;
        }

        #endregion

        #region Equatable

        public class EqualityComparer : IEqualityComparer<IPAddr>
        {
            public bool Equals(IPAddr x, IPAddr y)
            {
                return x.Equals(y);
            }
            public int GetHashCode(IPAddr x)
            {
                return x.GetHashCode();
            }
        }

        #endregion

        #region Constructors

        public IPAddr()
        {
            AddressBytes = new byte[4] { 0x00, 0x00, 0x00, 0x00 };
        }

        public IPAddr(byte[] bytes)
        {
            AddressBytes = bytes;
        }

        public IPAddr(IPAddress ip)
        {
            AddressBytes = ip.GetAddressBytes();
        }

        public IPAddr(IPAddr ip)
        {
            AddressBytes = ip.GetAddressBytes();
        }

        #endregion

        #region Static Factories

        public static bool TryParse(string ip, out IPAddr ipaddr)
        {
            try
            {
                ipaddr = Parse(ip);
                return true;
            }
            catch
            {
                ipaddr = new IPAddr();
                return false;
            }
        }

        public static IPAddr Parse(string ip)
        {
            if (ip.Contains("."))
            {
                string[] split = ip.Split('.');
                if (split.Length != 4)
                {
                    throw new FormatException();
                }
                byte[] bytes = new byte[4];
                bytes[0] = byte.Parse(split[0]);
                bytes[1] = byte.Parse(split[1]);
                bytes[2] = byte.Parse(split[2]);
                bytes[3] = byte.Parse(split[3]);
                IPAddr ret = new IPAddr(bytes);
                return ret;
            }
            else if (ip.Contains(":"))
            {
                string temp = ip;
                List<byte> bytes = new List<byte>();
                while (!string.IsNullOrWhiteSpace(temp))
                {
                    if (temp.StartsWith(":"))
                    {
                        bytes.Add(0x00);
                        bytes.Add(0x00);
                    }
                    else
                    {
                        bytes.Add(Convert.ToByte("" + temp[0] + temp[1], 16));
                        bytes.Add(Convert.ToByte("" + temp[0] + temp[1], 16));
                        temp = temp.Substring(4);
                    }
                    if(temp.Length != 0)
                        temp = temp.Substring(1);
                }
                if (bytes.Count != 16)
                    throw new FormatException();
                IPAddr ret = new IPAddr(bytes.ToArray());
                return ret;
            }
            throw new FormatException();
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            if (obj is IPAddr)
            {
                return Equals((IPAddr)obj);
            }
            return false;
        }

        public bool Equals(IPAddr other)
        {
            return Utility.ByteArrayEq(AddressBytes, other.AddressBytes);
        }

        public override int GetHashCode()
        {
            int hash = 0;
            for (int x = 0; x < AddressBytes.Length; x++)
            {
                hash += (AddressBytes[x] << (8 * (3 - (x % 4))));
            }
            return hash;
        }

        public override string ToString()
        {
            if (AddressBytes.Length == 4)
            {
                return AddressBytes[0] + "." + AddressBytes[1] + "." + AddressBytes[2] + "." + AddressBytes[3];
            }
            string ret = "";
            for (int x = 0; x < 16; x += 2)
            {
                if (!(AddressBytes[x] == 0x00 && AddressBytes[x + 1] == 0x00))
                {
                    if (Convert.ToString(AddressBytes[x], 16).Length == 1)
                        ret += "0";
                    ret += Convert.ToString(AddressBytes[x], 16);
                    if (Convert.ToString(AddressBytes[x + 1], 16).Length == 1)
                        ret += "0";
                    ret += Convert.ToString(AddressBytes[x + 1], 16);
                }
                if (x != 14)
                {
                    ret += ":";
                }
            }
            return ret;
        }

        #endregion

        #region Serialization

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            try
            {
                if (reader.IsEmptyElement)
                {
                    reader.ReadStartElement();
                }
                else
                {
                    while (!reader.IsStartElement("Addr"))
                        reader.Read();
                    reader.ReadStartElement("Addr");
                    XmlSerializer stringSerialization = new XmlSerializer(typeof(string));
                    IPAddr ip = IPAddr.Parse((string)stringSerialization.Deserialize(reader));
                    this.AddressBytes = ip.AddressBytes;
                    reader.ReadEndElement();
                    reader.ReadEndElement();
                }
            }
            catch (Exception e)
            {
                LogCenter.Instance.LogException(e);
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            try
            {
                writer.WriteStartElement("Addr");
                XmlSerializer stringSerialization = new XmlSerializer(typeof(string));
                stringSerialization.Serialize(writer, ToString());
                writer.WriteEndElement();
            }
            catch (Exception e)
            {
                LogCenter.Instance.LogException(e);
            }
        }

        #endregion
    }
}
