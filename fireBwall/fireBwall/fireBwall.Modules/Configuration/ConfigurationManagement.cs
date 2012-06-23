using System;
using System.IO;
using System.Threading;
using fireBwall.Logging;

namespace fireBwall.Configuration
{
    /// <summary>
    /// Manages configurations for this running instance
    /// </summary>
    public sealed class ConfigurationManagement
    {
        #region ConcurrentSingleton

        private static volatile ConfigurationManagement instance;
        private static object syncRoot = new Object();
        private ReaderWriterLock rwlock = new ReaderWriterLock();

        private ConfigurationManagement() { }

        public static ConfigurationManagement Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        instance = new ConfigurationManagement();
                    }
                }
                return instance;
            }
        }

        #endregion

        #region Variables

        private string configPath = null;

        #endregion

        #region Members

        public string ConfigurationPath
        {
            get
            {
                string ret = null;
                try
                {
                    rwlock.AcquireReaderLock(new TimeSpan(0, 1, 0));
                    try
                    {
                        if (configPath == null)
                        {
                            LockCookie lc = new LockCookie();
                            try
                            {
                                lc = rwlock.UpgradeToWriterLock(new TimeSpan(0, 1, 0));
                                configPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + Path.DirectorySeparatorChar + "fireBwall";
                                if (!Directory.Exists(configPath))
                                {
                                    Directory.CreateDirectory(configPath);
                                }
                            }
                            catch (ApplicationException ex)
                            {
                                LogCenter.Instance.LogException(ex);
                            }
                            finally
                            {
                                rwlock.DowngradeFromWriterLock(ref lc);
                            }
                        }
                        ret = "" + configPath;
                    }
                    catch (Exception e)
                    {
                        LogCenter.Instance.LogException(e);
                    }
                    finally
                    {
                        rwlock.ReleaseLock();
                    }
                }
                catch (ApplicationException ex)
                {
                    LogCenter.Instance.LogException(ex);
                }
                return ret;
            }
            set
            {
                try
                {
                    rwlock.AcquireWriterLock(new TimeSpan(0, 1, 0));
                    try
                    {
                        configPath = value;
                        if (!Directory.Exists(configPath))
                        {
                            Directory.CreateDirectory(configPath);
                        }
                    }
                    finally
                    {
                        rwlock.ReleaseWriterLock();
                    }
                }
                catch (ApplicationException ex)
                {
                    LogCenter.Instance.LogException(ex);
                }
            }
        }

        #endregion

        #region Functions

        public void SaveAllConfigurations()
        {
            GeneralConfiguration.Instance.Save();
            ThemeConfiguration.Instance.Save();
            IPLists.Instance.Save();
        }

        public void LoadAllConfigurations()
        {
            GeneralConfiguration.Instance.Load();
            ThemeConfiguration.Instance.Load();
            IPLists.Instance.Load();
        }

        #endregion
    }
}
