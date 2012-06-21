using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using fireBwall.Modules;
using fireBwall.Filters.NDIS;
using fireBwall.Configuration;
using fireBwall.Logging;
using System.Reflection;
using fireBwall.Utils;

namespace fireBwall.Modules
{
    public class ModuleList
    {
        [Serializable]
        public class ModuleOrder
        {
            [Serializable]
            public class ModulePair
            {
                public bool Enabled = false;
                public string Name = "";
            }
            public ModulePair[] Order = new ModulePair[0];
        }

        List<int> ProcessingIndex = new List<int>();
        List<bool> enabled = new List<bool>();
        List<NDISModule> modules = new List<NDISModule>();
        object padlock = new object();
        Dictionary<string, string> loadedMods = new Dictionary<string, string>();
        List<KeyValuePair<bool, string>> moduleOrder = new List<KeyValuePair<bool, string>>();
        INDISFilter na;
        static Dictionary<string, Assembly> loadedAssemblies = new Dictionary<string, Assembly>();

        public void SaveModuleOrder()
        {
            string folder = ConfigurationManagement.Instance.ConfigurationPath;
            folder = folder + Path.DirectorySeparatorChar + "Adapters";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            folder = folder + Path.DirectorySeparatorChar + na.GetAdapterInformation().Id;
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            string file = folder + Path.DirectorySeparatorChar + "modules.cfg";

            List<ModuleOrder.ModulePair> pairs = new List<ModuleOrder.ModulePair>();
            foreach(KeyValuePair<bool, string> p in moduleOrder)
            {
                pairs.Add(new ModuleOrder.ModulePair() { Enabled = p.Key, Name = p.Value });
            }
            ModuleOrder mo = new ModuleOrder() { Order = pairs.ToArray() };

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ModuleOrder));
                TextWriter writer = new StreamWriter(file);
                serializer.Serialize(writer, mo);
                writer.Close();
            }
            catch (Exception e)
            {
                LogCenter.Instance.LogException(e);
            }
        }

        public void LoadModuleOrder()
        {
            string folder = ConfigurationManagement.Instance.ConfigurationPath;
            folder = folder + Path.DirectorySeparatorChar + "Adapters";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            folder = folder + Path.DirectorySeparatorChar + na.GetAdapterInformation().Id;
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            string file = folder + Path.DirectorySeparatorChar + "modules.cfg";
            if (!File.Exists(file))
            {
                moduleOrder = new List<KeyValuePair<bool, string>>();
                return;
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ModuleOrder));
                TextReader reader = new StreamReader(file);
                ModuleOrder mo = (ModuleOrder)serializer.Deserialize(reader);
                reader.Close();
                moduleOrder = new List<KeyValuePair<bool,string>>();
                foreach (ModuleOrder.ModulePair p in mo.Order)
                {
                    moduleOrder.Add(new KeyValuePair<bool, string>(p.Enabled, p.Name));
                }
            }
            catch (Exception e)
            {
                LogCenter.Instance.LogException(e);
                moduleOrder = new List<KeyValuePair<bool, string>>();
            }
        }

        public ModuleList(INDISFilter na)
        {
            this.na = na;
            LoadModuleOrder();
        }

        public void LoadModule(string file)
        {
            NDISModule mod = null;
            lock (loadedAssemblies)
            {
                if (file.Contains("FirewallModule.dll") || !file.Contains(".dll"))
                    return;
                try
                {
                    if (loadedMods.ContainsValue(file))
                        return;
                    Assembly assembly;
                    if (!loadedAssemblies.TryGetValue(file, out assembly))
                    {
                        assembly = Assembly.Load(File.ReadAllBytes(file));
                        loadedAssemblies.Add(file, assembly);
                    }
                    Type[] type = assembly.GetTypes();
                    foreach (Type t in type)
                    {
                        if (typeof(NDISModule).IsAssignableFrom(t))
                        {
                            mod = (NDISModule)Activator.CreateInstance(t);
                            mod.Adapter = na;
                            AddModule(mod);
                            loadedMods.Add(mod.MetaData.GetMeta().Name, file);
                        }
                    }
                }
                catch (ArgumentException ae)
                {
                    LogCenter.Instance.LogEvent(new LogEvent("Module attempted load twice.", mod));
                    LogCenter.Instance.LogException(ae);
                }
                catch (Exception e)
                {
                    LogCenter.Instance.LogException(e);
                }
            }
        }

        public void LoadExternalModules()
        {
            if (Directory.Exists("modules"))
            {
                DirectoryInfo di = new DirectoryInfo("modules");
                foreach (FileInfo fi in di.GetFiles())
                {
                    LoadModule(fi.FullName);
                }
            }
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (Directory.Exists(Configuration.ConfigurationManagement.Instance.ConfigurationPath + Path.DirectorySeparatorChar + "modules"))
            {
                DirectoryInfo di = new DirectoryInfo(Configuration.ConfigurationManagement.Instance.ConfigurationPath + Path.DirectorySeparatorChar + "modules");
                foreach (FileInfo fi in di.GetFiles())
                {
                    LoadModule(fi.FullName);
                }
            }
        }

        public List<KeyValuePair<bool, string>> GetModuleOrder()
        {
            return moduleOrder;
        }

        public void UpdateModuleOrder(List<KeyValuePair<bool, string>> mO)
        {
            moduleOrder = mO;
            UpdateModuleOrder();
        }

        public void UpdateModuleOrder()
        {
            lock (padlock)
            {
                ProcessingIndex.Clear();
                List<int> indexes = new List<int>();
                if (moduleOrder.Count == 0)
                {
                    for (int i = 0; i < Count; i++)
                    {
                        if (!indexes.Contains(i))
                        {
                            ProcessingIndex.Add(i);
                            modules[i].ModuleStart();
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < moduleOrder.Count; i++)
                    {
                        int mindex = GetModuleIndex(moduleOrder[i].Value);
                        if (mindex != -1)
                        {
                            indexes.Add(mindex);
                            ProcessingIndex.Add(mindex);
                            if (enabled[mindex] != moduleOrder[i].Key)
                            {
                                if (enabled[mindex])
                                {
                                    modules[mindex].ModuleStop();
                                }
                                else
                                {
                                    modules[mindex].ModuleStart();
                                }
                                enabled[mindex] = moduleOrder[i].Key;
                            }
                        }
                    }

                    for (int i = 0; i < Count; i++)
                    {
                        if (!indexes.Contains(i))
                        {
                            ProcessingIndex.Add(i);
                        }
                    }
                }
                moduleOrder.Clear();
                for (int i = 0; i < Count; i++)
                {
                    NDISModule fm = GetModule(i, false);
                    moduleOrder.Add(new KeyValuePair<bool, string>(enabled[GetModuleIndex(fm.MetaData.GetMeta().Name)], fm.MetaData.GetMeta().Name));
                }
                SaveModuleOrder();
            }
        }

        void InsertPIndex(int oIndex, int nIndex)
        {
            if (oIndex == nIndex) return;
            lock (padlock)
            {
                int toMove = ProcessingIndex[oIndex];
                ProcessingIndex.RemoveAt(oIndex);
                if (nIndex < ProcessingIndex.Count)
                {
                    ProcessingIndex.Insert(nIndex, toMove);
                }
                else
                {
                    ProcessingIndex.Add(toMove);
                }
            }
        }

        public void AddModule(NDISModule fm)
        {
            lock (padlock)
            {
                modules.Add(fm);
                ProcessingIndex.Add(modules.Count - 1);
                enabled.Add(false);
            }
        }

        int GetModuleIndex(string name)
        {
            for (int x = 0; x < modules.Count; x++)
            {
                if (modules[x].MetaData.GetMeta().Name == name)
                {
                    return x;
                }
            }
            return -1;
        }

        public void ShutdownAllModules()
        {
            lock (padlock)
            {
                for (int x = 0; x < modules.Count; x++)
                {
                    if (enabled[x])
                    {
                        modules[x].ModuleStop();
                        enabled[x] = false;
                    }
                }
            }
        }

        public NDISModule GetModule(int index, bool nullOnDisabled = true)
        {
            lock (padlock)
            {
                if (nullOnDisabled)
                {
                    if (enabled[ProcessingIndex[index]])
                        return modules[ProcessingIndex[index]];
                    else
                        return null;
                }
                return modules[ProcessingIndex[index]];
            }
        }

        public int Count
        {
            get
            {
                return modules.Count;
            }
        }
    }
}
