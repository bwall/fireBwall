using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using fireBwall.UI;
using fireBwall.Logging;
using fireBwall.Configuration;

namespace fireBwall.UI.Tabs
{
    /// <summary>
    /// Class to represent the tray icon and manage whats displayed from it
    /// </summary>
    public class TrayIcon
    {
        static TrayPopup popup;

        void Shutdown(object o, EventArgs args)
        {
            Program.Shutdown();
        }

        /// <summary>
        /// Makes the actual NotifyIcon, and hooks up all the events for it
        /// </summary>
        public TrayIcon()
        {
            ContextMenu cm = new ContextMenu();
            List<MenuItem> links = new List<MenuItem>();
            MenuItem closeButton = new MenuItem("Exit", new EventHandler(Shutdown));

            adapters = new MenuItem("Adapters");
            cm.MenuItems.Add(adapters);

            links.Add(new MenuItem("fireBwall.com", new EventHandler(ToFirebwallCom)));
            links.Add(new MenuItem("Facebook", new EventHandler(ToFacebook)));
            links.Add(new MenuItem("Reddit", new EventHandler(ToReddit)));
            links.Add(new MenuItem("Twitter", new EventHandler(ToTwitter)));
            links.Add(new MenuItem("fireBwall's Modules", new EventHandler(ToModules)));
            links.Add(new MenuItem("fireBwall's Themes", new EventHandler(ToThemes)));
            links.Add(new MenuItem("fireBwall Forum", new EventHandler(ToForum)));
            links.Add(new MenuItem("fireBwall Trello", new EventHandler(ToTrello)));
            cm.MenuItems.Add("Links", links.ToArray());

            cm.MenuItems.Add(closeButton);
            tray = new NotifyIcon();
            tray.ContextMenu = cm;
            tray.Icon = DynamicForm.GetIcon();
            tray.Visible = true;
            tray.DoubleClick += new EventHandler(tray_DoubleClick);
            popup = new TrayPopup();
            popup.Show();
            popup.Location = new System.Drawing.Point(Screen.PrimaryScreen.WorkingArea.Width - popup.Width, Screen.PrimaryScreen.WorkingArea.Height - popup.Height);
            popup.Visible = false;
            LogCenter.Instance.PushLogEvent += AddLine;
        }

        NotifyIcon tray;
        public MenuItem adapters;

        void ToTrello(object we, EventArgs dontMatter)
        {
            System.Diagnostics.Process.Start("https://trello.com/board/firebwall/4f6d3d48255ed1e9081e88ed");
        }

        void ToForum(object we, EventArgs dontMatter)
        {
            System.Diagnostics.Process.Start("http://firebwall.proboards.com");
        }

        void ToFirebwallCom(object we, EventArgs dontMatter)
        {
            System.Diagnostics.Process.Start("https://firebwall.com");
        }

        private void ToFacebook(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/pages/FireBwall/261822493882169");
        }

        private void ToReddit(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.reddit.com/r/firebwall/");
        }

        private void ToModules(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://firebwall.com/modules.php");
        }

        private void ToThemes(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://firebwall.com/themes.php");
        }

        private void ToTwitter(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://twitter.com/#!/firebwall");
        }

        /// <summary>
        /// Queue of lines to display in the pop up balloon
        /// </summary>
        Queue<string> lines = new Queue<string>();

        /// <summary>
        /// Adds a line to the display queue
        /// </summary>
        /// <param name="line"></param>
        public void AddLine(LogEvent line)
        {
            // only display if checked AND the return type is to notify
            if (GeneralConfiguration.Instance.ShowPopups && line.Module.GetUserInterface() != null && ((line.PMR & fireBwall.Modules.PacketMainReturnType.Popup) == fireBwall.Modules.PacketMainReturnType.Popup))
            {
                popup.AddLogEvent(line);
            }
        }

        /// <summary>
        /// Disposes of the tray
        /// </summary>
        public void Dispose()
        {
            popup.Close();
            tray.Dispose();
        }

        /// <summary>
        /// Controls the main window with double clicks on the tray icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tray_DoubleClick(object sender, EventArgs e)
        {
            Program.mainWindow.Visible = !Program.mainWindow.Visible;
            Program.mainWindow.WindowState = FormWindowState.Normal;
            Program.mainWindow.Activate();
        }
    }
}
