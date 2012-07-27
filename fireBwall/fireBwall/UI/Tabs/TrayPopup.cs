using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Threading;
using System.Text;
using System.Windows.Forms;
using fireBwall.Logging;

namespace fireBwall.UI.Tabs
{
    public partial class TrayPopup : DynamicForm
    {
        Thread t;

        public TrayPopup()
        {
            InitializeComponent();
            t = new Thread(Loop);
            t.Name = "Tray Hide Timer Loop";
            t.Start();
        }

        void Loop()
        {
            while (true)
            {
                if (DateTime.Now > hideTime)
                    HideMe();
                Thread.Sleep(1000);
            }
        }

        void ShowMe()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ThreadStart(HideMe));
            }
            else
            {
                this.Visible = true;
            }
        }

        void HideMe()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ThreadStart(HideMe));
            }
            else
            {
                this.Visible = false;
            }
        }

        DateTime hideTime = DateTime.Now;

        delegate void ALE(LogEvent le);

        public void AddLogEvent(LogEvent le)
        {
            if (listBox1.InvokeRequired)
            {
                ALE ale = new ALE(AddLogEvent);
                listBox1.Invoke(ale, le);
            }
            else
            {
                listBox1.Items.Insert(0, le);
                while (listBox1.Items.Count > 5)
                {
                    listBox1.Items.RemoveAt(5);
                }
                hideTime = DateTime.Now.AddSeconds(10);
                ShowMe();
            }
        }

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                LogEvent le = (LogEvent)listBox1.SelectedItem;
                if (le.Module != null)
                {
                    if (le.Module.GetUserInterface() != null)
                    {
                        try
                        {
                            DynamicUserControl uc = le.Module.GetUserInterface();
                            if (uc != null)
                            {
                                DynamicForm f = new DynamicForm();
                                f.Size = new System.Drawing.Size(640, 480);
                                f.Text = le.Module.Adapter.GetAdapterInformation().Name + " - " + le.Module.MetaData.GetMeta().Name + " " + le.Module.MetaData.GetMeta().Version;
                                f.Controls.Add(uc);
                                f.Show();
                                f.ThemeChanged();
                            }
                        }
                        catch (Exception ne)
                        {
                            LogCenter.Instance.LogException(ne);
                        }
                    }
                }
            }
        }

        private void TrayPopup_FormClosing(object sender, FormClosingEventArgs e)
        {
            t.Abort();
        }
    }
}
