using System;
using System.Collections.Generic;
using fireBwall.UI;
using System.Text;
using fireBwall.Packets;
using fireBwall.Filters.NDIS;
using fireBwall.Configuration;
using fireBwall.Logging;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace fireBwall.Modules
{
    public abstract class NDISModule
    {
        #region Variables

        INDISFilter adapter;

        #endregion

        #region Members

        public INDISFilter Adapter
        {
            get { return adapter; }
            set { adapter = value; }
        }

        public ModuleMeta MetaData;

        #endregion

        #region Constructors

        public NDISModule()
        {
        }

        #endregion

        #region Functions

        public T Load<T>()
        {
            try
            {
                string folder = ConfigurationManagement.Instance.ConfigurationPath;
                folder = folder + Path.DirectorySeparatorChar + "modules";
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                folder = folder + Path.DirectorySeparatorChar + "configs";
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                string file = folder + Path.DirectorySeparatorChar + adapter.GetAdapterInformation().Id + "-" + MetaData.GetMeta().Name + ".cfg";
                if (File.Exists(file))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    TextReader reader = new StreamReader(file);
                    try
                    {
                        T ret = (T)serializer.Deserialize(reader);
                        reader.Close();
                        return ret;
                    }
                    catch(Exception e)
                    {
                        reader.Close();
                        throw (e);
                    }                    
                }
            }
            catch (Exception e)
            {
                LogCenter.Instance.LogException(e);
            }
            return default(T);
        }

        public void Save<T>(T toSerialize)
        {
            try
            {
                string folder = ConfigurationManagement.Instance.ConfigurationPath;
                folder = folder + Path.DirectorySeparatorChar + "modules";
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                folder = folder + Path.DirectorySeparatorChar + "configs";
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                string file = folder + Path.DirectorySeparatorChar + adapter.GetAdapterInformation().Id + "-" + MetaData.GetMeta().Name + ".cfg";

                XmlSerializer serializer = new XmlSerializer(typeof(T));
                TextWriter writer = new StreamWriter(file, false);
                serializer.Serialize(writer, toSerialize);
                writer.Close();
            }
            catch (Exception e)
            {
                LogCenter.Instance.LogException(e);
            }
        }

        public virtual DynamicUserControl GetUserInterface()
        {
            return null;
        }

        public virtual bool ModuleStart()
        {
            return true;
        }

        public virtual bool ModuleStop()
        {
            return true;
        }

        public abstract PacketMainReturnType interiorMain(ref Packet in_packet);

        public int PacketMain(ref Packet in_packet)
        {
            try
            {
                return (int)interiorMain(ref in_packet);
            }
            catch (Exception e)
            {
                LogCenter.Instance.LogException(e);
            }
            return 0;
        }

        #endregion
    }
}
