using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using fireBwall.UI;
using fireBwall.Configuration;
using System.Text;
using System.Windows.Forms;

namespace BasicFirewall
{
    public partial class AddEditRule : DynamicForm
    {
        private Rule newRule;
        public Rule NewRule
        {
            get { return newRule; }
            set { newRule = value; }
        }

        public AddEditRule():base()
        {
            //English
            multistring.SetString(Language.ENGLISH, "All TCP Rule", "All TCP Rule");
            multistring.SetString(Language.ENGLISH, "TCP IP and Port Rule", "TCP IP and Port Rule");
            multistring.SetString(Language.ENGLISH, "TCP Port Rule", "TCP Port Rule");
            multistring.SetString(Language.ENGLISH, "All UDP Rule", "All UDP Rule");
            multistring.SetString(Language.ENGLISH, "UDP Port Rule", "UDP Port Rule");
            multistring.SetString(Language.ENGLISH, "All Rule", "All Rule");
            multistring.SetString(Language.ENGLISH, "IP Rule", "IP Rule");

            //Portuguese
            multistring.SetString(Language.PORTUGUESE, "All TCP Rule", "Todos Regra TCP");
            multistring.SetString(Language.PORTUGUESE, "TCP IP and Port Rule", "TCP IP e regra de porta");
            multistring.SetString(Language.PORTUGUESE, "TCP Port Rule", "Porta TCP Regra");
            multistring.SetString(Language.PORTUGUESE, "All UDP Rule", "Todos Regra UDP");
            multistring.SetString(Language.PORTUGUESE, "UDP Port Rule", "Porta UDP Regra");
            multistring.SetString(Language.PORTUGUESE, "All Rule", "Todos Regra");
            multistring.SetString(Language.PORTUGUESE, "IP Rule", "Regra IP");

            //Chinese
            multistring.SetString(Language.CHINESE, "All TCP Rule", "所有的TCP規則");
            multistring.SetString(Language.CHINESE, "TCP IP and Port Rule", "TCP IP和端口規則");
            multistring.SetString(Language.CHINESE, "TCP Port Rule", "TCP端口規則");
            multistring.SetString(Language.CHINESE, "All UDP Rule", "所有的UDP規則");
            multistring.SetString(Language.CHINESE, "UDP Port Rule", "UDP端口規則");
            multistring.SetString(Language.CHINESE, "All Rule", "所有規則");
            multistring.SetString(Language.CHINESE, "IP Rule", "IP规则");

            //German
            multistring.SetString(Language.GERMAN, "All TCP Rule", "Alle TCP Rule");
            multistring.SetString(Language.GERMAN, "TCP IP and Port Rule", "TCP IP und Port Regel");
            multistring.SetString(Language.GERMAN, "TCP Port Rule", "TCP Port Rule");
            multistring.SetString(Language.GERMAN, "All UDP Rule", "Alle UDP Regel");
            multistring.SetString(Language.GERMAN, "UDP Port Rule", "UDP Port Rule");
            multistring.SetString(Language.GERMAN, "All Rule", "Alle Rule");
            multistring.SetString(Language.GERMAN, "IP Rule", "IP Rule");

            //Russian
            Language lang = Language.RUSSIAN;
            multistring.SetString(lang, "All TCP Rule", "Все правила TCP");
            multistring.SetString(lang, "TCP IP and Port Rule", "TCP-IP и порт Правило");
            multistring.SetString(lang, "TCP Port Rule", "TCP-порт Правило");
            multistring.SetString(lang, "All UDP Rule", "Все правила UDP");
            multistring.SetString(lang, "UDP Port Rule", "UDP-порта Правило");
            multistring.SetString(lang, "All Rule", "Все правила");
            multistring.SetString(lang, "IP Rule", "IP правила");

            //Spanish
            lang = Language.SPANISH;
            multistring.SetString(lang, "All TCP Rule", "Todos los Regla TCP");
            multistring.SetString(lang, "TCP IP and Port Rule", "TCP IP y puerto de Regla");
            multistring.SetString(lang, "TCP Port Rule", "Puerto TCP Regla");
            multistring.SetString(lang, "All UDP Rule", "Todos los Regla UDP");
            multistring.SetString(lang, "UDP Port Rule", "El puerto UDP Regla");
            multistring.SetString(lang, "All Rule", "todos los Regla");
            multistring.SetString(lang, "IP Rule", "Regla IP");

            //Dutch
            lang = Language.DUTCH;
            multistring.SetString(lang, "All TCP Rule", "Alle TCP regel");
            multistring.SetString(lang, "TCP IP and Port Rule", "TCP IP en poortregel");
            multistring.SetString(lang, "TCP Port Rule", "TCP-poort regel");
            multistring.SetString(lang, "All UDP Rule", "Alle UDP-regel");
            multistring.SetString(lang, "UDP Port Rule", "UDP-poort regel");
            multistring.SetString(lang, "All Rule", "Alle regel");
            multistring.SetString(lang, "IP Rule", "IP-regel");

            //French
            lang = Language.FRENCH;
            multistring.SetString(lang, "All TCP Rule", "Toutes les règle TCP");
            multistring.SetString(lang, "TCP IP and Port Rule", "TCP IP et la règle de Port");
            multistring.SetString(lang, "TCP Port Rule", "Règle de Port TCP");
            multistring.SetString(lang, "All UDP Rule", "Toutes les règle UDP");
            multistring.SetString(lang, "UDP Port Rule", "Règle de Port UDP");
            multistring.SetString(lang, "All Rule", "Règle tous les");
            multistring.SetString(lang, "IP Rule", "Règle de la IP");

            //Hebrew
            lang = Language.HEBREW;
            multistring.SetString(lang, "All TCP Rule", "כל כלל TCP");
            multistring.SetString(lang, "TCP IP and Port Rule", "TCP IP וכלל יציאה");
            multistring.SetString(lang, "TCP Port Rule", "כלל יציאה TCP");
            multistring.SetString(lang, "All UDP Rule", "כל כלל UDP");
            multistring.SetString(lang, "UDP Port Rule", "יציאת ה-UDP כלל");
            multistring.SetString(lang, "All Rule", "כל כלל");
            multistring.SetString(lang, "IP Rule", "כלל ה-IP");

            //Italian
            lang = Language.ITALIAN;
            multistring.SetString(lang, "All TCP Rule", "Ogni regola TCP");
            multistring.SetString(lang, "TCP IP and Port Rule", "TCP IP e regola di porta");
            multistring.SetString(lang, "TCP Port Rule", "Regola di porta TCP");
            multistring.SetString(lang, "All UDP Rule", "Ogni regola UDP");
            multistring.SetString(lang, "UDP Port Rule", "Regola di porta UDP");
            multistring.SetString(lang, "All Rule", "Regola tutte le");
            multistring.SetString(lang, "IP Rule", "IP Rule");

            //Japanese
            lang = Language.JAPANESE;
            multistring.SetString(lang, "All TCP Rule", "すべての TCP ルール");
            multistring.SetString(lang, "TCP IP and Port Rule", "TCP IP とポートの規則");
            multistring.SetString(lang, "TCP Port Rule", "TCP ポートの規則");
            multistring.SetString(lang, "All UDP Rule", "すべての UDP ルール");
            multistring.SetString(lang, "UDP Port Rule", "UDP ポート ルール");
            multistring.SetString(lang, "All Rule", "すべてのルール");
            multistring.SetString(lang, "IP Rule", "IP ルール");
            InitializeComponent();
        }

