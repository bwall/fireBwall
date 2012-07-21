using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using fireBwall.UI;
using fireBwall.Configuration;
using fireBwall.Logging;

namespace fireBwall.UI.Tabs
{
    public partial class MainWindow : DynamicForm
    {
        public MainWindow() : base()
        {                
			InitializeComponent();
            Program.mainWindow = this;
            multistring.SetString(Language.ENGLISH, "Log", "Log");
            multistring.SetString(Language.ENGLISH, "Options", "Options");
            multistring.SetString(Language.ENGLISH, "Adapters", "Adapters");
            multistring.SetString(Language.ENGLISH, "Help", "Help");

            multistring.SetString(Language.DUTCH, "Log", "Log");
            multistring.SetString(Language.DUTCH, "Options", "Opties");
            multistring.SetString(Language.DUTCH, "Adapters", "Adapters");
            multistring.SetString(Language.DUTCH, "Help", "Help");

            multistring.SetString(Language.HEBREW, "Log", "יומן רישום");
            multistring.SetString(Language.HEBREW, "Options", "אפשרויות");
            multistring.SetString(Language.HEBREW, "Adapters", "מתאמים");
            multistring.SetString(Language.HEBREW, "Help", "עזרה");

            multistring.SetString(Language.CHINESE, "Log", "登录");
            multistring.SetString(Language.CHINESE, "Options", "选项");
            multistring.SetString(Language.CHINESE, "Adapters", "适配器");
            multistring.SetString(Language.CHINESE, "Help", "帮助");

            multistring.SetString(Language.GERMAN, "Log", "Log");
            multistring.SetString(Language.GERMAN, "Options", "Optionen");
            multistring.SetString(Language.GERMAN, "Adapters", "Adapter");
            multistring.SetString(Language.GERMAN, "Help", "Hilfe");

            multistring.SetString(Language.RUSSIAN, "Log", "журнал");
            multistring.SetString(Language.RUSSIAN, "Options", "опции");
            multistring.SetString(Language.RUSSIAN, "Adapters", "Адаптеры");
            multistring.SetString(Language.RUSSIAN, "Help", "Помогите");

            multistring.SetString(Language.SPANISH, "Log", "Log");
            multistring.SetString(Language.SPANISH, "Options", "Opciones");
            multistring.SetString(Language.SPANISH, "Adapters", "Adaptadores");
            multistring.SetString(Language.SPANISH, "Help", "Ayuda");

            multistring.SetString(Language.PORTUGUESE, "Log", "Entrar");
            multistring.SetString(Language.PORTUGUESE, "Options", "Opções");
            multistring.SetString(Language.PORTUGUESE, "Adapters", "Adaptadores");
            multistring.SetString(Language.PORTUGUESE, "Help", "Ajudar");

            multistring.SetString(Language.JAPANESE, "Log", "ログ");
            multistring.SetString(Language.JAPANESE, "Options", "オプション");
            multistring.SetString(Language.JAPANESE, "Adapters", "アダプター");
            multistring.SetString(Language.JAPANESE, "Help", "ヘルプ");

            multistring.SetString(Language.ITALIAN, "Log", "Registro");
            multistring.SetString(Language.ITALIAN, "Options", "Opzioni");
            multistring.SetString(Language.ITALIAN, "Adapters", "Adattatori");
            multistring.SetString(Language.ITALIAN, "Help", "Guida");

            multistring.SetString(Language.FRENCH, "Log", "Journal");
            multistring.SetString(Language.FRENCH, "Options", "Options");
            multistring.SetString(Language.FRENCH, "Adapters", "Adaptateurs");
            multistring.SetString(Language.FRENCH, "Help", "Aide");
		}

		public void Exit() {
            try
            {
                ac.Kill();
                Application.Exit();
            }
            catch { }
		}
			
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e) 
        {
			if (e.CloseReason == CloseReason.UserClosing)
			{
				this.Visible = false;
				e.Cancel = true;
			}
		}

        LogDisplay log;
        AdapterControl ac;
        OptionsDisplay od;
        Help help;

		private void MainWindow_Load(object sender, EventArgs e) 
        {
            Program.trayIcon = new TrayIcon();
            Program.OnShutdown += Program.trayIcon.Dispose;                
            Program.OnShutdown += LogCenter.Instance.Kill;
            Program.updater = new Updates.UpdateChecker();
            Program.updater.Updater();
            Program.OnShutdown += Program.updater.Close;
            // call the log purger
            LogCenter.Instance.CleanLogs();

            log = new LogDisplay();
            log.Dock = DockStyle.Fill;

            splitContainer1.Panel2.Controls.Add(log);

            // load up the adapter control handler
            ac = new AdapterControl();
            ac.Dock = DockStyle.Fill;

            // load up the options tab handler
            od = new OptionsDisplay();
            od.Dock = DockStyle.Fill;

            // load up the options tab handler
            help = new Help();
            help.Dock = DockStyle.Fill;

            MainWindow_Resize(null, null);
            if (GeneralConfiguration.Instance.StartMinimized)
            {
                this.WindowState = FormWindowState.Minimized;
            }
            splitContainer1.Panel1.BackgroundImage = ThemeConfiguration.GetCurrentBanner();
            LanguageChanged();
            ThemeChanged();            
		}

        public override void ThemeChanged()
        {
            ThemeConfiguration.SetColorScheme(this);
            ThemeConfiguration.SetColorScheme(log);
            ThemeConfiguration.SetColorScheme(od);
            ThemeConfiguration.SetColorScheme(help);
            ThemeConfiguration.SetColorScheme(ac);
        }

        public override void LanguageChanged()
        {
            tabPage1.Text = multistring.GetString("Log");
            tabPage2.Text = multistring.GetString("Options");
            tabPage3.Text = multistring.GetString("Adapters");
            tabPage4.Text = multistring.GetString("Help");
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                this.Visible = false;
            }
            else
            {
                this.Visible = true;
                tabPage1.Location = new Point(1 * splitContainer1.Panel1.Width / 20 - tabPage1.Width / 2, (splitContainer1.Panel1.Height / 2) - (tabPage1.Height / 2) - 4);
                tabPage2.Location = new Point(8 * splitContainer1.Panel1.Width / 20 - tabPage2.Width / 2, (splitContainer1.Panel1.Height / 2) - (tabPage2.Height / 2) - 4);
                tabPage3.Location = new Point(4 * splitContainer1.Panel1.Width / 20 - tabPage3.Width / 2, (splitContainer1.Panel1.Height / 2) - (tabPage3.Height / 2) - 4);
                tabPage4.Location = new Point(12 * splitContainer1.Panel1.Width / 20 - tabPage4.Width / 2, (splitContainer1.Panel1.Height / 2) - (tabPage4.Height / 2) - 4);
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2.Controls.Clear();
            splitContainer1.Panel2.Controls.Add(log);
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2.Controls.Clear();
            splitContainer1.Panel2.Controls.Add(od);
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2.Controls.Clear();
            splitContainer1.Panel2.Controls.Add(ac);
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2.Controls.Clear();
            splitContainer1.Panel2.Controls.Add(help);
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            MainWindow_Resize(null, null);
        }
	}
    
}
