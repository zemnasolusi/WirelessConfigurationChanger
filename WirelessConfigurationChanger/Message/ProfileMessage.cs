using GalaSoft.MvvmLight.Ioc;
using NativeWifi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WirelessConfigurationChanger.Database;
using WirelessConfigurationChanger.Enum;
using WirelessConfigurationChanger;

namespace WirelessConfigurationChanger.Message
{
    public class ProfileMessage
    {
        public ProfileModes Mode { get; private set; }

        public profile Profile { get; private set; }

        public ProfileMessage(ProfileModes mode)
            : this(mode, null)
        {
        }

        public ProfileMessage(ProfileModes mode, profile profile)
        {
            this.Mode = mode;
            this.Profile = profile;
        }
    }
}
