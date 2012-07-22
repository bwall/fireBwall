using System;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using fireBwall.Logging;
using System.Globalization;

namespace fireBwall.Configuration
{
    public sealed class GeneralConfiguration
    {
        #region ConcurrentSingleton

        private static volatile GeneralConfiguration instance;
        private static object syncRoot = new Object();
        private ReaderWriterLock locker = new ReaderWriterLock();

        private GeneralConfiguration() { }

        /// <summary>
        /// Makes sure that the creation of a new GeneralConfiguration is threadsafe
        /// </summary>
        public static GeneralConfiguration Instance
        {
            get 
            {
                lock (syncRoot) 
                {
                    if (instance == null)
                        instance = new GeneralConfiguration();
                }
                return instance;               
            }
        }

        #endregion

        #region Serializable State

        [Serializable()]
        public class GeneralConfig
        {
            public string CurrentTheme = null;
            public string PreferredLanguage = null;
            public bool StartMinimized = false;
            public bool ShowPopups = true;
            public bool DeveloperMode = false;
            public uint MaxLogs = 5;
            public uint MaxPcapLogs = 25;
            public UpdateConfiguration UpdateConfig = new UpdateConfiguration();
        }

        [Serializable()]
        public class UpdateConfiguration
        {
            public bool CheckOnStartUp = true;
            public bool IntervaledChecks = true;
            public uint IntervalMinutes = 10;
        }

        #endregion

        #region Variables

        private GeneralConfig configuration = null;

        #endregion

        #region Event

        public event System.Threading.ThreadStart LanguageChanged;

        #endregion

        #region Members

