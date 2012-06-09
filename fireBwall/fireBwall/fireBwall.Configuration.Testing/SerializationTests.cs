using System;
using System.Collections.Generic;
using NUnit.Framework;
using fireBwall.Utils;
using System.Xml.Serialization;
using System.IO;
using System.Text;

namespace fireBwall.Configuration.Testing
{
    [TestFixture]
    public class SerializationTests
    {
        [Test]
        public void IPAddrSerialization()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(IPAddr));
            MemoryStream ms = new MemoryStream();
            IPAddr outAddr = IPAddr.Parse("192.168.1.1");
            serializer.Serialize(ms, outAddr);
            ms.Position = 0;
            IPAddr inAddr = (IPAddr)serializer.Deserialize(ms);
            Assert.AreEqual("192.168.1.1", inAddr.ToString());
        }

        [Test]
        public void ListofIPAddrSerialization()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<IPAddr>));
            MemoryStream ms = new MemoryStream();
            IPAddr outAddr = IPAddr.Parse("192.168.1.1");
            List<IPAddr> list = new List<IPAddr>();
            list.Add(outAddr);
            list.Add(outAddr);
            list.Add(outAddr);
            serializer.Serialize(ms, list);
            ms.Position = 0;
            List<IPAddr> inAddr = (List<IPAddr>)serializer.Deserialize(ms);
            Assert.AreEqual("192.168.1.1", inAddr[0].ToString());
            Assert.AreEqual("192.168.1.1", inAddr[1].ToString());
            Assert.AreEqual("192.168.1.1", inAddr[2].ToString());
        }

        [Test]
        public void DictionaryofIPAddrSerialization()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SerializableDictionary<IPAddr, IPAddr>));
            MemoryStream ms = new MemoryStream();
            IPAddr outAddr = IPAddr.Parse("192.168.1.1");
            SerializableDictionary<IPAddr, IPAddr> list = new SerializableDictionary<IPAddr, IPAddr>();
            list.Add(outAddr, outAddr);
            serializer.Serialize(ms, list);
            ms.Position = 0;
            SerializableDictionary<IPAddr, IPAddr> inAddr = (SerializableDictionary<IPAddr, IPAddr>)serializer.Deserialize(ms);
            foreach (KeyValuePair<IPAddr, IPAddr> pair in list)
            {
                Assert.AreEqual("192.168.1.1", pair.Key.ToString());
                Assert.AreEqual("192.168.1.1", pair.Value.ToString());
            }            
        }
    }
}
