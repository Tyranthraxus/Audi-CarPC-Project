using System;
using System.Collections.Generic;
using System.Text;

namespace CarPCSolutions.LawicelCANUSB
{
    public class LawicelCANUSBAdapter
    {
        // Private Fields
        private string _SerialNumber;
        private string _Version;
        private uint _Handle;
        // private Thread _MessageProcessor;
        // private bool _Enabled;
        private List<CANMsgStats> p_MessageCounts = new List<CANMsgStats>();
        private bool _Open = false;
        
        // Public Accessors
        public string SerialNumber
        {
            get
            {
                return this._SerialNumber;
            }
        }
        public string Version
        {
            get
            {
                return this._Version;
            }
        }
        public uint Handle
        {
            get
            {
                return this._Handle;
            }
        }
        
        // Constructor
        public LawicelCANUSBAdapter(string SerialNumber)
        {
            this._SerialNumber = SerialNumber;
        }

        // Methods
        public void Open(string BitRate, uint AcceptanceCode, uint AcceptanceMask)
        {
            try
            {
                this._Handle = LAWICEL.canusb_Open(_SerialNumber, BitRate, AcceptanceCode, AcceptanceMask, LAWICEL.CANUSB_FLAG_QUEUE_REPLACE);

                /*while (Handle < 1) // if (Handle > 0)
                {
                    // Can reach here after the system has just started..
                    LogFileWriter.LogWarning("Failed to Open CANUSB: " + _SerialNumber + ". canusb_Open returned: " + Handle.ToString());

                    Handle = LAWICEL.canusb_Open(_SerialNumber, CANUSBBaudRate, CANUSBAcceptanceCode, CANUSBAcceptanceMask, LAWICEL.CANUSB_FLAG_QUEUE_REPLACE);

                    Thread.Sleep(200);
                }*/

                // Test the adapter opened successfully.
                GetVersionInfo();

                _Open = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void GetVersionInfo()
        {
            StringBuilder buf = new StringBuilder(256);
            int rv = LAWICEL.canusb_VersionInfo(_Handle, buf);
            if (rv == LAWICEL.ERROR_CANUSB_OK)
            {
                this._Version = buf.ToString();
            }
            else
            {
                throw new Exception("Could not retrieve version info from adapter.");
            }
        }
        public void Close()
        {
            if (!_Open) return;

            int res = LAWICEL.canusb_Close(_Handle);
            if (res == LAWICEL.ERROR_CANUSB_OK) return;
            else
            {
                throw new Exception("Could not close adapter: " + _SerialNumber + ", return code from canusb_Close: " + res.ToString());
            }
        }
        public void SendMessage(LAWICEL.CANMsg msg)
        {
            int rv = LAWICEL.canusb_Write(_Handle, ref msg);

            if (rv == LAWICEL.ERROR_CANUSB_OK)
            {
                
            }
            else if (rv == LAWICEL.ERROR_CANUSB_TX_FIFO_FULL)
            {
                throw new Exception("FIFO Full. Can't send message.");
            }
            else
            {
                throw new Exception("Failed to send message.");
            }
        }
    }
}
