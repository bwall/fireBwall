using System;
using System.Collections.Generic;
using System.Text;
using System.Net.NetworkInformation;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Timers;

using fireBwall.Utils;
using fireBwall.Configuration;
using fireBwall.Modules;
using fireBwall.Logging;
using fireBwall.Packets;

namespace IPMonitor
{
    /// <summary>
    /// IP monitoring module for viewing listening connections, i/o, and 
    /// traffic
    /// </summary>
    public class IPMonitorModule : NDISModule
    {
        // cache of TCP connections
        private List<TcpConnectionInformation> tcpcache = new List<TcpConnectionInformation>();
        public List<TcpConnectionInformation> Tcpcache 
                { get { return tcpcache; } set { tcpcache = new List<TcpConnectionInformation>(value); } }

        // cache of UDP listeners
        private List<IPEndPoint> udpcache = new List<IPEndPoint>();
        public List<IPEndPoint> Udpcache
            { get { return udpcache; } set { udpcache = new List<IPEndPoint>(value); } }

        // ipdisplay
        private IPMonitorDisplay ipmon;

        // update timer
        private System.Timers.Timer updateTimer = new System.Timers.Timer();

        public MultilingualStringManager multistring = new MultilingualStringManager();

         /// <summary>
         /// Class constructor
         /// </summary>
        public IPMonitorModule()
            : base()
        {
            Help();
        }

        /// <summary>
        /// Returns a GUI handle
        /// </summary>
        /// <returns></returns>
        public override fireBwall.UI.DynamicUserControl GetUserInterface()
        {
            try
            {
                ipmon = new IPMonitorDisplay(this) { Dock = System.Windows.Forms.DockStyle.Fill };
                // force a tick when the GUI is loaded
                timer_Tick(updateTimer, null);
            }
            catch (Exception ex)
            {
                LogCenter.Instance.LogException(ex);
            }
            return ipmon;
        }

        /// <summary>
        /// Starts the module
        /// </summary>
        /// <returns></returns>
        public override bool ModuleStart()
        {
            try
            {
                ipmon = new IPMonitorDisplay(this) { Dock = System.Windows.Forms.DockStyle.Fill };

                // configure the update timer to refresh the caches every
                // 5 seconds
                updateTimer.Elapsed += new ElapsedEventHandler(timer_Tick);
                updateTimer.Interval = 5000;
                updateTimer.Enabled = true;
                updateTimer.Start();
            }
            catch (Exception e)
            {
                LogCenter.Instance.LogException(e);
                return false;
            }
            return true;
        }

        /// <summary>
        /// ModuleStop
        /// </summary>
        /// <returns></returns>
        public override bool ModuleStop()
        {
            // why not
            return true;
        }

        /// <summary>
        /// main module routine.
        /// Right now it doesn't do anything with individual packets, but in the future the
        /// user will be able to parse through data based on a certain connection.
        /// </summary>
        /// <param name="in_packet"></param>
        /// <returns></returns>
        public override PacketMainReturnType interiorMain(ref Packet in_packet)
        {
            return PacketMainReturnType.Allow;
        }

        /// <summary>
        /// Event handler for when the 5 second update timer goes off.
        /// Updates both the local TCP/UDP caches, then calls the GUI invoker methods.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            UpdateTCP();
            UpdateUDP();
            ipmon.UpdateTCP();
            ipmon.UpdateUDP();
            ipmon.UpdateStats();
        }

        /// <summary>
        /// Method used to update TCP connections
        /// </summary>
        private void UpdateTCP()
        {
            // get the connection info
            IPGlobalProperties ipGlob = IPGlobalProperties.GetIPGlobalProperties();
            List<TcpConnectionInformation> tcpInfo = new List<TcpConnectionInformation>(ipGlob.GetActiveTcpConnections());
            List<TcpConnectionInformation> temp = new List<TcpConnectionInformation>(tcpcache);

            // remove invalid connections
            foreach (TcpConnectionInformation tci in temp)
            {
                if (!(tcpInfo.Contains(tci)))
                    tcpcache.Remove(tci);
            }
            
            // if the cache doesn't contain the TcpConnection, add it
            foreach (TcpConnectionInformation tci in tcpInfo)
            {
                if ( !(tcpcache.Contains(tci)))
                    tcpcache.Add(tci);
            }
        }

