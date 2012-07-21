using System;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using fireBwall.Configuration;

namespace fireBwall.UI
{
    public class DynamicForm : Form
    {
        #region Icon

        static readonly object padlock = new object();
        static Icon icon = null;
        public static Icon GetIcon()
        {
            lock (padlock)
            {
                if (icon == null)
                {
                    Assembly target = System.Reflection.Assembly.GetExecutingAssembly();
                    icon = new Icon(target.GetManifestResourceStream("fireBwall.Modules.Resources.newTray.ico"));
                }
                return icon;
            }
        }

        #endregion

        public DynamicForm()
        {
            GeneralConfiguration.Instance.LanguageChanged += LanguageChanged;
            Icon = DynamicForm.GetIcon();
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
