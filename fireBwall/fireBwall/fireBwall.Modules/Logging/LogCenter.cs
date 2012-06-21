using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using fireBwall.Utils;
using fireBwall.Configuration;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;

namespace fireBwall.Logging
{
    public sealed class LogCenter
    {
        #region ConcurrentSingleton

        private static volatile LogCenter instance;
        private static object syncRoot = new Object();

        private LogCenter() 
        { 
            eventLoop = new Thread(EventLoop);
            eventLoop.Name = "Log Event Loop";
            eventLoop.Start();
            debugLoop = new Thread(DebugLoop);
            debugLoop.Name = "Debug Event Loop";
            debugLoop.Start();
            errorLoop = new Thread(ExceptionLoop);
            errorLoop.Name = "Error Event Loop";
            errorLoop.Start();
        }

        public static LogCenter Instance
        {
            get 
            {
                if (instance == null) 
                {
                    lock (syncRoot)
                    {
                        instance = new LogCenter();
                    }
                }
                return instance;           
            }
        }

        #endregion

        #region Events

        public delegate void NewLogEvent(LogEvent e);
        public delegate void NewDebugLog(DebugLogMessage log);
        public delegate void NewExceptionLog(Exception e);
        public event NewLogEvent PushLogEvent;
        public event NewDebugLog PushDebugLogEvent;
        public event NewExceptionLog PushExceptionEvent;

        #endregion

        #region Variables

        SwapBufferQueue<LogEvent> EventQueue = new SwapBufferQueue<LogEvent>();
        SwapBufferQueue<Exception> ExceptionQueue = new SwapBufferQueue<Exception>();
        SwapBufferQueue<DebugLogMessage> DebugQueue = new SwapBufferQueue<DebugLogMessage>();
        Thread eventLoop;
        Thread debugLoop;
        Thread errorLoop;

        #endregion

        #region Loops

        public void Kill()
        {
            eventLoop.Abort();
            debugLoop.Abort();
            errorLoop.Abort();
        }

        void EventLoop()
        {
            while (true)
            {
                Thread.Sleep(50);
                LogEvent[] temp = EventQueue.DumpBuffer().ToArray();
                if (temp != null)
                {
                    foreach (LogEvent le in temp)
                    {
                        WriteLogFile(le);
                        if (PushLogEvent != null)
                            PushLogEvent(le);
                    }
                }
            }
        }

        void ExceptionLoop()
        {
            while (true)
            {
                Thread.Sleep(50);
                Exception[] temp = ExceptionQueue.DumpBuffer().ToArray();
                if (temp != null)
                {
                    foreach (Exception le in temp)
                    {
                        WriteErrorLog(le);
                        if (PushExceptionEvent != null)
                            PushExceptionEvent(le);
                    }
                }
            }
        }

        void DebugLoop()
        {
            while (true)
            {
                Thread.Sleep(50);
                DebugLogMessage[] temp = DebugQueue.DumpBuffer().ToArray();
                if (temp != null)
                {
                    foreach (DebugLogMessage le in temp)
                    {
                        WriteDebugLog(le);
                        if (PushDebugLogEvent != null)
                            PushDebugLogEvent(le);
                    }
                }
            }
        }

        #endregion

        #region Functions

        public void WriteDebugLog(DebugLogMessage log)
        {
            string currentdate = DateTime.Now.ToString("M-d-yyyy");
            string folder = ConfigurationManagement.Instance.ConfigurationPath + Path.DirectorySeparatorChar + "Log";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            string filepath = folder;
            string filename = Path.DirectorySeparatorChar + "DebugLog.log";

            FileStream stream;
            if (log != null)
            {
                if (Directory.Exists(filepath))
                {
                    if (File.Exists(filepath + filename))
                        stream = new FileStream(filepath + filename, FileMode.Append, FileAccess.Write, FileShare.Write);
                    else
                        stream = new FileStream(filepath + filename, FileMode.CreateNew, FileAccess.Write, FileShare.Write);

                    StreamWriter m_streamWriter = new StreamWriter(stream);
                    // write out the current date and message
                    // followed by the source and a stack trace
                    m_streamWriter.WriteLine(currentdate + " "+ log.ToString());

                    m_streamWriter.Close();
                    stream.Close();
                }
            }
        }

        // Write the exception out to the error log
        public void WriteErrorLog(Exception e)
        {
            string currentdate = DateTime.Now.ToString("M-d-yyyy");
            string folder = ConfigurationManagement.Instance.ConfigurationPath + Path.DirectorySeparatorChar + "Log";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            string filepath = folder;
            string filename = Path.DirectorySeparatorChar + "ErrorLog.log";

            FileStream stream;
            if (e != null)
            {
                if (Directory.Exists(filepath))
                {
                    if (File.Exists(filepath + filename))
                        stream = new FileStream(filepath + filename, FileMode.Append, FileAccess.Write, FileShare.Write);
                    else
                        stream = new FileStream(filepath + filename, FileMode.CreateNew, FileAccess.Write, FileShare.Write);

                    StreamWriter m_streamWriter = new StreamWriter(stream);
                    // write out the current date and message
                    // followed by the source and a stack trace
                    m_streamWriter.WriteLine(DateTime.Now.ToString() + " " + e.Message);
                    m_streamWriter.WriteLine(e.Source);
                    m_streamWriter.WriteLine(e.StackTrace);

                    m_streamWriter.Close();
                    stream.Close();
                }
            }
        }