        /// <summary>
        /// Constructor for initializing the GUi when editing a rule
        /// </summary>
        /// <param name="tmp"></param>
        public AddEditRule(Rule tmp)
        {
            //English
            multistring.SetString(Language.ENGLISH, "All TCP Rule", "All TCP Rule");
            multistring.SetString(Language.ENGLISH, "TCP IP and Port Rule", "TCP IP and Port Rule");
            multistring.SetString(Language.ENGLISH, "TCP Port Rule", "TCP Port Rule");
            multistring.SetString(Language.ENGLISH, "All UDP Rule", "All UDP Rule");
            multistring.SetString(Language.ENGLISH, "UDP Port Rule", "UDP Port Rule");
            multistring.SetString(Language.ENGLISH, "All Rule", "All Rule");
            multistring.SetString(Language.ENGLISH, "IP Rule", "IP Rule");

            //Portuguese
            multistring.SetString(Language.PORTUGUESE, "All TCP Rule", "Todos Regra TCP");
            multistring.SetString(Language.PORTUGUESE, "TCP IP and Port Rule", "TCP IP e regra de porta");
            multistring.SetString(Language.PORTUGUESE, "TCP Port Rule", "Porta TCP Regra");
            multistring.SetString(Language.PORTUGUESE, "All UDP Rule", "Todos Regra UDP");
            multistring.SetString(Language.PORTUGUESE, "UDP Port Rule", "Porta UDP Regra");
            multistring.SetString(Language.PORTUGUESE, "All Rule", "Todos Regra");
            multistring.SetString(Language.PORTUGUESE, "IP Rule", "Regra IP");

            //Chinese
            multistring.SetString(Language.CHINESE, "All TCP Rule", "所有的TCP規則");
            multistring.SetString(Language.CHINESE, "TCP IP and Port Rule", "TCP IP和端口規則");
            multistring.SetString(Language.CHINESE, "TCP Port Rule", "TCP端口規則");
            multistring.SetString(Language.CHINESE, "All UDP Rule", "所有的UDP規則");
            multistring.SetString(Language.CHINESE, "UDP Port Rule", "UDP端口規則");
            multistring.SetString(Language.CHINESE, "All Rule", "所有規則");
            multistring.SetString(Language.CHINESE, "IP Rule", "IP规则");

            //German
            multistring.SetString(Language.GERMAN, "All TCP Rule", "Alle TCP Rule");
            multistring.SetString(Language.GERMAN, "TCP IP and Port Rule", "TCP IP und Port Regel");
            multistring.SetString(Language.GERMAN, "TCP Port Rule", "TCP Port Rule");
            multistring.SetString(Language.GERMAN, "All UDP Rule", "Alle UDP Regel");
            multistring.SetString(Language.GERMAN, "UDP Port Rule", "UDP Port Rule");
            multistring.SetString(Language.GERMAN, "All Rule", "Alle Rule");
            multistring.SetString(Language.GERMAN, "IP Rule", "IP Rule");

            //Russian
            Language lang = Language.RUSSIAN;
            multistring.SetString(lang, "All TCP Rule", "Все правила TCP");
            multistring.SetString(lang, "TCP IP and Port Rule", "TCP-IP и порт Правило");
            multistring.SetString(lang, "TCP Port Rule", "TCP-порт Правило");
            multistring.SetString(lang, "All UDP Rule", "Все правила UDP");
            multistring.SetString(lang, "UDP Port Rule", "UDP-порта Правило");
            multistring.SetString(lang, "All Rule", "Все правила");
            multistring.SetString(lang, "IP Rule", "IP правила");

            //Spanish
            lang = Language.SPANISH;
            multistring.SetString(lang, "All TCP Rule", "Todos los Regla TCP");
            multistring.SetString(lang, "TCP IP and Port Rule", "TCP IP y puerto de Regla");
            multistring.SetString(lang, "TCP Port Rule", "Puerto TCP Regla");
            multistring.SetString(lang, "All UDP Rule", "Todos los Regla UDP");
            multistring.SetString(lang, "UDP Port Rule", "El puerto UDP Regla");
            multistring.SetString(lang, "All Rule", "todos los Regla");
            multistring.SetString(lang, "IP Rule", "Regla IP");

            //Dutch
            lang = Language.DUTCH;
            multistring.SetString(lang, "All TCP Rule", "Alle TCP regel");
            multistring.SetString(lang, "TCP IP and Port Rule", "TCP IP en poortregel");
            multistring.SetString(lang, "TCP Port Rule", "TCP-poort regel");
            multistring.SetString(lang, "All UDP Rule", "Alle UDP-regel");
            multistring.SetString(lang, "UDP Port Rule", "UDP-poort regel");
            multistring.SetString(lang, "All Rule", "Alle regel");
            multistring.SetString(lang, "IP Rule", "IP-regel");

            //French
            lang = Language.FRENCH;
            multistring.SetString(lang, "All TCP Rule", "Toutes les règle TCP");
            multistring.SetString(lang, "TCP IP and Port Rule", "TCP IP et la règle de Port");
            multistring.SetString(lang, "TCP Port Rule", "Règle de Port TCP");
            multistring.SetString(lang, "All UDP Rule", "Toutes les règle UDP");
            multistring.SetString(lang, "UDP Port Rule", "Règle de Port UDP");
            multistring.SetString(lang, "All Rule", "Règle tous les");
            multistring.SetString(lang, "IP Rule", "Règle de la IP");

            //Hebrew
            lang = Language.HEBREW;
            multistring.SetString(lang, "All TCP Rule", "כל כלל TCP");
            multistring.SetString(lang, "TCP IP and Port Rule", "TCP IP וכלל יציאה");
            multistring.SetString(lang, "TCP Port Rule", "כלל יציאה TCP");
            multistring.SetString(lang, "All UDP Rule", "כל כלל UDP");
            multistring.SetString(lang, "UDP Port Rule", "יציאת ה-UDP כלל");
            multistring.SetString(lang, "All Rule", "כל כלל");
            multistring.SetString(lang, "IP Rule", "כלל ה-IP");

            //Italian
            lang = Language.ITALIAN;
            multistring.SetString(lang, "All TCP Rule", "Ogni regola TCP");
            multistring.SetString(lang, "TCP IP and Port Rule", "TCP IP e regola di porta");
            multistring.SetString(lang, "TCP Port Rule", "Regola di porta TCP");
            multistring.SetString(lang, "All UDP Rule", "Ogni regola UDP");
            multistring.SetString(lang, "UDP Port Rule", "Regola di porta UDP");
            multistring.SetString(lang, "All Rule", "Regola tutte le");
            multistring.SetString(lang, "IP Rule", "IP Rule");

            //Japanese
            lang = Language.JAPANESE;
            multistring.SetString(lang, "All TCP Rule", "すべての TCP ルール");
            multistring.SetString(lang, "TCP IP and Port Rule", "TCP IP とポートの規則");
            multistring.SetString(lang, "TCP Port Rule", "TCP ポートの規則");
            multistring.SetString(lang, "All UDP Rule", "すべての UDP ルール");
            multistring.SetString(lang, "UDP Port Rule", "UDP ポート ルール");
            multistring.SetString(lang, "All Rule", "すべてのルール");
            multistring.SetString(lang, "IP Rule", "IP ルール");
            InitializeComponent();

            // force the combo box to load the rules
            AddEditRule_Load(this, null);

            // disallow changing the base type of the rule
            comboBox1.Enabled = false;

            // set the rule type and GUI options
            if (tmp is AllRule)
            {
                AllRule t = (AllRule)tmp;
                checkBoxLog.Checked = t.log;

                // set the rule type
                comboBox1.SelectedIndex = 5;

                // set the direction
                checkBoxIn.Checked = ((t.direction & Direction.IN) != 0) ? true : false;
                checkBoxOut.Checked = ((t.direction & Direction.OUT) != 0) ? true : false;

                // set the action box
                comboBoxAction.SelectedIndex = ((t.ps & PacketStatus.ALLOWED) != 0) ? 1 : 0;

                //notify
                notifyBox.Checked = (t.notify);
            }
            else if (tmp is IPRule)
            {
                IPRule t = (IPRule)tmp;
                checkBoxLog.Checked = t.log;

                //idx
                comboBox1.SelectedIndex = 6;

                //direction
                checkBoxIn.Checked = ((t.direction & Direction.IN) != 0) ? true : false;
                checkBoxOut.Checked = ((t.direction & Direction.OUT) != 0) ? true : false;

                //action
                comboBoxAction.SelectedIndex = ((t.ps & PacketStatus.ALLOWED) != 0) ? 1 : 0;

                //args
                textBoxArguments.Text = t.GetIPString();

                //notify
                notifyBox.Checked = (t.notify);
            }
            else if (tmp is TCPAllRule)
            {
                TCPAllRule t = (TCPAllRule)tmp;
                checkBoxLog.Checked = t.log;

                //idx
                comboBox1.SelectedIndex = 0;

                //dir
                checkBoxIn.Checked = ((t.direction & Direction.IN) != 0) ? true : false;
                checkBoxOut.Checked = ((t.direction & Direction.OUT) != 0) ? true : false;

                //action
                comboBoxAction.SelectedIndex = ((t.ps & PacketStatus.ALLOWED) != 0) ? 1 : 0;

                //notify
                notifyBox.Checked = (t.notify);
            }
            else if (tmp is TCPIPPortRule)
            {
                TCPIPPortRule t = (TCPIPPortRule)tmp;
                checkBoxLog.Checked = t.log;

                //idx
                comboBox1.SelectedIndex = 1;

                //dir
                checkBoxIn.Checked = ((t.direction & Direction.IN) != 0) ? true : false;
                checkBoxOut.Checked = ((t.direction & Direction.OUT) != 0) ? true : false;

                //action
                comboBoxAction.SelectedIndex = ((t.ps & PacketStatus.ALLOWED) != 0) ? 1 : 0;

                //args
                textBoxArguments.Text = String.Format("{0} {1}", t.ip, t.port);

                //notify
                notifyBox.Checked = (t.notify);
            }
            else if (tmp is TCPPortRule)
            {
                TCPPortRule t = (TCPPortRule)tmp;
                checkBoxLog.Checked = t.log;

                //idx
                comboBox1.SelectedIndex = 2;

                //dir
                checkBoxIn.Checked = ((t.direction & Direction.IN) != 0) ? true : false;
                checkBoxOut.Checked = ((t.direction & Direction.OUT) != 0) ? true : false;

                //action
                comboBoxAction.SelectedIndex = ((t.ps & PacketStatus.ALLOWED) != 0) ? 1 : 0;

                //args
                textBoxArguments.Text = t.GetPortString();

                //notify
                notifyBox.Checked = (t.notify);
            }
            else if (tmp is UDPAllRule)
            {
                UDPAllRule t = (UDPAllRule)tmp;
                checkBoxLog.Checked = t.log;

                //idx
                comboBox1.SelectedIndex = 3;

                //dir
                checkBoxIn.Checked = ((t.direction & Direction.IN) != 0) ? true : false;
                checkBoxOut.Checked = ((t.direction & Direction.OUT) != 0) ? true : false;

                //action
                comboBoxAction.SelectedIndex = ((t.ps & PacketStatus.ALLOWED) != 0) ? 1 : 0;

                //notify
                notifyBox.Checked = (t.notify);
            }
            else if (tmp is UDPPortRule)
            {
                UDPPortRule t = (UDPPortRule)tmp;
                checkBoxLog.Checked = t.log;

                //idx
                comboBox1.SelectedIndex = 4;

                //dir
                checkBoxIn.Checked = ((t.direction & Direction.IN) != 0) ? true : false;
                checkBoxOut.Checked = ((t.direction & Direction.OUT) != 0) ? true : false;

                //action
                comboBoxAction.SelectedIndex = ((t.ps & PacketStatus.ALLOWED) != 0) ? 1 : 0;

                //args
                textBoxArguments.Text = t.GetPortString();

                //notify
                notifyBox.Checked = (t.notify);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tmp = "";
            bool enableArgs = true;

            switch (comboBox1.SelectedIndex)
            {
                case 0:
                case 3:
                case 5:
                    // most of these aren't going to translate very differently 
                    // into their foreign tongue, so i'm leaving it as is
                    tmp = "No args";
                    enableArgs = false;
                    break;
                case 2:
                case 4:
                    tmp = "(Space Separated) Port";
                    break;
                case 1:
                    tmp = "(Space Separated) IP Port";
                    break;
                case 6:
                    tmp = "(Space Separated) IP";
                    break;
            }

            labelArgs.Text = tmp;
            textBoxArguments.Enabled = enableArgs;
        }

        /// <summary>
        /// Event handles the OK button to generate a new rule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBoxIn.Checked || checkBoxOut.Checked)
            {
                try
                {
                    RuleType rt = RuleType.ALL;
                    switch (comboBox1.SelectedIndex)
                    {
                        case 3:
                            rt = RuleType.UDPALL;
                            break;
                        case 4:
                            rt = RuleType.UDPPORT;
                            break;
                        case 1:
                            rt = RuleType.TCPIPPORT;
                            break;
                        case 2:
                            rt = RuleType.TCPPORT;
                            break;
                        case 0:
                            rt = RuleType.TCPALL;
                            break;
                        case 5:
                            rt = RuleType.ALL;
                            break;
                        case 6:
                            rt = RuleType.IP;
                            break;
                    }
                    Direction dir;
                    if (checkBoxIn.Checked && checkBoxOut.Checked)
                    {
                        dir = Direction.IN | Direction.OUT;
                    }
                    else if (checkBoxOut.Checked)
                    {
                        dir = Direction.OUT;
                    }
                    else
                    {
                        dir = Direction.IN;
                    }
                    PacketStatus ps;
                    if (comboBoxAction.Text == "Block")
                        ps = PacketStatus.BLOCKED;
                    else
                        ps = PacketStatus.ALLOWED;

                    // multiple ports and the ip:port rule are parsed later on, so send all args as a string

                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    newRule = BasicFirewallModule.RuleFactory.MakeRule(rt, ps, dir, textBoxArguments.Text,
                                                            checkBoxLog.Checked, notifyBox.Checked);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in creating rule.");
                    fireBwall.Logging.LogCenter.Instance.LogException(ex);
                }
            }
            else
            {
                MessageBox.Show("You need to select in or out first");
            }
        }

