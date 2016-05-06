using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarPCSolutions.SystemTrayApplication.Classes
{
    class Audi_CAN_Messages
    {
        public const ulong AUDI_PREVIOUS_TRACK_RELEASED = 0x000001013037;
        public const ulong AUDI_PREVIOUS_TRACK_PRESSED = 0x000001043037;
        public const ulong AUDI_NEXT_TRACK_PRESSED = 0x000002013037;
        public const ulong AUDI_NEXT_TRACK_RELEASED = 0x000002043037;
        public const ulong AUDI_MMI_CENTER_BUTTON_PRESSED = 0x001000013037;
        public const ulong AUDI_MMI_CENTER_BUTTON_RELEASED = 0x001000043037;
        public const ulong AUDI_MMI_UPPER_LEFT_PRESSED = 0x000040013037;
        public const ulong AUDI_MMI_UPPER_LEFT_RELEASED = 0x000040043037;
        public const ulong AUDI_MMI_LOWER_LEFT_PRESSED = 0x000080013037;
        public const ulong AUDI_MMI_LOWER_LEFT_RELEASED = 0x000080043037;
        public const ulong AUDI_MMI_SETUP_BUTTON_PRESSED = 0x000100013037;
        public const ulong AUDI_MMI_SETUP_BUTTON_RELEASED = 0x000100043037;
        public const ulong AUDI_MMI_RETURN_PRESSED = 0x000200013037;
        public const ulong AUDI_MMI_RETURN_RELEASED = 0x000200043037;
        public const ulong AUDI_MMI_BUTTON_WHEEL_RIGHT_BEGIN = 0x012000013037;
        public const ulong AUDI_MMI_BUTTON_WHEEL_RIGHT_RELEASE = 0x002000043037;
        public const ulong AUDI_MMI_BUTTON_WHEEL_LEFT_BEGIN = 0x014000013037;
        public const ulong AUDI_MMI_BUTTON_WHEEL_LEFT_RELEASE = 0x004000043037;
    }
}
