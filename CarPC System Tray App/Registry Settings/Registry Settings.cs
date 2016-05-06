using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SteveO.RegistryLibrary;

namespace CarPCSolutions.SystemTrayApplication
{
    public static class RegistryLocations
    {
        public static RegistryLocation GeneralSettings = new RegistryLocation(RegistryHive.CurrentUser, "Software\\Car PC Solutions\\RNSMessageProcessor");
    }
    public static class RegistrySettings
    {
        /// <summary>
        /// Amount of time in msec to wait for track forward/rewind to be held down to send fast forward/rewind commands.
        /// </summary>
        public static int ButtonHoldDelay
        {
            get
            {
                return RegistryLibrary.GetInt32(RegistryLocations.GeneralSettings, "ButtonHoldDelay", 300);
            }
            set
            {
                RegistryLibrary.SetValue(RegistryLocations.GeneralSettings, "FFRWDelay", value);
            }
        }
        /// <summary>
        /// This value determines whether we start sending TV module present messages on the CAN immediately or if we wait for the Media Player application to be detected.
        /// </summary>
        public static bool AlwaysSendTVEnable
        {
            get
            {
                return RegistryLibrary.GetBooleanValue(RegistryLocations.GeneralSettings, "AlwaysSendTVEnableMsg", false);
            }
            set
            {
                RegistryLibrary.SetValue(RegistryLocations.GeneralSettings, "AlwaysSendTVEnableMsg", value);
            }
        }
        /// <summary>
        /// The amount of time we wait after the application has started before we start sending TV module present messages on the CAN Bus.
        /// This will stop the screen from showing the Windows desktop, or the Media Center splash screen while the application / Windows is still loading.
        /// </summary>
        public static int DelayBetweenTVEnableMessages
        {
            get
            {
                return RegistryLibrary.GetInt32(RegistryLocations.GeneralSettings, "DelayBetweenTVEnableMessages", 100);
            }
            set
            {
                RegistryLibrary.SetValue(RegistryLocations.GeneralSettings, "DelayBetweenTVEnableMessages", value);
            }
        }
        public static int DelayBeforeSendingTVEnableOnStartup
        {
            get
            {
                return RegistryLibrary.GetInt32(RegistryLocations.GeneralSettings, "DelayBeforeSendingTVEnableMsgOnStartup", 500);
            }
            set
            {
                RegistryLibrary.SetValue(RegistryLocations.GeneralSettings, "DelayBeforeSendingTVEnableMsgOnStartup", value);
            }
        }

        public static bool EnableMediaImport
        {
            get
            {
                return RegistryLibrary.GetBooleanValue(RegistryLocations.GeneralSettings, "Enable Media Import", false);
            }
            set
            {
                RegistryLibrary.SetValue(RegistryLocations.GeneralSettings, "Enable Media Import", value);
            }
        }
        
        public static bool EnableButtonPressLogging
        {
            get
            {
                return RegistryLibrary.GetBooleanValue(RegistryLocations.GeneralSettings, "EnableButtonPressLogging", false);
            }
            set
            {
                RegistryLibrary.SetValue(RegistryLocations.GeneralSettings, "EnableButtonPressLogging", value);
            }
        }
        public static bool EnableVerboseLogging
        {
            get
            {
                return RegistryLibrary.GetBooleanValue(RegistryLocations.GeneralSettings, "Enable Verbose Logging", false);
            }
            set
            {
                RegistryLibrary.SetValue(RegistryLocations.GeneralSettings, "Enable Verbose Logging", value);
            }
        }
        public static int MaxLogFileSizeKb
        {
            get
            {
                return RegistryLibrary.GetInt32(RegistryLocations.GeneralSettings, "Maximum Logfile Size (kb)", 20000);
            }
            set
            {
                RegistryLibrary.SetValue(RegistryLocations.GeneralSettings, "Maximum Logfile Size (kb)", value);
            }
        }
        public static string CANUSBAcceptanceCode
        {
            get
            {
                return RegistryLibrary.GetStringValue(RegistryLocations.GeneralSettings, "LAWICEL CANUSB Acceptance Code", LAWICEL.CANUSB_ACCEPTANCE_CODE_TV.ToString("X"));
            }
            set
            {
                RegistryLibrary.SetValue(RegistryLocations.GeneralSettings, "LAWICEL CANUSB Acceptance Code", value);
            }
        }
        public static string CANUSBAcceptanceMask
        {
            get
            {
                return RegistryLibrary.GetStringValue(RegistryLocations.GeneralSettings, "LAWICEL CANUSB Acceptance Mask", LAWICEL.CANUSB_ACCEPTANCE_MASK_TV.ToString("X"));
            }
            set
            {
                RegistryLibrary.SetValue(RegistryLocations.GeneralSettings, "LAWICEL CANUSB Acceptance Mask", value);
            }
        }
        public static int MessageProcessorInterval
        {
            get
            {
                return RegistryLibrary.GetInt32(RegistryLocations.GeneralSettings, "Message Processor Interval (msec)", 25);
            }
            set
            {
                RegistryLibrary.SetValue(RegistryLocations.GeneralSettings, "Message Processor Interval (msec)", value);
            }
        }

    }
}
