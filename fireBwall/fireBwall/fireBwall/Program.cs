using System;
using System.Windows.Forms;
using System.Threading;
using fireBwall.UI.Tabs;
using fireBwall.Configuration;
using fireBwall.Updates;
using fireBwall.Filters.NDIS;

namespace fireBwall
{
    public static class Program
    {
        public static event ThreadStart OnShutdown;
        public static MainWindow mainWindow = null;
        public static TrayIcon trayIcon = null;
        public static UpdateChecker updater = new UpdateChecker();

        public static void Shutdown()
        {
            ConfigurationManagement.Instance.SaveAllConfigurations();
            if (OnShutdown != null)
                OnShutdown();
            Application.Exit();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool setPath = false;
            if (args.Length != 0)
            {
                foreach (string arg in args)
                {
                    switch (arg)
                    {
                        case "-d":
                        case "--dev-mode":
                            break;
                        case "-m":
                        case "--start-minimized":
                            break;
                        case "-p":
                        case "--dont-show-popups":
                            break;
                        default:
                            ConfigurationManagement.Instance.ConfigurationPath = arg;
                            setPath = true;
                            break;
                    }
                }                
            }
            if (!setPath)
                ConfigurationManagement.Instance.ConfigurationPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + System.IO.Path.DirectorySeparatorChar + "fireBwall";
            ConfigurationManagement.Instance.LoadAllConfigurations();
            if (args.Length != 0)
            {
                foreach (string arg in args)
                {
                    switch (arg)
                    {
                        case "-d":
                        case "--dev-mode":
                            GeneralConfiguration.Instance.DeveloperMode = true;
                            break;
                        case "-m":
                        case "--start-minimized":
                            GeneralConfiguration.Instance.StartMinimized = true;
                            break;
                        case "-p":
                        case "--dont-show-popups":
                            GeneralConfiguration.Instance.ShowPopups = false;
                            break;
                    }
                }
            } 
            foreach (INDISFilter filter in ProcessingConfiguration.Instance.NDISFilterList.GetAllAdapters())
            {
                filter.StartProcessing();
            }
            Program.OnShutdown += ProcessingConfiguration.Instance.NDISFilterList.ShutdownAll;
            Program.OnShutdown += ConfigurationManagement.Instance.SaveAllConfigurations;
            Application.Run(new MainWindow());
        }
    }
}
