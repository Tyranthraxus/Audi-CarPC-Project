using System;
using System.Timers;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.Security;
using Microsoft.Win32.Security.Win32Structs;

using CarPCSolutions.LawicelCANUSB;
using CarPCSolutions.SystemTrayApplication.Forms;
using CarPCSolutions.SystemTrayApplication.Classes;
using SteveO.Logging;
using SteveO.RegistryLibrary;


namespace CarPCSolutions.SystemTrayApplication
{ 
    class SysTrayApp
    {
        #region Import User32.dll
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(int hWnd, int Msg, int wParam, int lParam);
        #endregion

        #region Private variables
        private NotifyIcon CarPCSysTrayAppIcon = new NotifyIcon();
        private LogFileWriter _LogFileWriter;
        private LawicelCANUSBAdapter _CANAdapter = null;
        private bool HaveNotifiedAboutNoAdapter = false;
        private long TotalNumberOfMessages = 0;
        #endregion

        #region Button state variables
        private bool _IsPrevTrackDepressed = false;
        private bool _AreRewinding = false;
        private DateTime _TimePrevTrackPressed;
        private bool _IsNextTrackDepressed = false;
        private bool _AreFastForwarding = false;
        private DateTime _TimeNextTrackPressed;
        private bool _IsMMIUpperLeftDepressed = false;
        private bool _IsMMILowerLeftDepressed = false;
        private bool _IsMMIButtonDepressed = false;
        private bool _HaveHeldMMIButtonDown = false;
        private DateTime _TimeMMIButtonPressed;
        private bool _IsMMIButtonTurnedLeft = false;
        private bool _IsMMIButtonTurnedRight = false;
        private bool _IsReturnPressed = false;
        private bool _IsSetupPressed = false;
        #endregion
        
        #region TV enable functionality variables
        private static DateTime _TimeSinceSendingTVEnable = DateTime.Now;
        private static bool _MediaPlayerApplicationIsRunning = false;
        private static DateTime _TimeMCEStarted;
        #endregion
        
        #region MenuItems
        // Define the menu.
        private ContextMenu sysTrayMenu = new ContextMenu();
        private MenuItem exitAppMenuItem = new MenuItem("Exit");
        private MenuItem ConfigMenuitem = new MenuItem("Configuration");
        private MenuItem CloseAdaptersMenuItem = new MenuItem("Close Adapters");
        private MenuItem OpenAdaptersMenuItem = new MenuItem("Open Adapters");
        private ToolStripSeparator seperator = new ToolStripSeparator();
        #endregion
        
        #region Timers
        System.Timers.Timer _UpgradeTimer;
        System.Timers.Timer _AdapterOpener;
        System.Timers.Timer _MessageProcessor;
        #endregion

        ImportUpdateUtility _myImportUtil;

