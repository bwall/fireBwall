using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using fireBwall.UI;
using fireBwall.Utils;

namespace ICMPFilter
{
    /// <summary>
    /// ICMP filter display class
    /// </summary>
    public partial class ICMPFilterDisplay : DynamicUserControl
    {
        // global filter obj
        private ICMPFilterModule filter;

        // table of user rule mappings
        // key: type
        // value: list<string> of codes
        private SerializableDictionary<string, List<string>> ruletable;
        private SerializableDictionary<string, List<string>> ruletablev6;

        // table of ICMP mappings
        // key: type
        // value: dictionary of codes and descriptions
        private Dictionary<string, Dictionary<string, string>> icmpv4List = new Dictionary<string, Dictionary<string, string>>();
        private Dictionary<string, Dictionary<string, string>> icmpv6List = new Dictionary<string, Dictionary<string, string>>();

        // constructor initializes the local ICMPFilter object and the UI
        public ICMPFilterDisplay(ICMPFilterModule filter)
        {
            this.filter = filter;
            ruletable = new SerializableDictionary<string, List<string>>(filter.data.RuleTable);
            ruletablev6 = new SerializableDictionary<string, List<string>>(filter.data.RuleTablev6);
            buildICMPListv4();
            buildICMPListv6();
            InitializeComponent();
        }

       /// <summary>
       /// Parse input and add to the blocked box
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            List<string> temp = new List<String>();
            string[] codes;
            string type;

            // remove spaces and split on comma
            codes = codeField.Text.Replace(" ", string.Empty).Split(',');
            type = typeField.Text;

            // parse for ipv4
            if (!ipv6Box.Checked)
            {
                // if the type exists
                if (icmpv4List.ContainsKey(type))
                {
                    // iterate through codes and add them to the table and hashtable
                    foreach (string s in codes)
                    {
                        // if the code exists
                        if (icmpv4List[type].ContainsKey(s))
                        {
                            // if that type already exists
                            if (ruletable.ContainsKey(type))
                            {
                                // get the list of the key
                                ruletable.TryGetValue(type, out temp);
                                // don't allow duplicate entries
                                if (temp.Contains(s))
                                    continue;
                                // add the code to the list
                                temp.Add(s);
                                // assignment of updated kv pair
                                ruletable[type] = temp;
                            }

                            // if the type doesn't exist yet, create it
                            else
                            {
                                temp.Add(s);
                                ruletable.Add(type, temp);
                            }

                            object[] row = { type, s, icmpv4List[type][s], "IPv4" };
                            tableDisplay.Rows.Add(row);
                        }
                    }
                }
            }
            // else its an IPv6 type/code
            else
            {
                // check the type
                if (icmpv6List.ContainsKey(type))
                {
                    // iterate through the codes and add them
                    foreach (string s in codes)
                    {
                        // if the code exists
                        if (icmpv6List[type].ContainsKey(s))
                        {
                            // if the type exists
                            if (ruletablev6.ContainsKey(type))
                            {
                                // get the list of the key
                                ruletablev6.TryGetValue(type, out temp);
                                // don't allow duplicate entries
                                if (temp.Contains(s))
                                    continue;
                                // add the code to the list
                                temp.Add(s);
                                // update assignment
                                ruletablev6[type] = temp;
                            }
                            // else the type doesn't exist yet
                            else
                            {
                                temp.Add(s);
                                ruletablev6.Add(type, temp);
                            }

                            object[] row = { type, s, icmpv6List[type][s], "IPv6" };
                            tableDisplay.Rows.Add(row);
                        }
                    }
                }
            }
            
            // consume input
            codeField.Text = "";
            typeField.Text = "";

            // update our rule table
            UpdateRuleTable();
        }

