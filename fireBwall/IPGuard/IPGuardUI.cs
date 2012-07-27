using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

using fireBwall.UI;
using fireBwall.Logging;
using fireBwall.Configuration;

/// This is the frontend to IPGuard.
namespace PassThru
{
    public partial class IPGuardUI : DynamicUserControl
    {
        // local ipguard object
        private IPGuard g;
        
        // initialize the local ipguard object and load any new 
        // lists from the module list dir
        public IPGuardUI(IPGuard g)
        {
            this.g = g;
            loadLists();
            InitializeComponent();
        }

        /// <summary>
        /// Load settings back into the GUI when redrawn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void IPGuard_Load(object sender, EventArgs e)
        {
            try
            {
                loadLists();
                available();
                loaded();
                this.logBox.Checked = this.g.data.logBlocked;
                this.incomingSelection.Checked = this.g.data.blockIncoming;
            }
            catch (Exception ex)
            {
                LogCenter.Instance.LogException(ex);
            }
        }

        /// <summary>
        /// Update the GUI with all the available lists
        /// </summary>
        private void available()
        {
            foreach (string s in this.g.Available_Lists)
            {
                availableBox.Items.Add(s);
            }
        }

        /// <summary>
        /// Update the GUI with all loaded lists
        /// </summary>
        private void loaded()
        {
            foreach (string s in this.g.data.Loaded_Lists)
            {
                loadedBox.Items.Add(s);
            }
        }

        /// <summary>
        /// Handles the add list button action.  First we add the item to the loaded
        /// box, then dump it off the available list, then go and build the ranges of that
        /// item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addList_Click(object sender, EventArgs e)
        {
            try
            {
                String item = (String)availableBox.SelectedItem;

                // add the item to the loaded box
                loadedBox.Items.Add(item);

                // remove from availableBox 
                availableBox.Items.Remove(item);

                // update serialized data
                this.g.Available_Lists.Remove(item);
                this.g.data.Loaded_Lists.Add(item);
                
                // go and build stuff
                g.buildRanges(item);
            }
            catch(Exception ex) 
            {
                LogCenter.Instance.LogException(ex);
            }
        }

        /// <summary>
        /// Handles the remove list button.  We first add it to the available box,
        /// then dump it off the loaded box, then go and rebuild the blocked ranges.  More
        /// info over at rebuild() on that.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeButton_Click(object sender, EventArgs e)
        {
            try
            {
                String item = (String)loadedBox.SelectedItem;

                // add the item to the availableBox
                availableBox.Items.Add(item);

                // remove from loaded box
                loadedBox.Items.Remove(item);

                // update serialized data
                this.g.Available_Lists.Add(item);
                this.g.data.Loaded_Lists.Remove(item);

                // go and rebuild all the ranges
                g.rebuild();
            }
            catch (Exception ex)
            {
                LogCenter.Instance.LogException(ex);
            }
        } 

        /// <summary>
        /// Return whether we should be logging all blocked connections
        /// </summary>
        /// <returns></returns>
        public bool isLog()
        {
            return logBox.Checked;
        }

        /// <summary>
        /// Return whether we should block incoming packets at these IPs too
        /// </summary>
        /// <returns></returns>
        public bool isIncoming()
        {
            return incomingSelection.Checked;
        }
        
        /// <summary>
        /// Checks if a list is already loaded
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool isLoaded(String s)
        {
            return loadedBox.Items.Contains(s);
        }

        /// <summary>
        /// Load all the text files in /firebwall/modules/IPGuard
        /// </summary>
        public void loadLists()
        {
            // do all the pathing shit
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            folder = folder + Path.DirectorySeparatorChar + "firebwall";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            folder = folder + Path.DirectorySeparatorChar + "modules" + Path.DirectorySeparatorChar + "IPGuard";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string filepath = folder;

            // get all the txt files in here
            string[] files = Directory.GetFiles(filepath, "*.txt");

            // add them to the list of available lists and update the UI
            foreach (string s in files)
            {
                // if the list isn't available and isn't already loaded
                if (!(this.g.Available_Lists.Contains(s)) &&
                    !(this.g.data.Loaded_Lists.Contains(s)))
                {
                    this.g.Available_Lists.Add(s);
                }
            }

            // set to list for easy iterating
            List<string> tmp = new List<string>(files);

            // remove lists no longer there
            foreach (string s in this.g.Available_Lists.ToArray())
            {
                if (!tmp.Contains(s) || 
                    !File.Exists(s))
                {
                    this.g.Available_Lists.Remove(s);
                }
            }

            // check loaded lists, too
            foreach (string s in this.g.data.Loaded_Lists.ToArray())
            {
                if (!tmp.Contains(s) ||
                    !File.Exists(s))
                {
                    this.g.data.Loaded_Lists.Remove(s);
                }
            }
        }

        /// <summary>
        /// Set the value of the incoming log
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void incomingSelection_CheckedChanged(object sender, EventArgs e)
        {
            this.g.data.blockIncoming = incomingSelection.Checked;
        }

        /// <summary>
        /// Set the value of the blocked logs box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logBox_CheckedChanged(object sender, EventArgs e)
        {
            this.g.data.logBlocked = logBox.Checked;
        }
    }
}
