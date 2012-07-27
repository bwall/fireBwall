using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using fireBwall.Configuration;

namespace fireBwall.UI
{
    public class DynamicUserControl : UserControl
    {
        public DynamicUserControl()
        {
            //ThemeConfiguration.Instance.ThemeChanged += new System.Threading.ThreadStart(ThemeChanged);
            GeneralConfiguration.Instance.LanguageChanged += LanguageChanged;
        }

        public virtual void ThemeChanged()
        {
            ThemeConfiguration.SetColorScheme(this);
        }

        public MultilingualStringManager multistring = new MultilingualStringManager();

        public virtual void LanguageChanged()
        {

        }
    }
}
