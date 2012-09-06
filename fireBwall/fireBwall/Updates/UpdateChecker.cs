using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;
using fireBwall.Configuration;
using fireBwall.UI.Tabs;

namespace fireBwall.Updates
{
    public class fireBwallMetaData
    {
        public List<string> changelog = new List<string>();
        public string Description = "";
        public string filename = "";
        public string version = "";
        public string downloadUrl = "";
        public string imageUrl = "";
        public List<string> screenShotUrls = new List<string>();
    }

    public class UpdateChecker
    {
        static int versionA = 0;
        static int versionB = 3;
        static int versionC = 12;
        static int versionD = 1;
        static Guid instanceGuid = Guid.NewGuid();
        Thread updateThread;

        public static fireBwallMetaData availableFirebwall = null;
        static object padlock = new object();

        public void Updater()
        {
            updateThread = new Thread(new ThreadStart(UpdateLoop));
            updateThread.Name = "Update Loop";
            updateThread.Start();
        }

        public void Close()
        {
            if(updateThread != null)
                updateThread.Abort();
        }

        public void MyDownloadFile(Uri url, string outputFilePath)
        {
            const int BUFFER_SIZE = 16 * 1024;
            using (var outputFileStream = File.Create(outputFilePath, BUFFER_SIZE))
            {
                WebRequest req = WebRequest.Create(url);
                using (WebResponse response = req.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        byte[] buffer = new byte[BUFFER_SIZE];
                        int bytesRead;
                        do
                        {
                            bytesRead = responseStream.Read(buffer, 0, BUFFER_SIZE);
                            outputFileStream.Write(buffer, 0, bytesRead);
                        } while (bytesRead > 0);
                    }
                }
            }
        }

        public void UpdateFirebwallMetaVersion()
        {
            try
            {
                WebClient client = new WebClient();
                client.Headers[HttpRequestHeader.UserAgent] = "firebwall 0.3.12.1 Updater";
                string xml = client.DownloadString("https://www.firebwall.com/api/firebwall/" + GeneralConfiguration.Instance.PreferredLanguage + ".xml?min=" + GeneralConfiguration.Instance.IntervaledUpdateMinutes.ToString() + "&instance=" + UpdateChecker.instanceGuid.ToString());
                XmlTextReader reader = new XmlTextReader(new MemoryStream(Encoding.UTF8.GetBytes(xml)));
                lock (padlock)
                {
                    if (!reader.Read())
                    {
                        availableFirebwall = null;
                        reader.Close();
                        return;
                    }
                    availableFirebwall = new fireBwallMetaData();
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                if (reader.Name == "current")
                                {
                                    availableFirebwall.version = reader.GetAttribute("version");
                                }
                                else if (reader.Name == "filename")
                                {
                                    reader.Read();
                                    if (reader.NodeType == XmlNodeType.Text)
                                        availableFirebwall.filename = reader.Value;
                                }
                                else if (reader.Name == "description")
                                {
                                    reader.Read();
                                    if (reader.NodeType == XmlNodeType.Text)
                                        availableFirebwall.Description = reader.Value;
                                }
                                else if (reader.Name == "entry")
                                {
                                    reader.Read();
                                    if (reader.NodeType == XmlNodeType.Text)
                                        availableFirebwall.changelog.Add(reader.Value);
                                }
                                else if (reader.Name == "downloadurl")
                                {
                                    reader.Read();
                                    if (reader.NodeType == XmlNodeType.Text)
                                        availableFirebwall.downloadUrl = reader.Value;
                                }
                                else if (reader.Name == "imageurl")
                                {
                                    reader.Read();
                                    if (reader.NodeType == XmlNodeType.Text)
                                        availableFirebwall.imageUrl = reader.Value;
                                }
                                else if (reader.Name == "url")
                                {
                                    reader.Read();
                                    if (reader.NodeType == XmlNodeType.Text)
                                    {
                                        availableFirebwall.screenShotUrls.Add(reader.Value);
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            catch { }
        }

        public static bool IsVersionNew()
        {
            if (availableFirebwall == null)
                return false;
            string version = availableFirebwall.version;
            string a = version.Substring(0, version.IndexOf("."));
            if (versionA == int.Parse(a))
            {
                version = version.Substring(version.IndexOf(".") + 1);
                string b = version.Substring(0, version.IndexOf("."));
                if (versionB == int.Parse(b))
                {
                    version = version.Substring(version.IndexOf(".") + 1);
                    string c = version.Substring(0, version.IndexOf("."));
                    if (versionC == int.Parse(c))
                    {
                        version = version.Substring(version.IndexOf(".") + 1);
                        if (versionD == int.Parse(version))
                        {
                            return false;
                        }
                        else if (versionD < int.Parse(version))
                        {
                            return true;
                        }
                    }
                    else if (versionC < int.Parse(c))
                    {
                        return true;
                    }
                }
                else if (versionB < int.Parse(b))
                {
                    return true;
                }
            }
            else if (versionA < int.Parse(a))
            {
                return true;
            }
            return false;
        }

        public void UpdateLoop()
        {
            bool firstTime = true;
            while (true)
            {
                Thread.Sleep(30000);
                if ((firstTime && GeneralConfiguration.Instance.CheckUpdateOnStartup) || GeneralConfiguration.Instance.IntervaledUpdateChecks)
                {
                    try
                    {
                        UpdateFirebwallMetaVersion();
                        if (availableFirebwall != null && IsVersionNew())
                        {
                            Application.Run(DownloadCenter.Instance);
                            return;
                        }
                    }
                    catch (ThreadAbortException)
                    {
                        break;
                    }
                    catch (Exception e)
                    {
                        fireBwall.Logging.LogCenter.Instance.LogException(e);
                    }
                }
                firstTime = false;
                Thread.Sleep(new TimeSpan(0, (int)GeneralConfiguration.Instance.IntervaledUpdateMinutes, 0));
            }
        }
    }
}
