using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using fireBwall.UI;
using fireBwall.Configuration;
using fireBwall.Logging;

namespace MacFilter
{
    public partial class MacFilterControl : DynamicUserControl
    {
        MacFilterModule mf;
        public MacFilterControl(MacFilterModule mf)
        {
            this.mf = mf;
            InitializeComponent();
        }

        private void MacFilterControl_Load(object sender, EventArgs e)
        {
            listBox1.DisplayMember = "String";
            List<MacFilterModule.MacRule> r = new List<MacFilterModule.MacRule>(mf.rules);
            listBox1.Items.Clear();
            foreach (MacFilterModule.MacRule rule in r)
            {
                listBox1.Items.Add(rule);
            }
            
            button1.Text = mf.multistring.GetString("Add Rule");
            button2.Text = mf.multistring.GetString("Remove Rule");
            buttonMoveDown.Text = mf.multistring.GetString("Move Down");
            buttonMoveUp.Text = mf.multistring.GetString("Move Up");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                AddEditMacRule aer = new AddEditMacRule();
                if (aer.ShowDialog() == DialogResult.OK)
                {
                    listBox1.Items.Add(aer.newRule);
                    List<MacFilterModule.MacRule> r = new List<MacFilterModule.MacRule>();
                    foreach (object rule in listBox1.Items)
                    {
                        r.Add((MacFilterModule.MacRule)rule);
                    }

                    mf.InstanceGetRuleUpdates(r);
                }
                aer.Dispose();
            }
            catch (Exception exception)
            {
                LogCenter.Instance.LogException(exception);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem == null) return;
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);

                List<MacFilterModule.MacRule> r = new List<MacFilterModule.MacRule>();
                foreach (object rule in listBox1.Items)
                {
                    r.Add((MacFilterModule.MacRule)rule);
                }

                mf.InstanceGetRuleUpdates(r);
            }
            catch (Exception ex)
            {
                LogCenter.Instance.LogException(ex);
            }
        }

        private void buttonMoveUp_Click(object sender, EventArgs e)
        {
            try
            {
                int index = listBox1.SelectedIndex;
                if (index != 0)
                {
                    MacFilterModule.MacRule rule = (MacFilterModule.MacRule)listBox1.Items[index];
                    listBox1.Items.RemoveAt(index);
                    index--;
                    listBox1.Items.Insert(index, rule);
                    listBox1.SelectedIndex = index;
                    List<MacFilterModule.MacRule> r = new List<MacFilterModule.MacRule>();
                    foreach (object ru in listBox1.Items)
                    {
                        r.Add((MacFilterModule.MacRule)ru);
                    }

                    mf.InstanceGetRuleUpdates(r);
                }
            }
            catch (Exception ex)
            {
                LogCenter.Instance.LogException(ex);
            }
        }

        private void buttonMoveDown_Click(object sender, EventArgs e)
        {
            try
            {
                int index = listBox1.SelectedIndex;
                if (index != listBox1.Items.Count - 1)
                {
                    MacFilterModule.MacRule rule = (MacFilterModule.MacRule)listBox1.Items[index];
                    listBox1.Items.RemoveAt(index);
                    index++;
                    listBox1.Items.Insert(index, rule);
                    listBox1.SelectedIndex = index;
                    List<MacFilterModule.MacRule> r = new List<MacFilterModule.MacRule>();
                    foreach (object ru in listBox1.Items)
                    {
                        r.Add((MacFilterModule.MacRule)ru);
                    }

                    mf.InstanceGetRuleUpdates(r);
                }
            }
            catch (Exception ex)
            {
                LogCenter.Instance.LogException(ex);
            }
        }

        /// <summary>
        /// Edits the selected rule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editButton_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null )
                return;

            try
            {
                // grab the current rule
                MacFilterModule.MacRule new_rule = (MacFilterModule.MacRule)listBox1.Items[listBox1.SelectedIndex];
                // grab its idx 
                int idx = listBox1.SelectedIndex;
                
                AddEditMacRule aer = new AddEditMacRule(new_rule);

                if (aer.ShowDialog() == DialogResult.OK)
                {
                    // replace rule
                    listBox1.Items[idx] = aer.newRule;
                    List<MacFilterModule.MacRule> r = new List<MacFilterModule.MacRule>();
                    foreach (object rule in listBox1.Items)
                    {
                        r.Add((MacFilterModule.MacRule)rule);
                    }

                    mf.InstanceGetRuleUpdates(r);
                    aer.Dispose();
                }
            }
            catch (Exception exception)
            {
                LogCenter.Instance.LogException(exception);
            }
        }
    }
}