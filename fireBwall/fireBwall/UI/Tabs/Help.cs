using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using fireBwall.Modules;
using fireBwall.Filters.NDIS;
using fireBwall.Configuration;

namespace fireBwall.UI.Tabs
{
    public partial class Help : DynamicUserControl
    {
        // list of modules associated with the first adapter.
        // They all have the same, anyway.
        private ModuleList list;

        // used for delayed drawing
        Object selectedItem = null;

        public Help() : base()
        {
            InitializeComponent();
        }

        // overloaded constructor for initializing the window to a specific module
        public Help(Object selectedItem)
        {
            InitializeComponent();
            this.selectedItem = selectedItem;
        }

        /// <summary>
        /// Set the labels as the user flips through them.  If the Help window is being
        /// called externally (i.e. from the module's frame), they can pass the module's object
        /// into a constructor and automatically set the selected module when it's drawn.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void modBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // if we need to initialize the window to a specific module
            if (selectedItem != null)
            {
                // set the idx to the selected item
                modBox.SelectedIndex = modBox.Items.IndexOf(this.selectedItem.ToString());
                // set it back to null
                selectedItem = null;
            }

            // valid idx...
            if (modBox.SelectedIndex >= 0)
            {
                NDISModule temp = list.GetModule(modBox.SelectedIndex);

                modData.Text = temp.MetaData.GetMeta().Name;
                modAuthorField.Text = temp.MetaData.GetMeta().Author;
                modContactField.Text = temp.MetaData.GetMeta().Contact;
                modVersionField.Text = temp.MetaData.GetMeta().Version;
                modDescriptionField.Text = temp.MetaData.GetMeta().Description;
                modHelpBox.Text = temp.MetaData.GetMeta().Help;
            }
        }

        /// <summary>
        /// Load the modules from the first adapter into the Help GUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Help_Load(object sender, EventArgs e)
        {
            try
            {
                // grab the first adapter
                INDISFilter first_adapter = ProcessingConfiguration.Instance.NDISFilterList.GetAllAdapters()[0];
                // get it's module list
                list = first_adapter.Modules;

                // add it to the box
                for (int i = 0; i < list.Count; ++i)
                {
                    modBox.Items.Insert(i, list.GetModule(i).MetaData.GetMeta().Name);
                }

                // if there's a set idx, set it
                if (selectedItem != null)
                    modBox_SelectedIndexChanged(this, null);
            }
            catch { }
        }
    }
}
