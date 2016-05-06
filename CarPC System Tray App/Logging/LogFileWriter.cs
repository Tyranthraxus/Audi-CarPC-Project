using System;
//using System.Collections.Generic;
//using System.Text;
using System.IO;

using CarPCSolutions.SystemTrayApplication;
using System.Threading;

namespace SteveO.Logging
{
    public class LogFileWriter
    {
        private FileInfo _LogFile;
        private int _MaximumLogFileSize;
        /// <summary>
        /// A locking object to prevent two different threads accessing the logfile at the same time.
        /// </summary>
        private static object LoggingLockObject = new object();
        public int MaximumLogFileSize
        {
            get
            {
                return this._MaximumLogFileSize;
            }
            set
            {
                this._MaximumLogFileSize = value;
            }
        }
        public FileInfo LogFile
        {
            get
            {
                return this._LogFile;
            }
        }
        public LogFileWriter(string FileName)
        {
            _LogFile = new FileInfo(FileName);
        }
        public void LogWarning(string Text)
        {
            LogEvent(Text, LogEntryType.Warning);
        }
        public void LogVerbose(string Text)
        {
            if (RegistrySettings.EnableVerboseLogging) LogEvent(Text, LogEntryType.Verbose);
        }
        public void LogError(string Text)
        {
            LogEvent(Text, LogEntryType.Error);
        }
        public void LogInformation(string Text)
        {
            LogEvent(Text, LogEntryType.Information);
        }
        public void LogEvent(string Text, LogEntryType type)
        {
            Text = DateTime.Now.ToString() + " " + DateTime.Now.Millisecond.ToString() + ": " + Text;

            lock (LoggingLockObject)
            {
                if (!File.Exists(_LogFile.FullName))
                {
                    CreateLogFile(_LogFile.FullName);
                }

                if (_LogFile.Length > (_MaximumLogFileSize * 1024))
                {
                    try
                    {
                        DeleteLogFile(_LogFile);
                    }
                    catch (Exception)
                    {
                        Thread.Sleep(100); 
                    }
                    CreateLogFile(_LogFile.FullName);
                }

                using (StreamWriter sw = File.AppendText(_LogFile.FullName))
                {
                    switch (type)
                    {
                        case LogEntryType.Information:
                            sw.WriteLine(Text);
                            break;
                        case LogEntryType.Warning:
                            sw.WriteLine("Warning: " + Text);
                            break;
                        case LogEntryType.Error:
                            sw.WriteLine("Error: " + Text);
                            break;
                        case LogEntryType.Verbose:
                            sw.WriteLine("Debug: " + Text);
                            break;
                    }
                }
            }
        }
        public static void LogEvent(string Text, LogEntryType type, FileInfo LogFile, int MaxLogFileSize)
        {
            

            lock (LoggingLockObject)
            {
                if (!File.Exists(LogFile.FullName))
                {
                    CreateLogFile(LogFile.FullName);
                }

                if (LogFile.Length > (MaxLogFileSize * 1024))
                {
                    DeleteLogFile(LogFile);
                    CreateLogFile(LogFile.FullName);
                }

                using (StreamWriter sw = File.AppendText(LogFile.FullName))
                {
                    switch (type)
                    {
                        case LogEntryType.Information:
                            sw.WriteLine(Text);
                            break;
                        case LogEntryType.Warning:
                            sw.WriteLine("Warning: " + Text);
                            break;
                        case LogEntryType.Error:
                            sw.WriteLine("Error: " + Text);
                            break;
                    }
                    sw.Close();
                }
            }
        }
        private static void CreateLogFile(string FileName)
        {
            StreamWriter sw2 = File.CreateText(FileName);
            sw2.Close();
        }
        private static void DeleteLogFile(FileInfo LogFile)
        {
            try
            {

                LogFile.Delete();
            }
            catch (Exception)
            {
                Thread.Sleep(100);
            }
        }
    }
}
