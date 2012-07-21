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

        private ThemeConfiguration() { }

        /// <summary>
        /// Makes sure that the creation of a new GeneralConfiguration is threadsafe
        /// </summary>
        public static ThemeConfiguration Instance
        {
            get 
            {
                if (instance == null)
                    instance = new ThemeConfiguration();
                return instance; 
            }
        }

        #endregion

        #region Functions

        static Image banner = null;

        public static Image GetCurrentBanner()
        {
            if (banner == null)
            {
                System.Reflection.Assembly target = System.Reflection.Assembly.GetExecutingAssembly();
                banner = new Bitmap(target.GetManifestResourceStream("fireBwall.Modules.bwall-header-v2.png"));
            }            
            return banner;
        }

        public static void SetColorScheme(Control control)
        {
            if (control is Button)
            {
                if (((Button)control).FlatStyle == FlatStyle.Flat)
                {
                    control.BackColor = System.Drawing.Color.Transparent;
                    control.ForeColor = System.Drawing.Color.White;
                }
                else
                {
                    control.BackColor = System.Drawing.Color.WhiteSmoke;
                    control.ForeColor = System.Drawing.Color.DarkBlue;
                }
            }
            else if (control is DataGridView)
            {
                ((DataGridView)control).GridColor = System.Drawing.Color.WhiteSmoke;
                ((DataGridView)control).ForeColor = System.Drawing.Color.DarkBlue;
                ((DataGridView)control).BackgroundColor = Color.WhiteSmoke;
                ((DataGridView)control).ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle() { ForeColor = Color.DarkBlue, BackColor = Color.WhiteSmoke, SelectionForeColor = Color.DarkBlue, SelectionBackColor = Color.WhiteSmoke };
                ((DataGridView)control).DefaultCellStyle = new DataGridViewCellStyle() { ForeColor = Color.DarkBlue, BackColor = Color.WhiteSmoke, SelectionBackColor = Color.LightBlue, SelectionForeColor = Color.DarkBlue };
            }
            else
            {
                control.BackColor = Color.WhiteSmoke;
                control.ForeColor = Color.DarkBlue;
            }
            foreach (Control c in control.Controls)
            {
                if (!(c is fireBwall.UI.DynamicUserControl))
                    SetColorScheme(c);
            }
        }
        #endregion
    }
}
