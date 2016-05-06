using System;
using System.Collections.Generic;
using System.Text;

namespace CarPCSolutions.LawicelCANUSB
{
    public class CANMsgStats
    {
        private uint p_ID;
        private int p_count;
        private bool p_IsExtended;

        public uint ID
        {
            get
            {
                return this.p_ID;
            }
        }
        public int count
        {
            get
            {
                return this.p_count;
            }
        }
        public bool IsExtended
        {
            get
            {
                return this.p_IsExtended;
            }
        }

        public CANMsgStats(uint ID, int count, bool IsExtended)
        {
            this.p_ID = ID;
            this.p_count = count;
            this.p_IsExtended = IsExtended;
        }
        public void Increment()
        {
            this.p_count++;
        }
    }
}
