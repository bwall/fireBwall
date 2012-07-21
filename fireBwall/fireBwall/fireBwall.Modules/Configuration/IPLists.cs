using System;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using fireBwall.Utils;

namespace fireBwall.Configuration
{
    public sealed class IPLists
    {
        #region ConcurrentSingleton

        private static volatile IPLists instance;
        private static object syncRoot = new Object();
        private ReaderWriterLock locker = new ReaderWriterLock();

        private IPLists() { }

        /// <summary>
        /// Makes sure that the creation of a new GeneralConfiguration is threadsafe
        /// </summary>
        public static IPLists Instance
        {
            get 
            {
                lock (syncRoot) 
                {
                    if (instance == null)
                        instance = new IPLists();
                }
                return instance;               
            }
        }

        #endregion

        #region Serializable State
        
        public class IPList
        {
            private SerializableDictionary<IPAddr, long> list = new SerializableDictionary<IPAddr, long>();

            [NonSerialized]
            private ReaderWriterLock locker = new ReaderWriterLock();

            public bool Contains(IPAddr ip, long secondsOld = -1)
            {
                bool contains = false;
                try
                {
                    locker.AcquireReaderLock(new TimeSpan(0, 1, 0));
                    try
                    {
                        if (secondsOld < 0)
                        {
                            contains = list.ContainsKey(ip);
                        }
                        else
                        {
                            contains = (list.ContainsKey(ip) && new DateTime(list[ip]).AddSeconds(secondsOld).CompareTo(DateTime.UtcNow) < 0);
                        }
                    }
                    finally
                    {
                        locker.ReleaseReaderLock();
                    }
                }
                catch (ApplicationException a)
                {
                    Logging.LogCenter.Instance.LogException(a);
                }
                return contains;
            }

            public void Remove(IPAddr ip)
            {
                try
                {
                    locker.AcquireReaderLock(new TimeSpan(0, 1, 0));
                    try
                    {
                        if (list.ContainsKey(ip))
                        {
                            LockCookie lc = new LockCookie();
                            try
                            {
                                lc = locker.UpgradeToWriterLock(new TimeSpan(0, 1, 0));
                                try
                                {
                                    list.Remove(ip);
                                }
                                finally
                                {
                                    locker.DowngradeFromWriterLock(ref lc);
                                }
                            }
                            catch (ApplicationException e)
                            {
                                Logging.LogCenter.Instance.LogException(e);
                            }
                        }
                    }
                    finally
                    {
                        locker.ReleaseReaderLock();
                    }
                }
                catch (ApplicationException aex)
                {
                    Logging.LogCenter.Instance.LogException(aex);
                }
            }

            public void Add(IPAddr ip)
            {
                try
                {
                    locker.AcquireWriterLock(new TimeSpan(0, 1, 0));
                    try
                    {
                        if (list.ContainsKey(ip))
                            list[ip] = DateTime.UtcNow.Ticks;
                        else
                            list.Add(ip, DateTime.UtcNow.Ticks);
                    }
                    finally
                    {
                        locker.ReleaseWriterLock();
                    }
                }
                catch (ApplicationException e)
                {
                    Logging.LogCenter.Instance.LogException(e);
                }                   
            }            
        }

        #endregion

        #region Variables

        SerializableDictionary<string, IPList> iplists = new SerializableDictionary<string, IPList>();

        #endregion

        #region Functions

        public bool InList(string list, IPAddr ip, long secondsOld = -1)
        {
            bool ret = false;
            try
            {
                locker.AcquireReaderLock(new TimeSpan(0, 1, 0));
                try
                {
                    IPList ipl;
                    if (iplists.TryGetValue(list, out ipl))
                    {
                        if (ipl.Contains(ip, secondsOld))
                            ret = true;
                    }
                }
                finally
                {
                   locker.ReleaseReaderLock();
                }
            }
            catch (ApplicationException ex)
            {
                Logging.LogCenter.Instance.LogException(ex);
            }
            return ret;
        }

        public void AddToList(string list, IPAddr ip)
        {
            try
            {
                LockCookie upgrade = new LockCookie();
                bool upgraded = false;
                if (locker.IsReaderLockHeld)
                {
                    upgrade = locker.UpgradeToWriterLock(new TimeSpan(0, 1, 0));
                    upgraded = true;
                }
                else
                    locker.AcquireWriterLock(new TimeSpan(0, 1, 0));
                try
                {
                    if (!iplists.ContainsKey(list))
                    {
                        iplists[list] = new IPList();
                    }
                    iplists[list].Add(ip);
                }
                finally
                {
                    if (upgraded)
                        locker.DowngradeFromWriterLock(ref upgrade);
                    else
                        locker.ReleaseWriterLock();
                }
            }
            catch (ApplicationException a)
            {
                Logging.LogCenter.Instance.LogException(a);
            }
        }

        public bool Save()
        {
            try
            {
                locker.AcquireReaderLock(new TimeSpan(0, 1, 0));
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(SerializableDictionary<string, IPList>));
                    TextWriter writer = new StreamWriter(ConfigurationManagement.Instance.ConfigurationPath + Path.DirectorySeparatorChar + "IPLists.cfg");
                    serializer.Serialize(writer, iplists);
                    writer.Close();
                }
                catch
                {
                    return false;
                }
                finally
                {
                    if (locker.IsReaderLockHeld)
                        locker.ReleaseReaderLock();
                }
                return true;
            }
            catch (ApplicationException ex)
            {
                Logging.LogCenter.Instance.LogException(ex);
                return false;
            }
        }

        public bool Load()
        {
            try
            {
                LockCookie upgrade = new LockCookie();
                bool upgraded = false;
                if (locker.IsReaderLockHeld)
                {
                    upgrade = locker.UpgradeToWriterLock(new TimeSpan(0, 1, 0));
                    upgraded = true;
                }
                else
                    locker.AcquireWriterLock(new TimeSpan(0, 1, 0));
                try
                {
                    try
                    {
                        if (File.Exists(ConfigurationManagement.Instance.ConfigurationPath + Path.DirectorySeparatorChar + "IPLists.cfg"))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(SerializableDictionary<string, IPList>));
                            TextReader reader = new StreamReader(ConfigurationManagement.Instance.ConfigurationPath + Path.DirectorySeparatorChar + "IPLists.cfg");
                            iplists = (SerializableDictionary<string, IPList>)serializer.Deserialize(reader);
                            reader.Close();
                        }
                        else
                        {
                            iplists = new SerializableDictionary<string,IPList>();
                        }
                    }
                    catch (Exception e)
                    {
                        Logging.LogCenter.Instance.LogException(e);
                        iplists = new SerializableDictionary<string, IPList>();
                    }
                }
                finally
                {
                    if (upgraded)
                        locker.DowngradeFromWriterLock(ref upgrade);
                    else
                        locker.ReleaseWriterLock();
                }
                return true;
            }
            catch (ApplicationException ex)
            {
                Logging.LogCenter.Instance.LogException(ex);
                return false;
            }
        }

        #endregion
    }
}