        #region Constructor
        public SysTrayApp(LogFileWriter logfilewriter)
        {
            _LogFileWriter = logfilewriter;

            #region MenuItems
            // Configure the system tray icon.
            CarPCSysTrayAppIcon.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            CarPCSysTrayAppIcon.Text = "AudiCarPC.com Message processor";

            // Attach event handlers.
            exitAppMenuItem.Click += new EventHandler(ExitApp_Click);
            ConfigMenuitem.Click += new EventHandler(Configuration_Click);
            CloseAdaptersMenuItem.Click += new EventHandler(CloseAdapters_Click);
            OpenAdaptersMenuItem.Click += new EventHandler(OpenAdapters_Click);

            // Place the menu items in the menu.
            sysTrayMenu.MenuItems.Add(ConfigMenuitem);
            sysTrayMenu.MenuItems.Add(CloseAdaptersMenuItem);
            sysTrayMenu.MenuItems.Add(OpenAdaptersMenuItem);
            sysTrayMenu.MenuItems.Add(exitAppMenuItem);
            
            CarPCSysTrayAppIcon.ContextMenu = sysTrayMenu;
            #endregion

            _UpgradeTimer = new System.Timers.Timer(250);
            _UpgradeTimer.Enabled = false;
            _UpgradeTimer.Elapsed += new ElapsedEventHandler(CheckForUpgrade);
            
            _AdapterOpener = new System.Timers.Timer(1000);
            _AdapterOpener.Enabled = false;
            _AdapterOpener.Elapsed += new ElapsedEventHandler(AdapterOpener_Tick);
            
            _MessageProcessor = new System.Timers.Timer(RegistrySettings.MessageProcessorInterval);
            _MessageProcessor.Elapsed += new ElapsedEventHandler(MessageProcessor_Tick);

            _myImportUtil = new ImportUpdateUtility(logfilewriter);
            
            // Show the system tray icon.
            this.CarPCSysTrayAppIcon.Visible = true;

            _LogFileWriter.LogInformation("System Tray Application has been initialized.");
            
            _UpgradeTimer.Enabled = false;
            _AdapterOpener.Enabled = true;
            if (RegistrySettings.EnableMediaImport) _myImportUtil.Start();
        }
        #endregion

        #region Menu Item Event handlers
        void Configuration_Click(object sender, EventArgs e)
        {
            ConfigurationPageForm myConfigForm = new ConfigurationPageForm();
            DialogResult dr = myConfigForm.ShowDialog();

            if (dr == DialogResult.OK)
            {
                // Check to see if any changes need to be updated in the program behaviour.
            }
            
        }
        void ExitApp_Click(object sender, System.EventArgs e)
        {
            ExitApp();
        }
        void CloseAdapters_Click(object sender, System.EventArgs e)
        {
            CloseAdapters();
        }
        void OpenAdapters_Click(object sender, System.EventArgs e)
        {
            //OpenAdapters();
        }
        void ViewLog_Click(object sender, System.EventArgs e)
        {
            // Open notepad to view the logfile.
            // System.
            //_LogFileWriter.LogFile
        }
        #endregion

        private void CheckForUpgrade(object sender, ElapsedEventArgs e)
        {
            if (ProgramVariables.ProgramUpdating)
            {
                _LogFileWriter.LogInformation("CheckForUpgrade(): Program is updating, exiting application.");
                _UpgradeTimer.Enabled = false;
                ExitApp();
            }
            else
            {
               //_LogFileWriter.LogInformation("CheckForUpgrade(): Program is not updating.");
            }
        }

        private void AdapterOpener_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                _AdapterOpener.Enabled = false;