        /// <summary>
        /// Event handles the cancel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public override void LanguageChanged()
        {
            int selected = comboBox1.SelectedIndex;
            comboBox1.Items.Clear();
            comboBox1.Items.Add(multistring.GetString("All TCP Rule"));
            comboBox1.Items.Add(multistring.GetString("TCP IP and Port Rule"));
            comboBox1.Items.Add(multistring.GetString("TCP Port Rule"));
            comboBox1.Items.Add(multistring.GetString("All UDP Rule"));
            comboBox1.Items.Add(multistring.GetString("UDP Port Rule"));
            comboBox1.Items.Add(multistring.GetString("All Rule"));
            comboBox1.Items.Add(multistring.GetString("IP Rule"));
            comboBox1.SelectedIndex = selected;
        }

        private void AddEditRule_Load(object sender, EventArgs e)
        {
            LanguageChanged();
            ThemeChanged();
        }

        /// <summary>
        /// if checked, lock Log into a check state as well since we can't notify without
        /// logging enabled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyBox_CheckedChanged(object sender, EventArgs e)
        {
            if (notifyBox.Checked)
            {
                checkBoxLog.Checked = true;
                checkBoxLog.Enabled = false;
            }
            else
            {
                checkBoxLog.Checked = false;
                checkBoxLog.Enabled = true;
            }
        }
    }
}
