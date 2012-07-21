using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Net;
using fireBwall.Configuration;
using fireBwall.Updates;

namespace fireBwall.UI.Tabs
{
    public partial class DownloadCenter : DynamicForm
    {
        static DownloadCenter dc = null;
        fireBwallMetaData meta = null;
        static object padlock = new object();
        public static DownloadCenter Instance
        {
            get
            {
                lock (padlock)
                {
                    if (dc == null)
                        dc = new DownloadCenter();
                    return dc;
                }
            }
        }

        public void ShowFirebwallUpdate()
        {
            if (this.InvokeRequired)
            {
                System.Threading.ThreadStart ts = new System.Threading.ThreadStart(ShowFirebwallUpdate);
                this.Invoke(ts);
            }
            else
            {
                this.Visible = true;
                meta = UpdateChecker.availableFirebwall;
                this.Text = "New Version: fireBwall " + meta.version;
                textBox1.Text = "New Version: fireBwall " + meta.version + "\r\n" + meta.Description + "\r\n\r\nChange Log:\r\n";
                foreach (string s in meta.changelog)
                {
                    textBox1.Text += "\t- " + s + "\r\n";
                }
                LanguageChanged();
                ThemeChanged();                
            }
        }

        private DownloadCenter() : base()
        {
            InitializeComponent();
        }

        private void DownloadCenter_Load(object sender, EventArgs e)
        {
            ShowFirebwallUpdate();
        }

        private void DownloadCenter_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Visible = false;
                e.Cancel = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string folder = ConfigurationManagement.Instance.ConfigurationPath;
                folder = folder + Path.DirectorySeparatorChar + "installers";
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                string file = folder + Path.DirectorySeparatorChar + meta.filename;
                WebClient wc = new WebClient();
                wc.DownloadFile(meta.downloadUrl, file);
                System.Diagnostics.Process.Start(file);
                Program.Shutdown();
            }
            catch
            {
                MessageBox.Show("An error occurred during downloading and installing the newest version of fireBwall.  Please visit https://firebwall.com");
            }
        }
    }
}
