using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using fireBwall.Logging;

namespace fireBwall.Configuration
{
    public sealed class ThemeConfiguration
    {
        #region ConcurrentSingleton

        private static volatile ThemeConfiguration instance;
        private static object syncRoot = new Object();
        private ReaderWriterLock locker = new ReaderWriterLock();

        private ThemeConfiguration() { }

        /// <summary>
        /// Makes sure that the creation of a new GeneralConfiguration is threadsafe
        /// </summary>
        public static ThemeConfiguration Instance
        {
            get 
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new ThemeConfiguration();
                }
                return instance; 
            }
        }

        #endregion

        #region Variables

        Dictionary<string, ColorScheme> schemes = new Dictionary<string, ColorScheme>();

        #endregion

        #region Members

        public Dictionary<string, ColorScheme> Schemes
        {
            get
            {
                Dictionary<string, ColorScheme> ret = new Dictionary<string, ColorScheme>();
                try
                {
                    locker.AcquireReaderLock(new TimeSpan(0, 1, 0));
                    try
                    {
                        ret = new Dictionary<string, ColorScheme>(schemes);
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
        }

        #endregion

        #region Events

        public event System.Threading.ThreadStart ThemeChanged;

        #endregion

        #region Functions

        public bool IsValid(ColorScheme cs)
        {
            bool ret = true;
            try
            {
                locker.AcquireReaderLock(new TimeSpan(0, 1, 0));
                try
                {
                    if (!cs.colors.ContainsKey("FlatButtonBack")
                        || !cs.colors.ContainsKey("FlatButtonFore")
                        || !cs.colors.ContainsKey("ButtonBack")
                        || !cs.colors.ContainsKey("ButtonFore")
                        || !cs.colors.ContainsKey("GridColor")
                        || !cs.colors.ContainsKey("GridForeColor")
                        || !cs.colors.ContainsKey("GridBackColor")
                        || !cs.colors.ContainsKey("GridHeaderFore")
                        || !cs.colors.ContainsKey("GridHeaderBack")
                        || !cs.colors.ContainsKey("GridCellFore")
                        || !cs.colors.ContainsKey("GridCellBack")
                        || !cs.colors.ContainsKey("GridSelectCellBack")
                        || !cs.colors.ContainsKey("GridSelectCellFore")
                        || !cs.colors.ContainsKey("Back")
                        || !cs.colors.ContainsKey("Fore"))
                        ret = false;
                    if (string.IsNullOrWhiteSpace(cs.Name) || string.IsNullOrWhiteSpace(cs.Base64Image))
                        ret = false;
                }
                finally
                {
                    locker.ReleaseReaderLock();
                }
            }
            catch (ApplicationException ex)
            {
                LogCenter.Instance.LogException(ex);
                ret = false;
            }
            return ret;
        }

        public Image GetCurrentBanner()
        {
            Image ret = null;
            try
            {
                locker.AcquireReaderLock(new TimeSpan(0, 1, 0));
                try
                {
                    string currentTheme = GeneralConfiguration.Instance.CurrentTheme;
                    MemoryStream ms = new MemoryStream(Convert.FromBase64String(schemes[currentTheme].Base64Image));
                    ret = Bitmap.FromStream(ms);
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

        public void SetColorScheme(Control control)
        {
            try
            {
                bool locked = false;
                if (!locker.IsReaderLockHeld)
                {
                    locked = true;
                    locker.AcquireReaderLock(new TimeSpan(0, 1, 0));
                }
                try
                {
                    string currentTheme = GeneralConfiguration.Instance.CurrentTheme;
                    if (control is Button)
                    {
                        if (((Button)control).FlatStyle == FlatStyle.Flat)
                        {
                            control.BackColor = schemes[currentTheme].colors["FlatButtonBack"].ToSystemColor();
                            control.ForeColor = schemes[currentTheme].colors["FlatButtonFore"].ToSystemColor();
                        }
                        else
                        {
                            control.BackColor = schemes[currentTheme].colors["ButtonBack"].ToSystemColor();
                            control.ForeColor = schemes[currentTheme].colors["ButtonFore"].ToSystemColor();
                        }
                    }
                    else if (control is DataGridView)
                    {
                        ((DataGridView)control).GridColor = schemes[currentTheme].colors["GridColor"].ToSystemColor();
                        ((DataGridView)control).ForeColor = schemes[currentTheme].colors["GridForeColor"].ToSystemColor();
                        ((DataGridView)control).BackgroundColor = schemes[currentTheme].colors["GridBackColor"].ToSystemColor();
                        ((DataGridView)control).ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle() { ForeColor = schemes[currentTheme].colors["GridHeaderFore"].ToSystemColor(), BackColor = schemes[currentTheme].colors["GridHeaderBack"].ToSystemColor(), SelectionForeColor = schemes[currentTheme].colors["GridHeaderFore"].ToSystemColor(), SelectionBackColor = schemes[currentTheme].colors["GridHeaderBack"].ToSystemColor() };
                        ((DataGridView)control).DefaultCellStyle = new DataGridViewCellStyle() { ForeColor = schemes[currentTheme].colors["GridCellFore"].ToSystemColor(), BackColor = schemes[currentTheme].colors["GridCellBack"].ToSystemColor(), SelectionBackColor = schemes[currentTheme].colors["GridSelectCellBack"].ToSystemColor(), SelectionForeColor = schemes[currentTheme].colors["GridSelectCellFore"].ToSystemColor() };
                    }
                    else
                    {
                        control.BackColor = schemes[currentTheme].colors["Back"].ToSystemColor();
                        control.ForeColor = schemes[currentTheme].colors["Fore"].ToSystemColor();
                    }
                    foreach (Control c in control.Controls)
                    {
                        if (c is fireBwall.UI.DynamicUserControl)
                        {
                            //No need
                        }
                        else
                            SetColorScheme(c);
                    }
                }
                finally
                {
                    if(locked)
                        locker.ReleaseReaderLock();
                }
            }
            catch (ApplicationException ex)
            {
                LogCenter.Instance.LogException(ex);
            }
        }

        public void ChangeTheme(string theme, bool force = false)
        {
            try
            {
                locker.AcquireReaderLock(new TimeSpan(0, 1, 0));
                try
                {
                    if (schemes.ContainsKey(theme) && IsValid(schemes[theme]))
                    {
                        bool changed = GeneralConfiguration.Instance.CurrentTheme != theme;
                        GeneralConfiguration.Instance.CurrentTheme = theme;
                        if (ThemeChanged != null && (force || changed))
                            ThemeChanged();
                    }
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
        }

        public void Load(string file, bool set = false)
        {
            if (File.Exists(file))
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
                        XmlSerializer serializer = new XmlSerializer(typeof(ColorScheme));
                        TextReader reader = new StreamReader(file);
                        ColorScheme scheme = (ColorScheme)serializer.Deserialize(reader);
                        reader.Close();
                        schemes[scheme.Name] = scheme;
                        if (set)
                            ChangeTheme(scheme.Name, true);
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

        public void Load()
        {
            if (Directory.Exists(ConfigurationManagement.Instance.ConfigurationPath + Path.DirectorySeparatorChar + "themes"))
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
                        foreach (string file in Directory.GetFiles(ConfigurationManagement.Instance.ConfigurationPath + Path.DirectorySeparatorChar + "themes"))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(ColorScheme));
                            TextReader reader = new StreamReader(file);
                            ColorScheme scheme = (ColorScheme)serializer.Deserialize(reader);
                            reader.Close();
                            schemes[scheme.Name] = scheme;
                        }
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

        public void CreateDefaultThemes()
        {
            ColorScheme scheme = new ColorScheme();
            ColorScheme s = new ColorScheme();
            scheme.colors["FlatButtonBack"] = new ColorScheme.Color(System.Drawing.Color.Transparent);
            scheme.colors["FlatButtonFore"] = new ColorScheme.Color(System.Drawing.Color.White);
            scheme.colors["ButtonBack"] = new ColorScheme.Color(System.Drawing.Color.WhiteSmoke);
            scheme.colors["ButtonFore"] = new ColorScheme.Color(System.Drawing.Color.DarkBlue);
            scheme.colors["GridColor"] = new ColorScheme.Color(System.Drawing.Color.WhiteSmoke);
            scheme.colors["GridForeColor"] = new ColorScheme.Color(System.Drawing.Color.DarkBlue);
            scheme.colors["GridBackColor"] = new ColorScheme.Color(System.Drawing.Color.WhiteSmoke);
            scheme.colors["GridHeaderFore"] = new ColorScheme.Color(System.Drawing.Color.DarkBlue);
            scheme.colors["GridHeaderBack"] = new ColorScheme.Color(System.Drawing.Color.WhiteSmoke);
            scheme.colors["GridCellFore"] = new ColorScheme.Color(System.Drawing.Color.DarkBlue);
            scheme.colors["GridCellBack"] = new ColorScheme.Color(System.Drawing.Color.WhiteSmoke);
            scheme.colors["GridSelectCellBack"] = new ColorScheme.Color(System.Drawing.Color.LightBlue);
            scheme.colors["GridSelectCellFore"] = new ColorScheme.Color(System.Drawing.Color.DarkBlue);
            scheme.colors["Back"] = new ColorScheme.Color(System.Drawing.Color.WhiteSmoke);
            scheme.colors["Fore"] = new ColorScheme.Color(System.Drawing.Color.DarkBlue);
            scheme.Name = "Light";
            s.Base64Image = @"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".Replace("\r", "").Replace("\n", "").Replace(" ", "").Replace("\t", "");
            scheme.Base64Image = s.Base64Image;
            AddScheme(scheme);
            s.Name = "Mordor";
            s.colors["FlatButtonBack"] = new ColorScheme.Color(System.Drawing.Color.Black);
            s.colors["FlatButtonFore"] = new ColorScheme.Color(System.Drawing.Color.White);
            s.colors["ButtonBack"] = new ColorScheme.Color(System.Drawing.Color.FromArgb(64, 0, 0));
            s.colors["ButtonFore"] = new ColorScheme.Color(System.Drawing.Color.White);
            s.colors["GridColor"] = new ColorScheme.Color(System.Drawing.Color.Black);
            s.colors["GridForeColor"] = new ColorScheme.Color(System.Drawing.Color.White);
            s.colors["GridBackColor"] = new ColorScheme.Color(System.Drawing.Color.FromArgb(64, 0, 0));
            s.colors["GridHeaderFore"] = new ColorScheme.Color(System.Drawing.Color.White);
            s.colors["GridHeaderBack"] = new ColorScheme.Color(System.Drawing.Color.FromArgb(64, 32, 16));
            s.colors["GridCellFore"] = new ColorScheme.Color(System.Drawing.Color.White);
            s.colors["GridCellBack"] = new ColorScheme.Color(System.Drawing.Color.FromArgb(64, 0, 0));
            s.colors["GridSelectCellBack"] = new ColorScheme.Color(System.Drawing.Color.FromArgb(128, 0, 0));
            s.colors["GridSelectCellFore"] = new ColorScheme.Color(System.Drawing.Color.White);
            s.colors["Back"] = new ColorScheme.Color(System.Drawing.Color.FromArgb(16, 0, 0));
            s.colors["Fore"] = new ColorScheme.Color(System.Drawing.Color.White);
            AddScheme(s);
        }

        public void AddScheme(ColorScheme scheme)
        {
            if (!IsValid(scheme))
                return;
            try
            {
                bool upgraded = false;
                LockCookie upgrade = new LockCookie();
                if (locker.IsReaderLockHeld)
                    upgrade = locker.UpgradeToWriterLock(new TimeSpan(0, 1, 0));
                else
                    locker.AcquireWriterLock(new TimeSpan(0, 1, 0));
                try
                {
                    schemes[scheme.Name] = scheme;
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

        public void Save()
        {
            if (!Directory.Exists(ConfigurationManagement.Instance.ConfigurationPath + Path.DirectorySeparatorChar + "themes"))
                Directory.CreateDirectory(ConfigurationManagement.Instance.ConfigurationPath + Path.DirectorySeparatorChar + "themes");
            try
            {
                locker.AcquireReaderLock(new TimeSpan(0, 1, 0));
                try
                {
                    foreach (ColorScheme scheme in schemes.Values)
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ColorScheme));
                        TextWriter writer = new StreamWriter(ConfigurationManagement.Instance.ConfigurationPath + Path.DirectorySeparatorChar + "themes" + Path.DirectorySeparatorChar + scheme.Name);
                        serializer.Serialize(writer, scheme);
                        writer.Close();
                    }
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
        }
        #endregion
    }
}
