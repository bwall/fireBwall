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
                if (configPath == null)
                {
                    try
                    {
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
                }
                return configPath;
            }
            set
            {
                configPath = value;
                if (!Directory.Exists(configPath))
                {
                    Directory.CreateDirectory(configPath);
                }
            }
        }

        #endregion

        #region Functions

        public void SaveAllConfigurations()
        {
            GeneralConfiguration.Instance.Save();
            IPLists.Instance.Save();
        }

        public void LoadAllConfigurations()
        {
            GeneralConfiguration.Instance.Load();
            IPLists.Instance.Load();
            LogCenter.Instance.ToString();
        }

        #endregion
    }
}