                StringBuilder buf = new StringBuilder(32);
                int numadapters = LAWICEL.canusb_getFirstAdapter(buf, 32);
                if (numadapters > 0)
                {
                    
                    
                    _LogFileWriter.LogInformation("Opening CAN adapter; Baud: " + LAWICEL.CAN_BAUD_100K.ToString() + ". Acceptance Code: " + RegistrySettings.CANUSBAcceptanceCode + ". Acceptance Mask: " + RegistrySettings.CANUSBAcceptanceMask + ".");
                    // Just open the first adapter found on the system.
                    _CANAdapter = new LawicelCANUSBAdapter(buf.ToString());
                    _CANAdapter.Open(
                        LAWICEL.CAN_BAUD_100K,
                        Convert.ToUInt32(RegistrySettings.CANUSBAcceptanceCode, 16),
                        Convert.ToUInt32(RegistrySettings.CANUSBAcceptanceMask, 16));
                    #region OpenAdditionalAdapters
                    /*
                     * for (int i = 1; i < numadapters; i++)
                    {
                        if (LAWICEL.canusb_getNextAdapter(buf, 32) > 0)
                        {
                            CANUSBAdapters.Add(new InfotainmentCANAdapter(buf.ToString(), CANUSBMessageReaderDelay, CANUSBFFRWDelay, CANUSBAcceptanceCode, CANUSBAcceptanceMask));
                        }
                    }
                     * */
                    #endregion // Open any additional adapters found..

                    // Check adapter is supported.
                    if (!_CANAdapter.Version.Contains("CARPCSOLN_"))
                    {
                        string ErrorMessage = "Attached LAWICEL CANUSB Adapter with version: " + _CANAdapter.Version + " is not supported.";
                        _LogFileWriter.LogInformation(ErrorMessage);
                        _AdapterOpener.Enabled = true;
                    }
                    else
                    {
                        _LogFileWriter.LogInformation("Adapter: " + _CANAdapter.SerialNumber + ", Version: " + _CANAdapter.Version + " opened successfully.");
                        CarPCSysTrayAppIcon.ShowBalloonTip(5000, "Adapter opened successfully.", "Adapter: " + _CANAdapter.SerialNumber + ", Version: " + _CANAdapter.Version + " opened successfully.", ToolTipIcon.Info);
                        _MessageProcessor.Enabled = true;
                    }
                }
                else
                {
                    _LogFileWriter.LogError("No adapters detected on the system.");
                    if (!HaveNotifiedAboutNoAdapter)
                    {
                        CarPCSysTrayAppIcon.ShowBalloonTip(10000, "Adapter not found.", "No LAWICEL CAN USB adapters found on the system.", ToolTipIcon.Warning);
                        HaveNotifiedAboutNoAdapter = true;
                    }
                    _AdapterOpener.Interval = 5000;
                    _AdapterOpener.Enabled = true;
                }
            }
            