        /*
        * pushes log events to the event log file
        * Log\Event_<date>.log
        * 
        * These logs are purged by the cleanLogs() method.
        */
        private void WriteLogFile(LogEvent le)
        {
            string currentdate = DateTime.Now.ToString("MM-dd-yyyy");
            string folder = ConfigurationManagement.Instance.ConfigurationPath;
            folder = folder + Path.DirectorySeparatorChar + "Log";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            string filepath = folder;
            string filename = Path.DirectorySeparatorChar + "Event_" + currentdate + ".log";

            FileStream stream;
            // if the log event is not null
            if (le != null)
            {
                // if the file exists, open in append and write to it
                if (File.Exists(filepath + filename))
                    stream = new FileStream(filepath + filename, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                else
                    stream = new FileStream(filepath + filename, FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite);

                StreamWriter m_streamWriter = new StreamWriter(stream);
                m_streamWriter.WriteLine(le.time.ToString() + " " + le.Module + ": " + le.Message + "\r");
                m_streamWriter.Close();
                stream.Close();
            }
        }

        /// <summary>
        /// Utility method for checking if a file is locked/in-use or not
        /// </summary>
        /// <param name="file"></param>
        /// <returns>True if the file is locked, false if it isn't</returns>
        private static bool isFileLocked(FileInfo file)
        {
            FileStream stream = null;
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                // the file cannot be opened for whatever reason; return that the file is indeed
                // locked.
                return true;
            }
            finally
            {
                if (null != stream)
                    stream.Close();
            }
            return false;
        }

        /*
        * Method called once per run, used to check log paths to 
        * clean up any old logs.  Called from MainWindow.cs - Load().
        * 
        * Logs are retained for however long the user requests (default is 5 days)
        */
        public void CleanLogs()
        {
            try
            {
                //
                // first clean up the /Log/ folder
                //
                string folder = ConfigurationManagement.Instance.ConfigurationPath;
                folder = folder + Path.DirectorySeparatorChar + "Log";
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                string filepath = folder;

                if (Directory.Exists(filepath))
                {
                    // grab all the logs in the directory
                    string[] files = Directory.GetFiles(filepath);

                    // iterate through them all looking for any that are old (>5)
                    foreach (string s in files)
                    {
                        Match match = Regex.Match(s, @"Event_[0-9]{2}-[0-9]{2}-[0-9]{4}");
                        if (match.Success)
                        {
                            // grab the log date from file path name and 
                            // convert to DateTime for day check                            
                            string logdate = s.Substring(s.LastIndexOf("_") + 1,
                                (s.LastIndexOf(".") - s.LastIndexOf("_")) - 1);

                            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                            dtfi.ShortDatePattern = "MM-dd-yyyy";
                            dtfi.DateSeparator = "-";

                            DateTime logDate = Convert.ToDateTime(logdate, dtfi);

                            // if it's old, get rid of it
                            if ((DateTime.Now - logDate).Days > GeneralConfiguration.Instance.MaxLogs)
                            {
                                if (isFileLocked(new FileInfo(s)))
                                    continue;

                                File.Delete(s);
                            }
                        }

                    }
                }

                //
                // Now clean up the pcap logs folder
                //
                folder = ConfigurationManagement.Instance.ConfigurationPath;
                folder = folder + Path.DirectorySeparatorChar + "pcapLogs";
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                filepath = folder;

                if (Directory.Exists(filepath))
                {
                    string[] files = Directory.GetFiles(filepath);

                    if (files.Length > GeneralConfiguration.Instance.MaxPcapLogs)
                    {
                        // hack off the number of files found in Settings (default should be 25), starting
                        // from the oldest
                        for (int i = 0; i < (files.Length - GeneralConfiguration.Instance.MaxPcapLogs); i++)
                        {
                            // if it's not accessible, skip it for next time
                            if (isFileLocked(new FileInfo(files[i])))
                                continue;

                            File.Delete(files[i]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogCenter.Instance.LogException(e);
            }
        }

        public void LogException(Exception e)
        {
            ExceptionQueue.Enqueue(e);
        }

        public void LogDebugMessage(string message)
        {
            if (GeneralConfiguration.Instance.DeveloperMode)
                DebugQueue.Enqueue(new DebugLogMessage(message));
        }

        public void LogEvent(LogEvent e)
        {
            EventQueue.Enqueue(e);
        }

        #endregion
    }
}
