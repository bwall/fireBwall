using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace fireBwall.Utils
{
    [Serializable]
    [XmlRoot("list")]
    public class SerializableList<T> : List<T>, IXmlSerializable
    {
        #region IXmlSerializable Members
        public SerializableList()
        {

        }

        public SerializableList(SerializableList<T> copy)
        {
            foreach (T kvp in copy)
                this.Add(kvp);
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(T));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("key");
                T key = (T)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                this.Add(key);
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(T));

            foreach (T key in this)
            {
                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();
            }
        }
        #endregion
    }
}
