using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WirelessConfigurationChanger.Enum
{
    public enum ProfileStatus
    {
        NotSet = 0,
        Disconnected = 1,
        New = 2,
        Different = 3,
        Same = 4,
        Changing = 5
    }
}