        /*
         * Method handles the delete button
         * 
         * @param sender
         * @param e
         */
        private void button1_Click_1(object sender, EventArgs e)
        {
            // if nothing's been selected, get out
            if (tableDisplay.SelectedRows.Count <= 0)
                return;

            // grab the type/code/rowIdx from the table
            int rowIdx = tableDisplay.SelectedCells[0].RowIndex;
            string type = tableDisplay["Type", rowIdx].Value.ToString();
            string code = tableDisplay["Code", rowIdx].Value.ToString();

            // if it's an IPv4 rule
            if (tableDisplay["Version", rowIdx].Value.ToString().Equals("IPv4"))
            {
                // if ruletable contains the key (it should)
                if (ruletable.ContainsKey(type))
                {
                    List<string> temp;
                    ruletable.TryGetValue(type, out temp);
                    // check if the code list of the key contains the code
                    if (temp.Contains(code))
                    {
                        // remove code from temp list
                        temp.Remove(code);
                        // assignment of updated kv pair
                        ruletable[type] = temp;
                        // remove from display table
                        tableDisplay.Rows.Remove(tableDisplay.Rows[rowIdx]);
                    }
                }
            }
            // else it's an ipv6 rule
            else
            {
                // if ruletable contains the key (it should)
                if (ruletablev6.ContainsKey(type))
                {
                    List<string> temp;
                    ruletablev6.TryGetValue(type, out temp);
                    // check if the code list of the key contains the code
                    if (temp.Contains(code))
                    {
                        // remove code from temp list
                        temp.Remove(code);
                        // assignment of updated kv pair
                        ruletablev6[type] = temp;
                        // remove from display table
                        tableDisplay.Rows.Remove(tableDisplay.Rows[rowIdx]);
                    }
                }
            }

            // update our rule table
            UpdateRuleTable();
        }

        /*
         * Loads settings back up into the table
         * 
         * @param sender
         * @param e
         */
        private void ICMPFilterDisplay_Load(object sender, EventArgs e)
        {
            // generate the v4/v6 ruletables
            ruletable = new SerializableDictionary<string, List<string>>(filter.data.RuleTable);
            ruletablev6 = new SerializableDictionary<string, List<string>>(filter.data.RuleTablev6);

            // rebuild the datagrid
            rebuildTable();

            // deny ipv4
            allBox.Checked = filter.data.DenyIPv4;

            // deny ipv6
            blockIPv6Box.Checked = filter.data.DenyIPv6;

            // deny ipv6 except ndp
            allButNDP.Checked = filter.data.DenyIPv6NDP;

            // log state
            logBox.Checked = filter.data.Log;
        }

        // pushes update to the ruletable object
        private void UpdateRuleTable()
        {
            this.filter.data.RuleTable = ruletable;
            this.filter.data.RuleTablev6 = ruletablev6;
        }

        /*
         * Method handles the All ICMP check box; 
         * Disables the type/code fields if selected, enables if not.
         * Updates the denyAll bool in the module.
         * Disables the add/delete button.
        */
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.filter.data.DenyIPv4 = allBox.Checked;

