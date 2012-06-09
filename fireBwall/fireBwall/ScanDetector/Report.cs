using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ScanDetector
{
    public partial class Report : Form
    {
        public Report()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Generate and display the report for the given ip object
        /// </summary>
        /// <param name="obj"></param>
        public void GenerateReport(IPObj obj)
        {
            // set the title for the form
            this.Text = "Report for " + obj.Address.ToString();

            // set the fields
            this.addressField.Text = obj.Address.ToString();
            this.accessField.Text = obj.last_access.ToString();
            this.averageField.Text = obj.getAverage().ToString();
            this.portsField.Text = obj.getTouchedPorts().Count.ToString();
            this.portBox.MultiColumn = true;

            // sort ports
            List<int> ports = obj.getTouchedPorts();
            ports.Sort();
            foreach (int p in ports)
            {
                portBox.Items.Add(p);
            }

            // disable icon
            this.ShowIcon = false;
            this.Show();
        }
    }
}
