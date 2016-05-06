using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;

using SteveO.Logging;

namespace CarPCSolutions.SystemTrayApplication
{
    /// <summary>
    /// Defines a class that monitors local drives and imports media or updates the application if new files are found.
    /// </summary>
    public class ImportUpdateUtility
    {
        private Thread _WorkerThread;
        private DriveInfo[] _CurrentDrives;
        private LogFileWriter _LogFileWriter;
        private bool _Enabled;

        public ImportUpdateUtility(LogFileWriter writer)
        {
            _CurrentDrives = DriveInfo.GetDrives();
            _WorkerThread = new Thread(new ThreadStart(Run));
            _LogFileWriter = writer;
            _Enabled = true;
        }

        public void Start()
        {
            _WorkerThread.Start();
        }
        public void Stop()
        {
            this._Enabled = false;
        }

        private void Run()
        {
            try
            {

                while (true)
                {

                    if (!_Enabled)
                    {
                        _LogFileWriter.LogInformation("Import thread is terminating.");
                        return;
                    }

                    Thread.Sleep(1000);

                    DirectoryInfo CarPCVideosFolder = new DirectoryInfo("C:\\Users\\CarPC\\Videos\\Car Music Videos");
                    DirectoryInfo CarPCMusicFolder = new DirectoryInfo("C:\\Users\\CarPC\\Music");

                    
                    //PrintDriveArrayContents(CurrentDrives);
                    //PrintDriveArrayContents(DriveInfo.GetDrives());

                    DriveInfo[] ExistingDrives = DriveInfo.GetDrives();
                    if (DriveArrayContainsNewDrives(_CurrentDrives, ExistingDrives))
                    {
                        _LogFileWriter.LogInformation("New Drive detected..");
                        Thread.Sleep(1000);

                        // If the system has less than 100Mb free, do nothing.
                        try
                        {
                            DriveInfo CDrive = new DriveInfo("C:\\");
                            if (CDrive.TotalFreeSpace < 1024000)
                            {
                                _LogFileWriter.LogInformation("There is too little space on the system drive to import files or update the program.");
                                continue;
                            }
                        }
                        catch (Exception ex)
                        {
                            _LogFileWriter.LogError("An error occured checking if the local C Drive has enough space to import files. " + ex.Message);


                            // Just exit out the main loop and try again after the normal delay.
                            continue;
                        }

                        // Copy whatever files are found on the drive into the movie / music folders.

                        foreach (DriveInfo d in DriveInfo.GetDrives())
                        {
                            // Exclude the C Drive..
                            if (d.Name == "C:\\")
                            {
                                //Console.WriteLine("Skipping drive C:");
                                continue;
                            }

                            //Console.WriteLine("Scanning drive: " + d.Name);

                            try
                            {

                                DirectoryInfo NewDriveRoot = new DirectoryInfo(d.Name);
                                _LogFileWriter.LogInformation("Copying files from " + NewDriveRoot.Name + " to system drive.");

                                // Check that the destination folders exist before running the imports.
                                if (CarPCMusicFolder.Exists)
                                {
                                    CopyMusic(d, CarPCMusicFolder);
                                }
                                if (CarPCVideosFolder.Exists)
                                {
                                    CopyMovieFiles(NewDriveRoot, CarPCVideosFolder, "*.flv");
                                    CopyMovieFiles(NewDriveRoot, CarPCVideosFolder, "*.mp4");
                                }
                                
                                CheckForProgramUpdate(d);

                                continue;
                            }
                            catch (System.IO.IOException)
                            {
                                continue;
                                // Assume that this is a CD drive and ignore..
                            }
                        }
                    }
                    else
                    {
                        //_LogFileWriter.LogInformation("No new drives detected.");
                    }

                    // Save the current state of the drives.
                    _CurrentDrives = ExistingDrives;

                    // Check if there are any files to copy.

                    // Sleep for some time..
                    Thread.Sleep(1000);

                    // Start over... 
                }
            }
            catch (Exception ex)
            {
                _LogFileWriter.LogError("An error occured in the Media Import thread: " + ex.Message);
            }
        }
        private void CopyMusic(DriveInfo SourceDrive, DirectoryInfo Destination)
        {
            DirectoryInfo MusicSubFolder = new DirectoryInfo(SourceDrive.Name + "CarPC Music");

            
            if (!MusicSubFolder.Exists)
            {
                _LogFileWriter.LogWarning("The folder \"CarPC Music\" does not exist on drive " + SourceDrive.Name + ".");
                return;
            }

            else
            {
                // Copy the contents of the subfolder into the Music folder.
                string CommandLine = "robocopy " + "\"" + MusicSubFolder.FullName + "\"" + " " + Destination.FullName + " /e";
                // Console.WriteLine("Command line: " + CommandLine);

                ProcessStartInfo si = new ProcessStartInfo("cmd.exe", " /c " + CommandLine);
                si.WindowStyle = ProcessWindowStyle.Hidden;
                Process p = new Process();
                p.StartInfo = si;
                p.Start();
            }


        }
        private void CopyMovieFiles(DirectoryInfo SourceDirectory, DirectoryInfo DestinationDirectory, string SearchPattern)
        {
            // Search the root folder for .mp4 and .flv files..
            foreach (FileInfo file in SourceDirectory.GetFiles(SearchPattern))
            {
                //Console.WriteLine("Found file: " + file.Name);
                //Console.WriteLine(file.FullName);
                //Console.WriteLine(CarPCVideosFolder.FullName);
                //Console.WriteLine(file.DirectoryName);
                //Console.WriteLine();

                _LogFileWriter.LogInformation("Copying file: " + file.Name + " to Music Videos folder.");

                try
                {
                    file.CopyTo(file.FullName.Replace(file.DirectoryName, DestinationDirectory.FullName + "\\"));
                }
                catch (Exception ex)
                {
                    _LogFileWriter.LogError("Copy of file: " + file.Name + " failed, " + ex.Message);
                    continue;
                }

                // Now proceed to the next folder.
                continue;
            }
        }
        private void CheckForProgramUpdate(DriveInfo SourceDrive)
        {
            try
            {

                FileInfo ProgramUpdate = new FileInfo(SourceDrive.Name + "RNS Message Processor.exe");

                if (ProgramUpdate.Exists)
                {
                    _LogFileWriter.LogInformation("A potential program update was found on the inserted drive. It will be used to replace the running program.");


                    // Create a temporary file that can be used to shutdown the running program, and replace the installed binary.
                    string BatchFileContent = @"
cd %TEMP%

REM Create a vb script file that can be used to sleep the batch file.
echo WScript.Sleep WScript.Arguments.Item(0) > Sleep.vbs

REM Sleep for 5 seconds
cscript Sleep.vbs 5000

REM Attempt to copy the file
move /Y " + "\"" + SourceDrive.Name + "RNS Message Processor.exe\" " + "\"" + ProgramVariables.WorkingDirectory + "\"" + @"

REM Launch the new program
" + "\"" + ProgramVariables.WorkingDirectory + "\\RNS Message Processor.exe\"" + @"

REM Restart the PC
REM shutdown /r /t 0



";
                    FileInfo BatchFile = new FileInfo(ProgramVariables.WorkingDirectory.FullName + "\\Upgrade.bat");

                    // This text is added only once to the file. 
                    if (!File.Exists(BatchFile.FullName))
                    {
                        // Create a file to write to.
                        StreamWriter sw = File.CreateText(BatchFile.FullName);
                        sw.Close();
                    }
                    else
                    {
                        // Delete any existing file.
                        BatchFile.Delete();
                    }

                    using (StreamWriter sw = File.AppendText(BatchFile.FullName))
                    {
                        sw.WriteLine(BatchFileContent);
                    }

                    ProcessStartInfo si = new ProcessStartInfo("cmd.exe", " /c " + "\"" + BatchFile.FullName + "\"");
                    si.WindowStyle = ProcessWindowStyle.Hidden;
                    Process p = new Process();
                    p.StartInfo = si;
                    p.Start();
                    _LogFileWriter.LogInformation("Upgrade batch file has been started.");

                    // Send a signal back to the main form to terminate.
                    ProgramVariables.ProgramUpdating = true;

                }
            }
            catch (Exception ex)
            {
                _LogFileWriter.LogError("An error occured checking for a program update: " + ex.Message);
                    
            }
        }
        /// <summary>
        /// Compares the two arrays.
        /// </summary>
        /// <param name="Array1"></param>
        /// <param name="Array2"></param>
        /// <returns>True, if the second array contains a member that is not present in the first.</returns>
        public static bool DriveArrayContainsNewDrives(DriveInfo[] Array1, DriveInfo[] Array2)
        {
            foreach (DriveInfo d in Array2)
            {
                // If there are any drives in the second array that are not in the first, we return true.
                if (!DriveArrayContainsDrive(Array1, d))
                {
                    //Console.WriteLine("Drive: " + d.Name
                    return true;
                }
            }
            return false;
        }
        public static bool DriveArrayContainsDrive(DriveInfo[] Array, DriveInfo drive)
        {
            foreach (DriveInfo d in Array)
            {
                if (drive.Name == d.Name) return true;
            }

            return false;
        }
        public static void PrintDriveArrayContents(DriveInfo[] Array)
        {
            //_LogFileWriter.LogInformation("Drive array contains: ");
            foreach (DriveInfo d in Array)
            {
               // _LogFileWriter.LogInformation(d.Name + ", ");
            }
            //_LogFileWriter.LogInformation("");
        }
    }

   
}
