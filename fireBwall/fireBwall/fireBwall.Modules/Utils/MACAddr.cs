using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace fireBwall.Utils
{
    [XmlRoot("MACAddr")]
    public class MACAddr : IEquatable<MACAddr>, IXmlSerializable
    {
        #region Variables

        public byte[] AddressBytes = new byte[6];

        #endregion

        #region Constructors

        public MACAddr()
        {
            AddressBytes = new byte[6] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        }

        public MACAddr(byte[] bytes)
        {
            AddressBytes = bytes;
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            if (obj is MACAddr)
            {
                return Equals((MACAddr)obj);
            }
            return false;
        }

        public bool Equals(MACAddr other)
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
            return BitConverter.ToString(AddressBytes).Replace('-', ':');
        }

        #endregion

        #region Static Factories

        public static bool TryParse(string ip, out MACAddr macaddr)
        {
            try
            {
                macaddr = Parse(ip);
                return true;
            }
            catch
            {
                macaddr = new MACAddr();
                return false;
            }
        }

        public static MACAddr Parse(string ip)
        {
            ip = ip.Replace(":", "");
            if (ip.Length != 12)
                throw new FormatException();
            byte[] bytes = Utility.StringToByteArray(ip);
            return new MACAddr(bytes);
        }

        #endregion

        #region Serialization

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            if (reader.IsEmptyElement)
            {
                reader.ReadStartElement();
            }
            else
            {
                reader.ReadStartElement("Addr");
                XmlSerializer stringSerialization = new XmlSerializer(typeof(string));
                MACAddr ip = MACAddr.Parse((string)stringSerialization.Deserialize(reader));
                this.AddressBytes = ip.AddressBytes;
                reader.ReadEndElement();
                reader.ReadEndElement();
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("Addr");
            XmlSerializer stringSerialization = new XmlSerializer(typeof(string));
            stringSerialization.Serialize(writer, ToString());
            writer.WriteEndElement();
        }

        #endregion

        #region Equatable

        public class EqualityComparer : IEqualityComparer<MACAddr>
        {
            public bool Equals(MACAddr x, MACAddr y)
            {
                return x.Equals(y);
            }
            public int GetHashCode(MACAddr x)
            {
                return x.GetHashCode();
            }
        }

        #endregion
    }
}