        public string PreferredLanguage
        {
            get
            {
                string ret = "en";
                try
                {
                    locker.AcquireReaderLock(new TimeSpan(0, 1, 0));
                    try
                    {
                        if (configuration == null)
                            Load();
                        ret = "" + configuration.PreferredLanguage;
                    }
                    catch(Exception e)
                    {
                        LogCenter.Instance.LogException(e);
                    }
                    finally
                    {
                        locker.ReleaseReaderLock();
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
                    locker.AcquireWriterLock(new TimeSpan(0, 1, 0));
                    try
                    {
                        if (configuration == null)
                            Load();
                        configuration.PreferredLanguage = value;
                    }
                    catch (Exception e)
                    {
                        LogCenter.Instance.LogException(e);
                    }
                    finally
                    {
                        locker.ReleaseWriterLock();
                    }
                }
                catch (ApplicationException ex)
                {
                    LogCenter.Instance.LogException(ex);
                }
                if (LanguageChanged != null)
                    LanguageChanged();
            }
        }

        public bool DeveloperMode
        {
            get
            {
                bool ret = false;
                try
                {
                    locker.AcquireReaderLock(new TimeSpan(0, 1, 0));
                    try
                    {
                        if (configuration == null)
                            Load();
                        ret = configuration.DeveloperMode;
                    }
                    catch (Exception e)
                    {
                        LogCenter.Instance.LogException(e);
                    }
                    finally
                    {
                        locker.ReleaseReaderLock();
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
                    locker.AcquireWriterLock(new TimeSpan(0, 1, 0));
                    try
                    {
                        if (configuration == null)
                            Load();
                        configuration.DeveloperMode = value;
                    }
                    catch (Exception e)
                    {
                        LogCenter.Instance.LogException(e);
                    }
                    finally
                    {
                        locker.ReleaseWriterLock();
                    }
                }
                catch (ApplicationException ex)
                {
                    LogCenter.Instance.LogException(ex);
                }
            }
        }

        public bool StartMinimized
        {
            get
            {
                bool ret = false;
                try
                {
                    locker.AcquireReaderLock(new TimeSpan(0, 1, 0));
                    try
                    {
                        if (configuration == null)
                            Load();
                        ret = configuration.StartMinimized;
                    }
                    catch (Exception e)
                    {
                        LogCenter.Instance.LogException(e);
                    }
                    finally
                    {
                        locker.ReleaseReaderLock();
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
                    locker.AcquireWriterLock(new TimeSpan(0, 1, 0));
                    try
                    {
                        if (configuration == null)
                            Load();
                        configuration.StartMinimized = value;
                    }
                    catch (Exception e)
                    {
                        LogCenter.Instance.LogException(e);
                    }
                    finally
                    {
                        locker.ReleaseWriterLock();
                    }
                }
                catch (ApplicationException ex)
                {
                    LogCenter.Instance.LogException(ex);
                }
            }
        }

        public bool ShowPopups
        {
            get
            {
                bool ret = true;
                try
                {
                    locker.AcquireReaderLock(new TimeSpan(0, 1, 0));
                    try
                    {
                        if (configuration == null)
                            Load();
                        ret = configuration.ShowPopups;
                    }
                    catch (Exception e)
                    {
                        LogCenter.Instance.LogException(e);
                    }
                    finally
                    {
                        locker.ReleaseReaderLock();
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
                    locker.AcquireWriterLock(new TimeSpan(0, 1, 0));
                    try
                    {
                        if (configuration == null)
                            Load();
                        configuration.ShowPopups = value;
                    }
                    catch (Exception e)
                    {
                        LogCenter.Instance.LogException(e);
                    }
                    finally
                    {
                        locker.ReleaseWriterLock();
                    }
                }
                catch (ApplicationException ex)
                {
                    LogCenter.Instance.LogException(ex);
                }
            }
        }

        public uint MaxLogs
        {
            get
            {
                uint ret = 25;
                try
                {
                    locker.AcquireReaderLock(new TimeSpan(0, 1, 0));
                    try
                    {
                        if (configuration == null)
                            Load();
                        ret = configuration.MaxLogs;
                    }
                    catch (Exception e)
                    {
                        LogCenter.Instance.LogException(e);
                    }
                    finally
                    {
                        locker.ReleaseReaderLock();
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
                    locker.AcquireWriterLock(new TimeSpan(0, 1, 0));
                    try
                    {
                        if (configuration == null)
                            Load();
                        configuration.MaxLogs = value;
                    }
                    catch (Exception e)
                    {
                        LogCenter.Instance.LogException(e);
                    }
                    finally
                    {
                        locker.ReleaseWriterLock();
                    }
                }
                catch (ApplicationException ex)
                {
                    LogCenter.Instance.LogException(ex);
                }
            }
        }

        public uint MaxPcapLogs
        {
            get
            {
                uint ret = 5;
                try
                {
                    locker.AcquireReaderLock(new TimeSpan(0, 1, 0));
                    try
                    {
                        if (configuration == null)
                            Load();
                        ret = configuration.MaxPcapLogs;
                    }
                    catch (Exception e)
                    {
                        LogCenter.Instance.LogException(e);
                    }
                    finally
                    {
                        locker.ReleaseReaderLock();
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
                    locker.AcquireWriterLock(new TimeSpan(0, 1, 0));
                    try
                    {
                        if (configuration == null)
                            Load();
                        configuration.MaxPcapLogs = value;
                    }
                    catch (Exception e)
                    {
                        LogCenter.Instance.LogException(e);
                    }
                    finally
                    {
                        locker.ReleaseWriterLock();
                    }
                }
                catch (ApplicationException ex)
                {
                    LogCenter.Instance.LogException(ex);
                }
            }
        }

        public bool CheckUpdateOnStartup
        {
            get
            {
                bool ret = true;
                try
                {
                    locker.AcquireReaderLock(new TimeSpan(0, 1, 0));
                    try
                    {
                        if (configuration == null)
                            Load();
                        if (configuration.UpdateConfig == null)
                            configuration.UpdateConfig = new UpdateConfiguration();
                        ret = configuration.UpdateConfig.CheckOnStartUp;
                    }
                    catch (Exception e)
                    {
                        LogCenter.Instance.LogException(e);
                    }
                    finally
                    {
                        locker.ReleaseReaderLock();
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
                    locker.AcquireWriterLock(new TimeSpan(0, 1, 0));
                    try
                    {
                        if (configuration == null)
                            Load();
                        if (configuration.UpdateConfig == null)
                            configuration.UpdateConfig = new UpdateConfiguration();
                        configuration.UpdateConfig.CheckOnStartUp = value;
                    }
                    catch (Exception e)
                    {
                        LogCenter.Instance.LogException(e);
                    }
                    finally
                    {
                        locker.ReleaseWriterLock();
                    }
                }
                catch (ApplicationException ex)
                {
                    LogCenter.Instance.LogException(ex);
                }
            }
        }

        public bool IntervaledUpdateChecks
        {
            get
            {
                bool ret = true;
                try
                {
                    locker.AcquireReaderLock(new TimeSpan(0, 1, 0));
                    try
                    {
                        if (configuration == null)
                            Load();
                        if (configuration.UpdateConfig == null)
                            configuration.UpdateConfig = new UpdateConfiguration();
                        ret = configuration.UpdateConfig.IntervaledChecks;
                    }
                    catch (Exception e)
                    {
                        LogCenter.Instance.LogException(e);
                    }
                    finally
                    {
                        locker.ReleaseReaderLock();
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
                    locker.AcquireWriterLock(new TimeSpan(0, 1, 0));
                    try
                    {
                        if (configuration == null)
                            Load();
                        if (configuration.UpdateConfig == null)
                            configuration.UpdateConfig = new UpdateConfiguration();
                        configuration.UpdateConfig.IntervaledChecks = value;
                    }
                    catch (Exception e)
                    {
                        LogCenter.Instance.LogException(e);
                    }
                    finally
                    {
                        locker.ReleaseWriterLock();
                    }
                }
                catch (ApplicationException ex)
                {
                    LogCenter.Instance.LogException(ex);
                }
            }
        }

        public uint IntervaledUpdateMinutes
        {
            get
            {
                uint ret = 10;
                try
                {
                    locker.AcquireReaderLock(new TimeSpan(0, 1, 0));
                    try
                    {
                        if (configuration == null)
                            Load();
                        if (configuration.UpdateConfig == null)
                            configuration.UpdateConfig = new UpdateConfiguration();
                        ret = configuration.UpdateConfig.IntervalMinutes;
                    }
                    catch (Exception e)
                    {
                        LogCenter.Instance.LogException(e);
                    }
                    finally
                    {
                        if(locker.IsReaderLockHeld)
                            locker.ReleaseReaderLock();
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
                    bool upgraded = false;
                    LockCookie upgrade = new LockCookie();
                    if (locker.IsReaderLockHeld)
                    {
                        upgrade = locker.UpgradeToWriterLock(new TimeSpan(0, 1, 0));
                        upgraded = true;
                    }
                    else
                        locker.AcquireWriterLock(new TimeSpan(0, 1, 0));
                    try
                    {
                        if (configuration == null)
                            Load();
                        if (configuration.UpdateConfig == null)
                            configuration.UpdateConfig = new UpdateConfiguration();
                        configuration.UpdateConfig.IntervalMinutes = value;
                    }
                    catch (Exception e)
                    {
                        LogCenter.Instance.LogException(e);
                    }
                    finally
                    {
                        if (upgraded)
                            locker.DowngradeFromWriterLock(ref upgrade);
                        else
                            locker.ReleaseWriterLock();
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

        public bool Save()
        {
            try
            {
                locker.AcquireReaderLock(new TimeSpan(0, 1, 0));
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(GeneralConfig));
                    TextWriter writer = new StreamWriter(ConfigurationManagement.Instance.ConfigurationPath + Path.DirectorySeparatorChar + "general.cfg");
                    serializer.Serialize(writer, configuration);
                    writer.Close();
                    writer.Dispose();
                }
                catch
                {
                    return false;
                }
                finally
                {
                    if(locker.IsReaderLockHeld)
                        locker.ReleaseReaderLock();
                }
                return true;
            }
            catch (ApplicationException ex)
            {
                LogCenter.Instance.LogException(ex);
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
                        if (File.Exists(ConfigurationManagement.Instance.ConfigurationPath + Path.DirectorySeparatorChar + "general.cfg"))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(GeneralConfig));
                            TextReader reader = new StreamReader(ConfigurationManagement.Instance.ConfigurationPath + Path.DirectorySeparatorChar + "general.cfg");
                            configuration = (GeneralConfig)serializer.Deserialize(reader);
                            reader.Close();
                            reader.Dispose();
                        }
                        else
                        {
                            configuration = new GeneralConfig();
                        }
                    }
                    catch (Exception e)
                    {
                        LogCenter.Instance.LogException(e);
                        configuration = new GeneralConfig();
                    }
                    finally
                    {
                        if (configuration.PreferredLanguage == null)
                        {
                            configuration.PreferredLanguage = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
                        }
                        switch (configuration.PreferredLanguage)
                        {
                            case "zh":
                            case "en":
                            case "de":
                            case "pt":
                            case "ru":
                            case "es":
                            case "ja":
                            case "it":
                            case "fr":
                            case "he":
                            case "nl":
                                break;
                            default:
                                configuration.PreferredLanguage = "en";
                                break;
                        }
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
                LogCenter.Instance.LogException(ex);
                return false;
            }
        }

        #endregion
    }
}
