using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace CarPCSolutions.SystemTrayApplication
{
    class ProgramVariables
    {
        public static bool ProgramTerminating = false;
        public static bool ProgramUpdating = false;
        
        
        public static DirectoryInfo WorkingDirectory;

        //public static bool LogCANErrors = false;
        //public static bool LoggingEnabled = true;
        public static object LoggingLockObject = new object();
    }
}
