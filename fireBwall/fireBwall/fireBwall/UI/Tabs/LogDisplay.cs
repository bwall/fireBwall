using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using fireBwall.Logging;

namespace fireBwall.UI.Tabs
{
    public partial class LogDisplay : DynamicUserControl
    {
        public LogDisplay() : base()
        {
            InitializeComponent();
            LogCenter.Instance.PushLogEvent += AddLogEvent;
        }

        List<string> lines = new List<string>();

        /*
         * Object handles logging of a log event to the window
         * @param le is the log event object to be logged
         */
        public void AddLogEvent(LogEvent le)
        {
            // if the log event is sent from a thread other than the UI thread, invoke it
            if (listBox1.InvokeRequired)
            {
                LogCenter.NewLogEvent d = new LogCenter.NewLogEvent(AddLogEvent);
                listBox1.Invoke(d, new object[] { le });
            }
            // else log the message
            else
            {
                LogEvent e = (LogEvent)le;
                listBox1.Items.Insert(0, e.time.ToString() + " " + e.Module + ": " + e.Message);
                while (listBox1.Items.Count > 1000)
                {
                    listBox1.Items.RemoveAt(1000);
                }
            }
        }

        // copy the selected log to their clipboard for easy access
        private void listBox1_Click(object sender, EventArgs e)
        {
            try
            {
                if(listBox1.SelectedItem != null)
                    Clipboard.SetText(listBox1.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                LogCenter.Instance.LogException(ex);
            }
        }

        /// <summary>
        /// Set up a tooltip for the longer messages so they don't need to use the hscroll bar if they don't want to
        /// </summary>
        private int lastIdx = -1;
        private ToolTip tt = new ToolTip();
        private void listBox1_MouseMove(object sender, MouseEventArgs e)
        {
            int idx = listBox1.IndexFromPoint(e.Location);

            try
            {
                if (lastIdx != idx)
                {
                    lastIdx = idx;
                    if (lastIdx > -1)
                    {
                        tt.Active = false;
                        tt.SetToolTip(listBox1, listBox1.Items[lastIdx].ToString());
                        tt.Active = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogCenter.Instance.LogException(ex);
            }
        }
    }
}
