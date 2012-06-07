using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net.NetworkInformation;
using System.Net;

using fireBwall.Packets;
using fireBwall.UI;
using fireBwall.Utils;

namespace IPMonitor
{
    public partial class IPMonitorDisplay : DynamicUserControl
    {
        // local ipmonitoring mod
        private IPMonitorModule ipmon;
        
        // local tcp cache
        private List<TcpConnectionInformation> tcpcache;
        public List<TcpConnectionInformation> Tcpcache
             { get { return tcpcache; } set { tcpcache = new List<TcpConnectionInformation>(value); } }

        // local udp cache
        private List<IPEndPoint> udpcache;
        public List<IPEndPoint> Udpcache
            { get { return udpcache; } set { udpcache = new List<IPEndPoint>(value); } }

        public IPMonitorDisplay(IPMonitorModule mon)
        {
            this.ipmon = mon;
            tcpcache = new List<TcpConnectionInformation>();
            InitializeComponent();
        }

        /// <summary>
        /// Update the TCP connections
        /// </summary>
        private void TCPUpdate()
        {
            // preserve the idx of the viewport
            int idx = tcpDisplay.FirstDisplayedScrollingRowIndex;

            // if the datasource hasn't been set, set it
            if (tcpDisplay.DataSource != this.tcpcache)
                tcpDisplay.DataSource = this.tcpcache;

            // update local tcpcache
            tcpcache = new List<TcpConnectionInformation>(ipmon.Tcpcache);
            
            // -1 = no viewport
            if ( idx >= 0 )
                tcpDisplay.FirstDisplayedScrollingRowIndex = idx;

            // set the total connections label
            tcpTotal.Text = String.Format("{0}: {1}","Connections", tcpcache.Count.ToString());
        }

        /// <summary>
        /// Update the UDP connections
        /// </summary>
        private void UDPUpdate()
        {
            int idx = udpDisplay.FirstDisplayedScrollingRowIndex;

            if (udpDisplay.DataSource != this.udpcache)
                udpDisplay.DataSource = this.udpcache;

            udpcache = new List<IPEndPoint>(ipmon.Udpcache);

            if (idx >= 0)
                udpDisplay.FirstDisplayedScrollingRowIndex = idx;

            udpTotal.Text = String.Format("{0}: {1}", "Listeners", udpcache.Count.ToString());
        }

        /// <summary>
        /// Set the TCP statistics fields
        /// </summary>
        private void DumpStats()
        {
            IPGlobalProperties stats = IPGlobalProperties.GetIPGlobalProperties();
            TcpStatistics tcpstat = stats.GetTcpIPv4Statistics();
            UdpStatistics udpstat = stats.GetUdpIPv4Statistics();

            // set labels 
            // this doesn't need to be done each time
            setLabels();

            // set the TCP field labels
            connAcceptField.Text = tcpstat.ConnectionsAccepted.ToString();
            connInitiatedField.Text = tcpstat.ConnectionsInitiated.ToString();
            cumulativeConnectionsField.Text = tcpstat.CumulativeConnections.ToString();

            errorsReceivedField.Text = tcpstat.ErrorsReceived.ToString();
            failedConAttField.Text = tcpstat.FailedConnectionAttempts.ToString();
            maxConnField.Text = tcpstat.MaximumConnections.ToString();

            resetConnField.Text = tcpstat.ResetConnections.ToString();
            resetsSentField.Text = tcpstat.ResetsSent.ToString();
            segsReceivedField.Text = tcpstat.SegmentsReceived.ToString();

            segsResentField.Text = tcpstat.SegmentsResent.ToString();
            segsSentField.Text = tcpstat.SegmentsSent.ToString();

            // set the UDP field labels
            dataReceivedField.Text = udpstat.DatagramsReceived.ToString();
            dataSentField.Text = udpstat.DatagramsSent.ToString();

            incDataDiscField.Text = udpstat.IncomingDatagramsDiscarded.ToString();
            incDataErrField.Text = udpstat.IncomingDatagramsWithErrors.ToString();
        }
        /// <summary>
        /// invoke the TCP datagridview updater
        /// </summary>
        public void UpdateTCP()
        {
            if (tcpDisplay.InvokeRequired)
            {
                System.Threading.ThreadStart up = new System.Threading.ThreadStart(TCPUpdate);
                tcpDisplay.Invoke(up);
            }
            else
                TCPUpdate();
        }

        /// <summary>
        /// invoke the UDP datagridview updater
        /// </summary>
        public void UpdateUDP()
        {
            if (udpDisplay.InvokeRequired)
            {
                System.Threading.ThreadStart up = new System.Threading.ThreadStart(UDPUpdate);
                udpDisplay.Invoke(up);
            }
            else
                UDPUpdate();
        }

        /// <summary>
        /// invoke the statistics updater
        /// </summary>
        public void UpdateStats()
        {
            if (statistics.InvokeRequired)
            {
                System.Threading.ThreadStart up = new System.Threading.ThreadStart(DumpStats);
                statistics.Invoke(up);
            }
            else
                DumpStats();
        }

        /// <summary>
        /// set the labels as language-specific
        /// </summary>
        private void setLabels()
        {
            connAcceptLabel.Text = ipmon.multistring.GetString("Connections Accepted");
            connInitLabel.Text = ipmon.multistring.GetString("Connections Initiated");
            cumulativeConnLabel.Text = ipmon.multistring.GetString("Cumulative Connections");
            errRecLabel.Text = ipmon.multistring.GetString("Errors Received");
            failedConnAttLabel.Text = ipmon.multistring.GetString("Failed Connection Attempts");
            maxConLabel.Text = ipmon.multistring.GetString("Maximum Connections");
            resetConLabel.Text = ipmon.multistring.GetString("Reset Connections");
            resetsSentLabel.Text = ipmon.multistring.GetString("Resets Sent");
            segRecLabel.Text = ipmon.multistring.GetString("Segments Received");
            segResentLabel.Text = ipmon.multistring.GetString("Segments Resent");
            segSentLabel.Text = ipmon.multistring.GetString("Segments Sent");
            dataRecLabel.Text = ipmon.multistring.GetString("Datagrams Received");
            dataSentLabel.Text = ipmon.multistring.GetString("Datagrams Sent");
            incDataDiscLabel.Text = ipmon.multistring.GetString("Incoming Datagrams Discarded");
            incDataWErrLabel.Text = ipmon.multistring.GetString("Incoming Datagrams With Errors");
        }
    }
}