        /// <summary>
        /// Method used to update UDP connections
        /// </summary>
        private void UpdateUDP()
        {
            // get the connection info
            IPGlobalProperties ipGlob = IPGlobalProperties.GetIPGlobalProperties();
            List<IPEndPoint> udpInfo = new List<IPEndPoint>(ipGlob.GetActiveUdpListeners());
            List<IPEndPoint> temp = new List<IPEndPoint>(udpcache);

            // remove old connections
            foreach (IPEndPoint ip in temp)
            {
                if (!(udpInfo.Contains(ip)))
                    udpcache.Remove(ip);
            }
            
            // add new connections
            foreach (IPEndPoint ip in udpInfo)
            {
                if ( !(udpcache.Contains(ip)))
                    udpcache.Add(ip);
            }
        }

        // module metadata
        private void Help()
        {
            ModuleMeta.Meta m = new ModuleMeta.Meta();
            m.Name = "IP Monitor";
            m.Version = "1.0.2.0";
            m.Description = "Displays varying information about network connections";
            m.Contact = "shodivine@gmail.com";
            m.Author = "Bryan A. (drone)";
            m.Help = "The IP Monitor module displays connections and their corresponding ports on the localhost's TCP/UDP sockets.  The list is "
                + "refreshed every 5 seconds to keep up with the frequency of lost/established connections.  The 'state' column of TCP connections can be one of "
                + "nine different states.  They are: \n"
                + "1. LISTENING\t\tIn case of a server, waiting for a connection request form any remote client.\n"
                + "2. SYN-SENT\t\tWaiting for a remote peer to send back a SYN/ACK.\n"
                + "3. SYN-RECEIVED\t\tWaiting for a remote peer to send back an ACK.\n"
                + "4. ESTABLISHED\t\tThe port is ready to receive/send data to/from a peer.\n"
                + "5/6. FIN-WAIT-(1|2)\t\tThe client is waiting for the server's FIN.\n"
                + "7.. CLOSE-WAIT\t\tIndicates the server is sending it's own FIN.\n"
                + "8. TIME-WAIT\t\tWaiting to ensure peer received teardown ACK.\n"
                + "9. CLOSED\t\tConnection is closed.\n\n"
                + "The format of connections is IP:PORT";

            MetaData = new ModuleMeta(m);

            Language lang = Language.ENGLISH;
            multistring.SetString(lang, "Connections Accepted", "Connections Accepted:");
            multistring.SetString(lang, "Connections Initiated", "Connections Initiated:");
            multistring.SetString(lang, "Cumulative Connections", "Cumulative Connections:");
            multistring.SetString(lang, "Errors Received", "Errors Received:");
            multistring.SetString(lang, "Failed Connection Attempts", "Failed Connection Attempts:");
            multistring.SetString(lang, "Maximum Connections", "Maximum Connections:");
            multistring.SetString(lang, "Reset Connections", "Reset Connections:");
            multistring.SetString(lang, "Resets Sent", "Resets Sent:");
            multistring.SetString(lang, "Segments Received", "Segments Received:");
            multistring.SetString(lang, "Segments Resent", "Segments Resent:");
            multistring.SetString(lang, "Segments Sent", "Segments Sent:");
            multistring.SetString(lang, "Datagrams Received", "Datagrams Received:");
            multistring.SetString(lang, "Datagrams Sent", "Datagrams Sent:");
            multistring.SetString(lang, "Incoming Datagrams Discarded", "Incoming Datagrams Discarded:");
            multistring.SetString(lang, "Incoming Datagrams With Errors", "Incoming Datagrams With Errors:");
            
        }
    }
}
