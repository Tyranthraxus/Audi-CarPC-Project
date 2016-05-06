using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;
using SteveO.Logging;

namespace CarPCSolutions.SystemTrayApplication
{
    public static class Windows
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        public static string DisplayActiveWindowName()
        {
            int chars = 256;
            StringBuilder buff = new StringBuilder(chars);

            // Obtain the handle of the active window.
            IntPtr handle = GetForegroundWindow();

            // Update the controls.
            if (GetWindowText(handle, buff, chars) > 0)
            {
                return (buff.ToString());
                //MessageBox.Show(handle.ToString());
            }
            throw new Exception("Could not get Active Window name");
        }
        public static bool MediaPlayerApplicationHasFocus()
        {
            string ActiveWindowName;
            try
            {
                ActiveWindowName = Windows.DisplayActiveWindowName();
            }
            catch (Exception)
            {
                // Unknown error - could be application exiting. return false.
                return false;
            }

            if (ActiveWindowName.Contains("Media Center") || 
                ActiveWindowName.Contains("XBMC") || 
                ActiveWindowName.Contains("Kodi") ||
                ActiveWindowName.Contains("Notepad"))
            return true;
            else
            {
                return false;
            }
        }
    }
}