            catch (Exception ex)
            {
                _LogFileWriter.LogError("Error in OpenAdapter: " + ex.Message);
            }
        }
        private void CloseAdapters()
        {
            try
            {
                if (_CANAdapter == null) return;
                _CANAdapter.Close();
                _LogFileWriter.LogInformation("Adapter " + _CANAdapter.SerialNumber + " closed successfully." + System.Environment.NewLine
                    + "Total number of messages processed: " + TotalNumberOfMessages.ToString() + "." + System.Environment.NewLine 
                    + "Messages collected: " + System.Environment.NewLine);
            }
            catch (Exception ex)
            {
                _LogFileWriter.LogError("An error occured closing adapters: " + ex.Message);
            }
        }

        private void MessageProcessor_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                _MessageProcessor.Enabled = false; 
                _LogFileWriter.LogVerbose("MessageProcessor_Tick()");
                
                // If track back/forward has been held down long enough, send appropriate keys to Media Player
                CheckButtonHeldDownFunctions();

                // Send a TV enable message?
                if (CheckToSendTVEnableMessage()) SendEnableTVMsg();

                // Read all messages in the adapter buffer.
                int rv = LAWICEL.ERROR_CANUSB_OK;
                while (rv == LAWICEL.ERROR_CANUSB_OK)
                {
                    // Attempt to read a message
                    LAWICEL.CANMsg msg = new LAWICEL.CANMsg();
                    rv = LAWICEL.canusb_Read(_CANAdapter.Handle, out msg);
                    if (rv == LAWICEL.ERROR_CANUSB_NO_MESSAGE)
                    {
                        _LogFileWriter.LogVerbose("No messages in CANUSB adapter buffer.");
                        break;
                    }
                    else if (rv == LAWICEL.ERROR_CANUSB_OK)
                    {
                        TotalNumberOfMessages++;
                        
                        ParseMessage(msg);
                    }
                    else
                    {
                        throw new Exception("Error in ReadMessage(), rv = " + rv.ToString());
                    }
                }
                // No more work to do as the message buffer is empty.
                _MessageProcessor.Enabled = true;
            }
            catch (Exception ex)
            {
                _LogFileWriter.LogError("An error occured reading messages from the adapter buffer: " + ex.Message + ex.StackTrace);
                _MessageProcessor.Enabled = true;
            }
        }
        private void CheckButtonHeldDownFunctions()
        {
            try
            {
                // Check if track forward/back button are held down
                // If track buttons pressed down for longer than defined timeframe, send FF/rewind keystrokes
                if (DateTime.Compare(DateTime.Now, _TimePrevTrackPressed.AddMilliseconds(RegistrySettings.ButtonHoldDelay)) == 1 && _IsPrevTrackDepressed)
                {
                    // Previous track held down for more than the FF/Rewind delay.
                    /*SendKeyToMCE(VK.CONTROL);
                    SendKeyToMCE(VK.SHIFT);
                    SendKeyToMCE(VK.R);
                    
                    SendKeyToMCE(VK.R, Win32Consts.KEYEVENTF_KEYUP);
                    SendKeyToMCE(VK.SHIFT, Win32Consts.KEYEVENTF_KEYUP);
                    SendKeyToMCE(VK.CONTROL, Win32Consts.KEYEVENTF_KEYUP);

                    AreRewinding = true;*/
                }
                if (DateTime.Compare(DateTime.Now, _TimeNextTrackPressed.AddMilliseconds(RegistrySettings.ButtonHoldDelay)) == 1 && _IsNextTrackDepressed)
                {
                    if (_AreFastForwarding)
                    {
                        //Logging.LogInformation("We are already fast forwarding, doing nothing.");
                        //We are already fast-forwarding, so do nothing
                    }
                    else
                    {
                        // Previous track held down for more than the FF/Rewind delay.
                        SendKeyToWindows(VK.CONTROL, 0);
                        SendKeyToWindows(VK.SHIFT, 0);
                        SendKeyToWindows(VK.F, 0);

                        SendKeyToWindows(VK.F, Win32Consts.KEYEVENTF_KEYUP);
                        SendKeyToWindows(VK.SHIFT, Win32Consts.KEYEVENTF_KEYUP);
                        SendKeyToWindows(VK.CONTROL, Win32Consts.KEYEVENTF_KEYUP);

                        _AreFastForwarding = true;
                    }
                }
                if (DateTime.Compare(DateTime.Now, this._TimeMMIButtonPressed.AddMilliseconds(
                    RegistrySettings.ButtonHoldDelay)) == 1 && this._IsMMIButtonDepressed)
                {
                    if (_HaveHeldMMIButtonDown)
                    {
                        // MMI Button has been held down, and already sent the context button press to KODI.
                    
                    }
                    else
                    {
                        if (RegistrySettings.EnableButtonPressLogging) _LogFileWriter.LogInformation("Sending \"c\" KEYEVENTS, KEYUP/KEYDOWN");
                        SendKeyToWindows(VK.c, 0);
                        SendKeyToWindows(VK.c, Win32Consts.KEYEVENTF_KEYUP);
                        _HaveHeldMMIButtonDown = true;
                    }
                }
                
                //SendKeyToWindows(VK.RETURN, 0);
            }
            catch (Exception ex)
            {
                _LogFileWriter.LogError("An error occured checking the state of track forward/backward button presses: " + ex.Message);
            }
        }
        private void ParseMessage(LAWICEL.CANMsg msg)
        {
            _LogFileWriter.LogVerbose("Message received: " + msg.data.ToString());

            if (msg.len == 6)
            {
                ulong MessageData = msg.data & 0x0000FFFFFFFFFFFF;

                if (MessageData == Audi_CAN_Messages.AUDI_PREVIOUS_TRACK_PRESSED)
                {
                    
                    if (this._IsPrevTrackDepressed == false)
                    {
                        this._IsPrevTrackDepressed = true;
                        if (RegistrySettings.EnableButtonPressLogging) _LogFileWriter.LogInformation("Prev track pressed");
                        _TimePrevTrackPressed = DateTime.Now;
                    }
                }
                else if (MessageData == Audi_CAN_Messages.AUDI_PREVIOUS_TRACK_RELEASED)
                {
                    if (this._IsPrevTrackDepressed == true)
                    {
                        this._IsPrevTrackDepressed = false;
                        if (RegistrySettings.EnableButtonPressLogging) _LogFileWriter.LogInformation("Prev track released");
                        if (_AreRewinding) // Resume normal playback
                        {
                            SendPlayToMCE();
                            _AreRewinding = false;
                        }
                        else // Not rewinding, so we can send a normal prev track button press.
                        {
                            if (RegistrySettings.EnableButtonPressLogging) _LogFileWriter.LogInformation("Sending key events, PREV TRACK KEYDOWN/KEYUP");    
                            SendKeyToWindows(VK.MEDIA_PREV_TRACK, 0);
                            SendKeyToWindows(VK.MEDIA_PREV_TRACK, Win32Consts.KEYEVENTF_KEYUP);
                        }
                    }
                }
                else if (MessageData == Audi_CAN_Messages.AUDI_NEXT_TRACK_PRESSED)
                {
                    if (this._IsNextTrackDepressed == false)
                    {
                        this._IsNextTrackDepressed = true;
                        if (RegistrySettings.EnableButtonPressLogging) _LogFileWriter.LogInformation("Next track button pressed.");
                        _TimeNextTrackPressed = DateTime.Now;
                    }
                }
                else if (MessageData == Audi_CAN_Messages.AUDI_NEXT_TRACK_RELEASED)
                {
                    if (this._IsNextTrackDepressed == true)
                    {
                        this._IsNextTrackDepressed = false;
                        if (RegistrySettings.EnableButtonPressLogging) _LogFileWriter.LogInformation("Next track button released");
                        if (_AreFastForwarding) // Resume normal playback
                        {
                            SendPlayToMCE();
                            _AreFastForwarding = false;
                        }
                        else // Not fast forwarding, so we can send a normal next track button press.
                        {
                            if (RegistrySettings.EnableButtonPressLogging) _LogFileWriter.LogInformation("Sending key events MEDIA NEXT TRACK KEYDOWN/KEYUP");    
                            SendKeyToWindows(VK.MEDIA_NEXT_TRACK, 0);
                            SendKeyToWindows(VK.MEDIA_NEXT_TRACK, Win32Consts.KEYEVENTF_KEYUP);
                        }
                    }
                }
                else if (MessageData == Audi_CAN_Messages.AUDI_MMI_UPPER_LEFT_PRESSED)
                {
                    if (this._IsMMIUpperLeftDepressed == false)
                    {
                        this._IsMMIUpperLeftDepressed = true;
                        if (RegistrySettings.EnableButtonPressLogging) _LogFileWriter.LogInformation("Sending key event UP ARROW KEYUP");
                        SendKeyToWindows(VK.UP, 0);
                    }
                }
                else if (MessageData == Audi_CAN_Messages.AUDI_MMI_UPPER_LEFT_RELEASED)
                {
                    if (this._IsMMIUpperLeftDepressed == true)
                    {
                        this._IsMMIUpperLeftDepressed = false;
                        if (RegistrySettings.EnableButtonPressLogging) _LogFileWriter.LogInformation("Sending key event UP ARROW KEYUP");
                        SendKeyToWindows(VK.UP, Win32Consts.KEYEVENTF_KEYUP);
                    }
                }
                else if (MessageData == Audi_CAN_Messages.AUDI_MMI_LOWER_LEFT_PRESSED)
                {
                    if (this._IsMMILowerLeftDepressed == false)
                    {
                        this._IsMMILowerLeftDepressed = true;
                        if (RegistrySettings.EnableButtonPressLogging) _LogFileWriter.LogInformation("Sending key event DOWN ARROW KEYDOWN");
                        SendKeyToWindows(VK.DOWN, 0);
                    }
                }
                else if (MessageData == Audi_CAN_Messages.AUDI_MMI_LOWER_LEFT_RELEASED)
                {
                    if (this._IsMMILowerLeftDepressed == true)
                    {
                        this._IsMMILowerLeftDepressed = false;
                        if (RegistrySettings.EnableButtonPressLogging) _LogFileWriter.LogInformation("Sending key event DOWN ARROW KEYUP");    
                        SendKeyToWindows(VK.DOWN, Win32Consts.KEYEVENTF_KEYUP);
                    }
                }
                else if (MessageData == Audi_CAN_Messages.AUDI_MMI_SETUP_BUTTON_PRESSED)
                {
                    if (this._IsSetupPressed == false)
                    {
                        this._IsSetupPressed = true;
                        SendKeyToWindows(VK.MEDIA_STOP, 0);
                        if (RegistrySettings.EnableButtonPressLogging) _LogFileWriter.LogInformation("Sending key event MEDIA STOP KEYDOWN");    
                    }
                }
                else if (MessageData == Audi_CAN_Messages.AUDI_MMI_SETUP_BUTTON_RELEASED)
                {
                    if (this._IsSetupPressed == true)
                    {
                        this._IsSetupPressed = false;
                        if (RegistrySettings.EnableButtonPressLogging) _LogFileWriter.LogInformation("Sending key event MEDIA STOP KEYUP");    
                        
                        SendKeyToWindows(VK.MEDIA_STOP, Win32Consts.KEYEVENTF_KEYUP);
                    }
                }
                else if (MessageData == Audi_CAN_Messages.AUDI_MMI_RETURN_PRESSED)
                {
                    if (this._IsReturnPressed == false)
                    {
                        this._IsReturnPressed = true;
                        
                        if (RegistrySettings.EnableButtonPressLogging) _LogFileWriter.LogInformation("Sending key event ESCAPE KEYDOWN");
                        SendKeyToWindows(VK.BACK, 0);
                    }
                }
                else if (MessageData == Audi_CAN_Messages.AUDI_MMI_RETURN_RELEASED)
                {
                    if (this._IsReturnPressed == true)
                    {
                        this._IsReturnPressed = false;
                        if (RegistrySettings.EnableButtonPressLogging) _LogFileWriter.LogInformation("Sending key event ESCAPE KEYUP");    
                        SendKeyToWindows(VK.BACK, Win32Consts.KEYEVENTF_KEYUP);
                    }
                }
                else if (MessageData == Audi_CAN_Messages.AUDI_MMI_CENTER_BUTTON_PRESSED)
                {
                    if (this._IsMMIButtonDepressed == false)
                    {
                        this._IsMMIButtonDepressed = true;
                        this._TimeMMIButtonPressed = DateTime.Now;
                        if (RegistrySettings.EnableButtonPressLogging) _LogFileWriter.LogInformation("MMI Center button pressed");
                        
                    }
                }
                else if (MessageData == Audi_CAN_Messages.AUDI_MMI_CENTER_BUTTON_RELEASED)
                {
                    _LogFileWriter.LogVerbose("Received center button release message.");
                    if (this._IsMMIButtonDepressed == true)
                    {
                        _LogFileWriter.LogVerbose("Center button recorded as pressed down, setting to pressed down off.");
                        this._IsMMIButtonDepressed = false;
                        
                        if (_HaveHeldMMIButtonDown)
                        {
                            _LogFileWriter.LogVerbose("MMI button is marked as held down, turning off.");
                            _HaveHeldMMIButtonDown = false;
                            // Do nothing as we have already sent the context button press
                        }
                        else
                        {
                            // Just a normal press of the MMI Button
                            if (RegistrySettings.EnableButtonPressLogging) _LogFileWriter.LogInformation("Sending key events RETURN KEYDOWN/KEYUP");
                            SendKeyToWindows(VK.RETURN, 0);
                            SendKeyToWindows(VK.RETURN, Win32Consts.KEYEVENTF_KEYUP);
                        }
                    }
                }
                else if (MessageData == Audi_CAN_Messages.AUDI_MMI_BUTTON_WHEEL_LEFT_BEGIN)
                {
                    if (this._IsMMIButtonTurnedLeft == false)
                    {
                        this._IsMMIButtonTurnedLeft = true;
                        if (RegistrySettings.EnableButtonPressLogging) _LogFileWriter.LogInformation("Sending key event LEFT ARROW KEYDOWN");
                        SendKeyToWindows(VK.LEFT, 0);
                    }
                }
                else if (MessageData == Audi_CAN_Messages.AUDI_MMI_BUTTON_WHEEL_LEFT_RELEASE)
                {
                    if (this._IsMMIButtonTurnedLeft == true)
                    {
                        this._IsMMIButtonTurnedLeft = false;
                        if (RegistrySettings.EnableButtonPressLogging) _LogFileWriter.LogInformation("Sending key event LEFT ARROW KEYUP");
                            
                        SendKeyToWindows(VK.LEFT, Win32Consts.KEYEVENTF_KEYUP);
                    }
                }
                else if (MessageData == Audi_CAN_Messages.AUDI_MMI_BUTTON_WHEEL_RIGHT_BEGIN)
                {
                    if (this._IsMMIButtonTurnedRight == false)
                    {
                        this._IsMMIButtonTurnedRight = true;
                        if (RegistrySettings.EnableButtonPressLogging) _LogFileWriter.LogInformation("Sending key event RIGHT ARROW KEYDOWN");
                        SendKeyToWindows(VK.RIGHT, 0);
                    }
                }
                else if (MessageData == Audi_CAN_Messages.AUDI_MMI_BUTTON_WHEEL_RIGHT_RELEASE)
                {
                    if (this._IsMMIButtonTurnedRight == true)
                    {
                        this._IsMMIButtonTurnedRight = false;
                        if (RegistrySettings.EnableButtonPressLogging) _LogFileWriter.LogInformation("Sending key event RIGHT ARROW KEYUP");
                        SendKeyToWindows(VK.RIGHT, Win32Consts.KEYEVENTF_KEYUP);
                    }
                }
            }
        }
        /// <summary>
        /// Checks if the software should send the CAN message that enables TV input on the RNS-E.
        /// </summary>
        /// <returns>True if it should be sent, False otherwise</returns>
        private bool CheckToSendTVEnableMessage()
        {
            bool ShouldSendTVEnableMessage = false;

            // If we are not waiting for the Media Player application to start-up, we send the TV Enable Msg.
            if (RegistrySettings.AlwaysSendTVEnable)
            {
                _LogFileWriter.LogVerbose("Registry key to Enable TV input without detecting Media Player is enabled.");
                ShouldSendTVEnableMessage = true;
            }

            else // Otherwise check if Media Player is running
            {
                if (!_MediaPlayerApplicationIsRunning) // If it is now running, record the time we first detected it.
                {

                    if (Windows.MediaPlayerApplicationHasFocus())
                    {
                        _LogFileWriter.LogVerbose("Media Player application detected.");
                        _MediaPlayerApplicationIsRunning = true;
                        _TimeMCEStarted = DateTime.Now;
                    }
                }

                // Now check if it is running, if it running, and has been running long enough to send a TV enable message, we send one.
                if (_MediaPlayerApplicationIsRunning) 
                {
                    if (DateTime.Compare(DateTime.Now, _TimeMCEStarted.AddMilliseconds(RegistrySettings.DelayBeforeSendingTVEnableOnStartup)) > 0)
                    {
                        _LogFileWriter.LogVerbose("Media Player application has started and the startup delay passed.");
                        ShouldSendTVEnableMessage = true;
                    }
                }
                else
                {
                    _LogFileWriter.LogVerbose("Media Application not detected. TV Enable will not be sent until the media application is detected, or the key to AlwaysSendTVEnableMsg is set to true.");
                }
            }

            // Finally if we have sent a TV enable message recently, we don't send another one.
            if (DateTime.Compare(DateTime.Now, _TimeSinceSendingTVEnable.AddMilliseconds(RegistrySettings.DelayBetweenTVEnableMessages)) < 0)
            {
                _LogFileWriter.LogVerbose("TV Enable message not being sent, one has been sent recently already.");
                ShouldSendTVEnableMessage = false;
            }
            
            return ShouldSendTVEnableMessage;
        }
        private void SendEnableTVMsg()
        {
            try
            {
                _LogFileWriter.LogVerbose("Sending a TV enable message.");
                // Send one message with extended id
                LAWICEL.CANMsg msg = new LAWICEL.CANMsg();
                msg.id = 0x602;
                msg.len = 8;
                //msg.flags = LAWICEL.CANUSB.CANMSG_EXTENDED;
                msg.data = 0x8877665544332211;

                _CANAdapter.SendMessage(msg);
                _TimeSinceSendingTVEnable = DateTime.Now;
            }
            catch (Exception ex)
            {
                _LogFileWriter.LogInformation("An error occured sending the TV enable message: " + ex.Message);
            }

        }
        unsafe private void SendKeyToWindows(VK key, uint flags)
        {
            // Setup the keypress to send.
            INPUT input = new INPUT();
            input.type = Win32Consts.INPUT_KEYBOARD;
            // Key down shift, ctrl, and/or alt
            input.ki.wScan = 0;
            input.ki.time = 0;
            input.ki.dwFlags = flags;
            input.ki.dwExtraInfo = Win32.GetMessageExtraInfo();

            input.ki.wVk = (ushort)key;

            bool IsMCEActive = false;
            try
            {
                IsMCEActive = Windows.MediaPlayerApplicationHasFocus();
            }
            catch (Exception ex)
            {
                _LogFileWriter.LogError("An exception was caught checking if MCE is active. " + ex.Message);
            }

            if (IsMCEActive == false)
            {
                _LogFileWriter.LogVerbose("Not sending keystroke:" + key.ToString() + ", MCE does not have focus");
                return; // Don't send any key presses to Windows if MCE is not active.
            }
            else Win32.SendInput(1, ref input, (UInt32)sizeof(INPUT));
            //else Win32.SendInput(1, ref input, (UInt32)System.Runtime.InteropServices.Marshal.SizeOf(INPUT));

        }
        private void SendPlayToMCE()
        {
            // Send play command
            SendKeyToWindows(VK.CONTROL, 0);
            SendKeyToWindows(VK.SHIFT, 0);
            SendKeyToWindows(VK.P, 0);

            SendKeyToWindows(VK.P, Win32Consts.KEYEVENTF_KEYUP);
            SendKeyToWindows(VK.SHIFT, Win32Consts.KEYEVENTF_KEYUP);
            SendKeyToWindows(VK.CONTROL, Win32Consts.KEYEVENTF_KEYUP);
        }
        private void ExitApp()
        {
            _MessageProcessor.Enabled = false;
            _AdapterOpener.Enabled = false;
            
            CloseAdapters();

            CarPCSysTrayAppIcon.Visible = false;
            CarPCSysTrayAppIcon.Icon = null;
            CarPCSysTrayAppIcon.Dispose();

            _myImportUtil.Stop();

            Application.Exit();

            // Old code from Windows form dispose().
            /* if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);*/
        }
        private void showBalloon(string title, string body)
        {
            NotifyIcon notifyIcon = new NotifyIcon();
            notifyIcon.Visible = true;

            if (title != null)
            {
                notifyIcon.BalloonTipTitle = title;
            }

            if (body != null)
            {
                notifyIcon.BalloonTipText = body;
            }
            notifyIcon.Icon = SystemIcons.Application;
            notifyIcon.ShowBalloonTip(30000);
        }
    }
}
        
