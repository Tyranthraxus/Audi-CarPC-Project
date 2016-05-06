using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;

using System.Windows.Forms;
using SteveO.Logging;

namespace CarPCSolutions.SystemTrayApplication
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Set the working folder of the application in the ProgramVariables static class.
            FileInfo ExecutingAssembly = new FileInfo(Assembly.GetExecutingAssembly().Location);
            ProgramVariables.WorkingDirectory = ExecutingAssembly.Directory;

            SysTrayApp mySysTrayApp;

            try
            {
                // Create the LogFile folder if it does not exist.
                Directory.CreateDirectory(Environment.ExpandEnvironmentVariables(@"%LOCALAPPDATA%") + "\\CarPC Solutions");
            
                string LogFileName = Environment.ExpandEnvironmentVariables(@"%LOCALAPPDATA%") + "\\CarPC Solutions\\CarPC SystemTray Log.txt";
                
                LogFileWriter logwriter = new LogFileWriter(LogFileName);
                logwriter.MaximumLogFileSize = RegistrySettings.MaxLogFileSizeKb;

                logwriter.LogInformation("*******************************************************");
                logwriter.LogInformation("AudiCarPC.com RNS-E Message Processor program starting.");
                logwriter.LogInformation("*******************************************************");

                // Check how many processes are already running.
            
                mySysTrayApp = new SysTrayApp(logwriter);
                
                Application.Run();

                logwriter.LogInformation("AudiCarPC.com RNS-E Message Processor program terminating." + System.Environment.NewLine);
            }
            catch (Exception ex)
            {
                string ErrorMessage = "Could not start program. Error: " + ex.Message + ". " + ex.GetType().ToString() + " exception caught at: " + ex.StackTrace;
                //logwriter.LogError(ErrorMessage);
                MessageBox.Show(ErrorMessage);

                return;
            }
        }
    }
}