            // if the ipv6 or ipv6ndp box is checked, don't do anything
            if (blockIPv6Box.Checked || allButNDP.Checked)
                return;
            flip();
        }

        /// <summary>
        /// Handles when the block ipv6 box changes status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void blockIPv6Box_CheckedChanged(object sender, EventArgs e)
        {
            this.filter.data.DenyIPv6 = blockIPv6Box.Checked;

            // invert the all but ndp button
            allButNDP.Enabled = !(allButNDP.Enabled);

            // if the ipv4 or ipv6ndp box is already checked, don't do anything
            if (allBox.Checked || allButNDP.Checked)
                return;
            flip();
        }

        /// <summary>
        /// Handles the all ipv6 but ndp check
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void allButNDP_CheckedChanged(object sender, EventArgs e)
        {
            this.filter.data.DenyIPv6NDP = allButNDP.Checked;

            // invert the all but ndp button
            blockIPv6Box.Enabled = !(blockIPv6Box.Enabled);

            if (allBox.Checked || blockIPv6Box.Checked)
                return;
            flip();
        }

        /// <summary>
        /// flip the 
        /// </summary>
        private void flip()
        {
            typeField.Enabled = !(typeField.Enabled);
            codeField.Enabled = !(codeField.Enabled);
            addButton.Enabled = !(addButton.Enabled);
            deleteButton.Enabled = !(deleteButton.Enabled);
            viewICMP.Enabled = !(viewICMP.Enabled);
            ipv6Box.Enabled = !(ipv6Box.Enabled);
        }
        /*
         * Method handles the View ICMP button;
         * Displays all available ICMP types/codes
         */
        private void button1_Click_2(object sender, EventArgs e)
        {
            // if they're viewing the ruletable
            if (typeField.Enabled)
            {
                tableDisplay.Rows.Clear();

                if (!ipv6Box.Checked)
                {
                    // get all the keys
                    ICollection keys = icmpv4List.Keys;
                    foreach (string s in keys)
                    {
                        // get all the codes
                        Dictionary<string, string> temp = icmpv4List[s];
                        ICollection codes = temp.Keys;
                        foreach (string c in codes)
                        {
                            // add each to the table
                            object[] t = { s, c, icmpv4List[s][c], "IPv4" };
                            tableDisplay.Rows.Add(t);
                        }
                    }
                }
                else
                {
                    // get keys
                    ICollection keys = icmpv6List.Keys;
                    foreach (string s in keys)
                    {
                        Dictionary<string, string> tmp = icmpv6List[s];
                        ICollection codes = tmp.Keys;
                        foreach (string c in codes)
                        {
                            // add each to the table
                            object[] t = { s, c, icmpv6List[s][c], "IPv6" };
                            tableDisplay.Rows.Add(t);
                        }
                    }
                }

                // set button text
                viewICMP.Text = "Hide ICMP";
            }

            // if they're viewing the ICMP stuff
            else
            {
                // clear it and rebuild from the ruletable
                tableDisplay.Rows.Clear();
                rebuildTable();

                // update button text
                viewICMP.Text = "View ICMP";
            }

            // swap the fields and buttons based on !(state)
            typeField.Enabled = !(typeField.Enabled);
            codeField.Enabled = !(codeField.Enabled);
            addButton.Enabled = !(addButton.Enabled);
            deleteButton.Enabled = !(deleteButton.Enabled);
            allBox.Enabled = !(allBox.Enabled);
            blockIPv6Box.Enabled = !(blockIPv6Box.Enabled);
            allButNDP.Enabled = !(allButNDP.Enabled);
        }

        /// <summary>
        /// Rebuilds the datagridview
        /// </summary>
        private void rebuildTable()
        {
            ICollection keys = ruletable.Keys;
            ICollection keysv6 = ruletablev6.Keys;
            List<string> lVal;

            // add the v4 keys/codes
            foreach (string type in keys)
            {
                // get the list of codes from the key
                ruletable.TryGetValue(type, out lVal);

                // iterate through the codes, adding them to the table
                // with the appropriate description
                foreach (string code in lVal)
                {
                    object[] row = { type, code, icmpv4List[type][code], "IPv4" };
                    tableDisplay.Rows.Add(row);
                }
            }

            // add v6 keys/codes
            foreach (string type in keysv6)
            {
                ruletablev6.TryGetValue(type, out lVal);
                
                foreach ( string code in lVal )
                {
                    object[] row = { type, code, icmpv6List[type][code], "IPv6" };
                    tableDisplay.Rows.Add(row);
                }
            }
        }

        // if they're viewing ICMP information and check/uncheck the IPv6 box,
        // it should repopulate the ICMP info with the correct version's info
        private void ipv6Box_CheckedChanged(object sender, EventArgs e)
        {
            // if they checked the IPv6 box and they're currently viewing
            // ICMPv4 rules, reload it with IPv6 rules
            if (ipv6Box.Checked && !typeField.Enabled)
            {
                // clear table data
                tableDisplay.Rows.Clear();

                // get all the keys
                ICollection keys = icmpv6List.Keys;
                foreach (string s in keys)
                {
                    // get all the codes
                    Dictionary<string, string> temp = icmpv6List[s];
                    ICollection codes = temp.Keys;
                    foreach (string c in codes)
                    {
                        // add each to the table
                        object[] t = { s, c, icmpv6List[s][c], "IPv6" };
                        tableDisplay.Rows.Add(t);
                    }
                }
            }
            // if they unchecked it and they were viewing ICMPv6 data,
            // load ICMPv4 rules up
            else if (!ipv6Box.Checked && !typeField.Enabled)
            {
                // clear table data
                tableDisplay.Rows.Clear();

                // get all the keys
                ICollection keys = icmpv4List.Keys;
                foreach (string s in keys)
                {
                    // get all the codes
                    Dictionary<string, string> temp = icmpv4List[s];
                    ICollection codes = temp.Keys;
                    foreach (string c in codes)
                    {
                        // add each to the table
                        object[] t = { s, c, icmpv4List[s][c], "IPv4" };
                        tableDisplay.Rows.Add(t);
                    }
                }
            }
        }

        /// <summary>
        /// Set the logging mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logBox_CheckedChanged(object sender, EventArgs e)
        {
            this.filter.data.Log = logBox.Checked;
        }

        /*
         * Builds the ICMP type/code/description object for desc retrieval
         * http://www.iana.org/assignments/icmp-parameters/icmp-parameters.xml
         * 
         * iterate through all 41 supported ICMP types, add the codes, 
         * then add the type with the temp dict.
         */
        private void buildICMPListv4()
        {
            for (int i = 0; i <= 41; ++i)
            {
                Dictionary<string, string> temp = new Dictionary<string, string>();
                switch (i)
                {
                    // echo
                    case 0:
                        temp.Add("0", "Echo Reply");
                        icmpv4List.Add("0", temp);
                        break;

                    // reserve
                    case 1: 
                        temp.Add("0", "Reserved");
                        temp.Add("1", "Reserved");
                        icmpv4List.Add("1", temp);
                        break;

                    // reserve
                    case 2:
                        temp.Add("0", "Reserved");
                        temp.Add("1", "Reserved");
                        icmpv4List.Add("2", temp);
                        break;

                    // destination unreachable
                    case 3:
                        temp.Add("0", "Destination Network Unreachable");
                        temp.Add("1", "Destination Host Unreachable");
                        temp.Add("2", "Destination Protocol Unreachable");
                        temp.Add("3", "Destination Port Unreachable");
                        temp.Add("4", "Fragmentation Required, and DF Flag Set");
                        temp.Add("5", "Source Route Failed");
                        temp.Add("6", "Destination Network Unknown");
                        temp.Add("7", "Destination Host Unknown");
                        temp.Add("8", "Source Host Isolated");
                        temp.Add("9", "Network Administratively Prohibited");
                        temp.Add("10", "Host Administratively Prohibited");
                        temp.Add("11", "Network Unreachable for TOS");
                        temp.Add("12", "Host Unreachable for TOS");
                        temp.Add("13", "Communication Administratively Prohibited");
                        icmpv4List.Add("3", temp);
                        break;

                    // source quench
                    case 4:
                        temp.Add("0", "Source Quenched(Congestion Control)");
                        icmpv4List.Add("4", temp);
                        break;

                    // redirect message
                    case 5:
                        temp.Add("0", "Redirect Datagram For the Network");
                        temp.Add("1", "Redirect Datagram For the Host");
                        temp.Add("2", "Redirect Datagram For the TOS & Network");
                        temp.Add("3", "Redirect Datagram For the TOS & Host");
                        icmpv4List.Add("5", temp);
                        break;

                    // alt host addr
                    case 6:
                        temp.Add("0", "Alternate Host Address");
                        icmpv4List.Add("6", temp);
                        break;

                    // alt host addr
                    case 7:
                        temp.Add("0", "Reserved");
                        icmpv4List.Add("7", temp);
                        break;

                    // echo request
                    case 8:
                        temp.Add("0", "Echo Request (Used to Ping)");
                        icmpv4List.Add("8", temp);
                        break;

                    // router advertisement
                    case 9:
                        temp.Add("0", "Router Advertisement");
                        icmpv4List.Add("9", temp);
                        break;

                    // router solicitation
                    case 10:
                        temp.Add("0", "Router Discovery/Selection/Solicitation");
                        icmpv4List.Add("10", temp);
                        break;

                    // time exceeded
                    case 11:
                        temp.Add("0", "TTL Expired in Transit");
                        temp.Add("1", "Fragment Reassembly Time Exceeded");
                        icmpv4List.Add("11", temp);
                        break;

                     // bad IP header
                    case 12:
                        temp.Add("0", "Pointer Indicates the Error");
                        temp.Add("1", "Missing A Required Option");
                        temp.Add("2", "Bad Length");
                        icmpv4List.Add("12", temp);
                        break;

                     // Timestamp
                    case 13:
                        temp.Add("0", "Timestamp");
                        icmpv4List.Add("13", temp);
                        break;
                    
                     // Timestamp reply
                    case 14:
                        temp.Add("0", "Timestamp Reply");
                        icmpv4List.Add("14", temp);
                        break;

                     // information request
                    case 15:
                        temp.Add("0", "Information Request");
                        icmpv4List.Add("15", temp);
                        break;

                     // information reply
                    case 16:
                        temp.Add("0", "Information Reply");
                        icmpv4List.Add("16", temp);
                        break;

                     // address mask request
                    case 17:
                        temp.Add("0", "Address Mask Request");
                        icmpv4List.Add("17", temp);
                        break;

                     // Address mask reply
                    case 18:
                        temp.Add("0", "Address Mask Reply");
                        icmpv4List.Add("18", temp);
                        break;

                     // Reserved
                    case 19:
                        temp.Add("0", "Reserved For Security");
                        icmpv4List.Add("19", temp);
                        break;

                    // traceroute
                    case 30:
                        temp.Add("0", "Information Request");
                        icmpv4List.Add("30", temp);
                        break;
                    
                    // datagram 
                    case 31:
                        temp.Add("0", "Datagram Conversion Error");
                        icmpv4List.Add("31", temp);
                        break;

                    // mobile host redirect
                    case 32:
                        temp.Add("0", "Mobile Host Redirect");
                        icmpv4List.Add("32", temp);
                        break;

                    // where-are-you
                    case 33:
                        temp.Add("0", "Where-Are-You (Originally meant for IPv6)");
                        icmpv4List.Add("33", temp);
                        break;

                    // here-i-am
                    case 34:
                        temp.Add("0", "Here-I-Am (Originally meant for IPv6)");
                        icmpv4List.Add("34", temp);
                        break;

                    // mobile reg
                    case 35:
                        temp.Add("0", "Mobile Registration Request");
                        icmpv4List.Add("35", temp);
                        break;

                    // mobile reg
                    case 36:
                        temp.Add("0", "Mobile Registration Reply");
                        icmpv4List.Add("36", temp);
                        break;

                    // domain name request
                    case 37:
                        temp.Add("0", "Domain Name Request");
                        icmpv4List.Add("37", temp);
                        break;

                    // domain name reply
                    case 38:
                        temp.Add("0", "Domain Name Reply");
                        icmpv4List.Add("38", temp);
                        break;
                
                    // SKIP
                    case 39:
                        temp.Add("0", "SKIP Algorithm Discovery Protocol");
                        icmpv4List.Add("39", temp);
                        break;

                    // Photuris
                    case 40:
                        temp.Add("0", "Photuris, Security Failures");
                        icmpv4List.Add("40", temp);
                        break;

                    // experimental ICMP
                    case 41:
                        temp.Add("0", "ICMP For Experimental Mobility Protocols");
                        icmpv4List.Add("41", temp);
                        break;
                }
            }
        }

        private void buildICMPListv6()
        {
            for (int i = 0; i < 36; ++i)
            {
                Dictionary<string, string> tmp = new Dictionary<string, string>();

                switch (i)
                {
                    // destination unreachable
                    case 0:
                        tmp.Add("0", "No route to destination");
                        tmp.Add("1", "Communication with destination administratively prohibited");
                        tmp.Add("2", "Communication with destination administratively prohibited");
                        tmp.Add("3", "Address Unreachable");
                        tmp.Add("4", "Port Unreachable");
                        tmp.Add("5", "Source address failed ingress/egress policy");
                        tmp.Add("6", "Reject route to destination");
                        tmp.Add("7", "Error in source routing header");
                        icmpv6List.Add("1", tmp);
                        break;
                    
                    // packet too big
                    case 1:
                        tmp.Add("0", "Packet too big");
                        icmpv6List.Add("2", tmp);
                        break;

                    // time exceeded
                    case 2:
                        tmp.Add("0", "Hop limit exceeded in transit");
                        tmp.Add("1", "Fragment reassembly time exceeded");
                        icmpv6List.Add("3", tmp);
                        break;

                    // parameter problem
                    case 3:
                        tmp.Add("0", "Erroneous header field encountered");
                        tmp.Add("1", "Unrecognized next header type encountered");
                        tmp.Add("2", "Unrecognized IPv6 option encountered");
                        icmpv6List.Add("4", tmp);
                        break;

                    // echo request
                    case 4:
                        tmp.Add("0", "Echo request");
                        icmpv6List.Add("128", tmp);
                        break;

                    // echo reply
                    case 5:
                        tmp.Add("0", "Echo reply");
                        icmpv6List.Add("129", tmp);
                        break;

                    // multicast listener query
                    case 6:
                        tmp.Add("0", "General/Multicast query");
                        icmpv6List.Add("130", tmp);
                        break;

                    // multicast listener report
                    case 7:
                        tmp.Add("0", "Multicast listener report");
                        icmpv6List.Add("131", tmp);
                        break;

                    // multicast listener done
                    case 8:
                        tmp.Add("0", "Multicast listener done");
                        icmpv6List.Add("132", tmp);
                        break;

                    // router solicitation (NDP)
                    case 9:
                        tmp.Add("0", "Router Solicitation (NDP)");
                        icmpv6List.Add("133", tmp);
                        break;

                    // router advertisement (NDP)
                    case 10:
                        tmp.Add("0", "Router Advertisement (NDP)");
                        icmpv6List.Add("134", tmp);
                        break;

                    // Neighbor Solicitation (NDP)
                    case 11:
                        tmp.Add("0", "Neighbor Solicitation (NDP)");
                        icmpv6List.Add("135", tmp);
                        break;

                    // Neighbor Advertisement (NDP)
                    case 12:
                        tmp.Add("0", "Router Advertisement (NDP)");
                        icmpv6List.Add("136", tmp);
                        break;

                    // Redirect Message (NDP)
                    case 13:
                        tmp.Add("0", "Redirect Message (NDP)");
                        icmpv6List.Add("137", tmp);
                        break;

                    // Router Renumbering
                    case 14:
                        tmp.Add("0", "Router Renumbering Command");
                        tmp.Add("1", "Router Renumbering Result");
                        tmp.Add("255", "Sequence Number Reset");
                        icmpv6List.Add("138", tmp);
                        break;

                    // ICMP Node information query
                    case 15:
                        tmp.Add("0", "Data field contains IPv6 Address");
                        tmp.Add("1", "Data field contains a name or is empty");
                        tmp.Add("2", "Data field contains IPv4 Address");
                        icmpv6List.Add("139", tmp);
                        break;

                    // ICMP Node information response
                    case 16:
                        tmp.Add("0", "Successful Reply");
                        tmp.Add("1", "Responder refuses to supply answer");
                        tmp.Add("2", "Qtype of query is unknown to responder");
                        icmpv6List.Add("140", tmp);
                        break;

                    // inverse neighbor discovery solicitation message
                    case 17:
                        tmp.Add("0", "Inverse neighbor discovery solicitation message");
                        icmpv6List.Add("141", tmp);
                        break;
                    
                    // inverse neighbor discovery advertisement message
                    case 18:
                        tmp.Add("0", "Inverse neighbor discovery advertisement message");
                        icmpv6List.Add("142", tmp);
                        break;
                    
                    // Multicast listener discovery reports
                    case 19:
                        tmp.Add("0", "Multicast listener discovery reports");
                        icmpv6List.Add("143", tmp);
                        break;

                    // Home agent address discovery request message
                    case 20:
                        tmp.Add("0", "Home agent address discovery request message");
                        icmpv6List.Add("144", tmp);
                        break;

                    // Home agent address discovery reply message 
                    case 21:
                        tmp.Add("0", "Home agent address discovery reply message");
                        icmpv6List.Add("145", tmp);
                        break;

                    // Mobile prefix solicitation
                    case 22:
                        tmp.Add("0", "Mobile prefix solicitation");
                        icmpv6List.Add("146", tmp);
                        break;

                    // Mobile prefix advertisement
                    case 23:
                        tmp.Add("0", "Mobile prefix advertisement");
                        icmpv6List.Add("147", tmp);
                        break;

                    // Certification Path Solicitation
                    case 24:
                        tmp.Add("0", "Certification path solicitation");
                        icmpv6List.Add("148", tmp);
                        break;

                    // Certification path advertisement
                    case 25:
                        tmp.Add("0", "Certification path advertisement");
                        icmpv6List.Add("149", tmp);
                        break;

                    // Multicast Router Advertisement
                    case 26:
                        tmp.Add("0", "Multicast Router Advertisement");
                        icmpv6List.Add("151", tmp);
                        break;

                    // Multicast router solicitation
                    case 27:
                        tmp.Add("0", "Multicast router solicitation");
                        icmpv6List.Add("152", tmp);
                        break;

                    // Multicast router termination
                    case 28:
                        tmp.Add("0", "Multicast router termination");
                        icmpv6List.Add("153", tmp);
                        break;

                    // RPL Control message
                    case 29:
                        tmp.Add("0", "RPL Control message");
                        icmpv6List.Add("155", tmp);
                        break;
                }
            }
        }
    }
}