using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;

using fireBwall.UI;
using fireBwall.Modules;
using fireBwall.Logging;

namespace DDoS
{
    /// <summary>
    /// DDoS display module
    /// </summary>
    public partial class DDoSDisplay : DynamicUserControl
    {
        private DDoSModule dosmod;
        private List<BlockedIP> blockcache = new List<BlockedIP>();

        // constructor sets the local DDoSModule object
        public DDoSDisplay(DDoSModule dosmod)
        {
            this.dosmod = dosmod;
            InitializeComponent();
        }

        // update the modules' blocked IP cache
        private void UpdateBlockedCache()
        {
            this.dosmod.data.BlockCache = blockcache;
        }

        /// <summary>
        /// Loads data grid settings back up into the table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DDoSDisplay_Load(object sender, EventArgs e)
        {
            // load the blockcache and the threshhold
            blockcache = new List<BlockedIP>(this.dosmod.data.BlockCache);
            thresholdBox.Text = Convert.ToString(this.dosmod.data.dos_threshold);

            removeButton.Text = "Remove IP";
            addButton.Text = "Add IP";
            dosBlockTable.Columns[0].HeaderText = "IP Address";
            dosBlockTable.Columns[1].HeaderText = "Reason";
            dosBlockTable.Columns[2].HeaderText = "Date Blocked";

            RebuildTable();
        }
        
        /// <summary>
        /// Handles the IP add button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addButton_Click(object sender, EventArgs e)
        {
            // if the given string is a valid IPv4 addr.
            // IPAddress.TryParse is broken.
            if (regIP.IsMatch(addField.Text))
            {
                IPAddress t = IPAddress.Parse(addField.Text);
                blockcache.Add(new BlockedIP(t, DateTime.UtcNow, "User added"));
                
                // update the module blockcache and update the table
                UpdateBlockedCache();
                RebuildTable();

                // consume input
                addField.Text = "";
            }
        }

        /// <summary>
        /// Rebuilds the table from what's in blockcache
        /// </summary>
        private void RebuildTable()
        {
            dosBlockTable.Rows.Clear();
            foreach (BlockedIP ip in blockcache)
            {
                object[] t = { ip.Blockedip, ip.Reason, ip.DateBlocked };
                dosBlockTable.Rows.Add(t);
            }
        }
       

        /// <summary>
        /// Handles the remove button action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeButton_Click(object sender, EventArgs e)
        {
            // if nothing's been selected, get out
            if (dosBlockTable.SelectedRows.Count <= 0)
                return;                

            // grab the rowidx/type from the table
            int rowIdx = dosBlockTable.SelectedCells[0].RowIndex;
            string remIP = dosBlockTable["blockedip", rowIdx].Value.ToString();
            BlockedIP remove = new BlockedIP();

            // find the IP in the blockcache
            foreach ( BlockedIP ip in blockcache )
            {
                // if the two match
                if (((ip.Blockedip).ToString()).Equals(remIP))
                {
                    remove = ip;
                    break;
                }
            }

            // remove from the cache, update the module blockcache, and rebuild grid
            blockcache.Remove(remove);
            this.dosmod.data.BlockCache = this.blockcache;
            RebuildTable();
        }
        
        /// <summary>
        /// When the user changes the threshhold box, update the dosmod data object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void threshholdBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (thresholdBox.Text.Length > 0)
                {
                    dosmod.data.dos_threshold = Convert.ToInt32(thresholdBox.Text);
                }
            }
            catch (Exception ex)
            {
                LogCenter.Instance.LogException(ex);
            }
        }
     
        // regex pattern for matching a valid IP address
        // http://www.codekeep.net/snippets/9bd4694c-1d33-415e-b97f-db2f7f07015e.aspx
        private static Regex regIP = new Regex(
            @"(?<First>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Second>2[0-4]\d|25"
            + @"[0-5]|[01]?\d\d?)\.(?<Third>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?"
            + @"<Fourth>2[0-4]\d|25[0-5]|[01]?\d\d?)",
            RegexOptions.IgnoreCase
            | RegexOptions.CultureInvariant
            | RegexOptions.IgnorePatternWhitespace
            | RegexOptions.Compiled
            );
    }
}
