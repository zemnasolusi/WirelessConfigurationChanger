using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using WirelessConfigurationChanger.Database;

namespace WirelessConfigurationChanger
{
    public class NetworkInfo : INotifyPropertyChanged
    {
        #region " InterfaceGuid "

        /// <summary>
        /// The <see cref="InterfaceGuid" /> property's name.
        /// </summary>
        public const string InterfaceGuidPropertyName = "InterfaceGuid";

        private string _interfaceGuid = "";

        /// <summary>
        /// Sets and gets the InterfaceGuid property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string InterfaceGuid
        {
            get
            {
                return _interfaceGuid;
            }

            set
            {
                if (_interfaceGuid == value)
                {
                    return;
                }

                _interfaceGuid = value;
                RaisePropertyChanged(InterfaceGuidPropertyName);
            }
        }

        #endregion

        #region " BSSID "

        /// <summary>
        /// The <see cref="Bssid" /> property's name.
        /// </summary>
        public const string BssidPropertyName = "Bssid";

        private string _bssid = "";

        /// <summary>
        /// Sets and gets the Bssid property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Bssid
        {
            get
            {
                return _bssid;
            }

            set
            {
                if (_bssid == value)
                {
                    return;
                }

                _bssid = value;
                RaisePropertyChanged(BssidPropertyName);
            }
        }

        #endregion

        #region " Ssid "

        /// <summary>
        /// The <see cref="Ssid" /> property's name.
        /// </summary>
        public const string SsidPropertyName = "Ssid";

        private string _ssid = "";

        /// <summary>
        /// Sets and gets the Ssid property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Ssid
        {
            get
            {
                return _ssid;
            }

            set
            {
                if (_ssid == value)
                {
                    return;
                }

                _ssid = value;
                RaisePropertyChanged(SsidPropertyName);
            }
        }

        #endregion

        #region " IsDhcpEnabled "

        /// <summary>
        /// The <see cref="IsDhcpEnabled" /> property's name.
        /// </summary>
        public const string IsDhcpEnabledPropertyName = "IsDhcpEnabled";

        private bool _isDhcpEnabled = false;

        /// <summary>
        /// Sets and gets the IsDhcpEnabled property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsDhcpEnabled
        {
            get
            {
                return _isDhcpEnabled;
            }

            set
            {
                if (_isDhcpEnabled == value)
                {
                    return;
                }

                _isDhcpEnabled = value;
                RaisePropertyChanged(IsDhcpEnabledPropertyName);
            }
        }

        #endregion

        #region " IpAddress "

        /// <summary>
        /// The <see cref="IpAddress" /> property's name.
        /// </summary>
        public const string IpAddressPropertyName = "IpAddress";

        private string _ipAddress = "";

        /// <summary>
        /// Sets and gets the IpAddress property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string IpAddress
        {
            get
            {
                return _ipAddress;
            }

            set
            {
                if (_ipAddress == value)
                {
                    return;
                }

                _ipAddress = value;
                RaisePropertyChanged(IpAddressPropertyName);
            }
        }

        #endregion

        #region " SubnetMask "

        /// <summary>
        /// The <see cref="SubnetMask" /> property's name.
        /// </summary>
        public const string SubnetMaskPropertyName = "SubnetMask";

        private string _subnetMask = "";

        /// <summary>
        /// Sets and gets the SubnetMask property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string SubnetMask
        {
            get
            {
                return _subnetMask;
            }

            set
            {
                if (_subnetMask == value)
                {
                    return;
                }

                _subnetMask = value;
                RaisePropertyChanged(SubnetMaskPropertyName);
            }
        }

        #endregion

        #region " Gateway "

        /// <summary>
        /// The <see cref="Gateway" /> property's name.
        /// </summary>
        public const string GatewayPropertyName = "Gateway";

        private string _gateway = "";

        /// <summary>
        /// Sets and gets the Gateway property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Gateway
        {
            get
            {
                return _gateway;
            }

            set
            {
                if (_gateway == value)
                {
                    return;
                }

                _gateway = value;
                RaisePropertyChanged(GatewayPropertyName);
            }
        }

        #endregion

        #region " IsDnsAuto "

        /// <summary>
        /// The <see cref="IsDnsAuto" /> property's name.
        /// </summary>
        public const string IsDnsAutoPropertyName = "IsDnsAuto";

        private bool _isDnsAuto = false;

        /// <summary>
        /// Sets and gets the IsDnsAuto property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsDnsAuto
        {
            get
            {
                return _isDnsAuto;
            }

            set
            {
                if (_isDnsAuto == value)
                {
                    return;
                }

                _isDnsAuto = value;
                RaisePropertyChanged(IsDnsAutoPropertyName);
            }
        }

        #endregion

        #region " PreferredDnsServer "

        /// <summary>
        /// The <see cref="PreferredDnsServer" /> property's name.
        /// </summary>
        public const string PreferredDnsServerPropertyName = "PreferredDnsServer";

        private string _preferredDnsServer = "";

        /// <summary>
        /// Sets and gets the PreferredDnsServer property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string PreferredDnsServer
        {
            get
            {
                return _preferredDnsServer;
            }

            set
            {
                if (_preferredDnsServer == value)
                {
                    return;
                }

                _preferredDnsServer = value;
                RaisePropertyChanged(PreferredDnsServerPropertyName);
            }
        }

        #endregion

        #region " AlternateDnsServer "

        /// <summary>
        /// The <see cref="AlternateDnsServer" /> property's name.
        /// </summary>
        public const string AlternateDnsServerPropertyName = "AlternateDnsServer";

        private string _alternateDnsServer = "";

        /// <summary>
        /// Sets and gets the AlternateDnsServer property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string AlternateDnsServer
        {
            get
            {
                return _alternateDnsServer;
            }

            set
            {
                if (_alternateDnsServer == value)
                {
                    return;
                }

                _alternateDnsServer = value;
                RaisePropertyChanged(AlternateDnsServerPropertyName);
            }
        }

        #endregion

        public bool IsSameConnection(NetworkInfo ni)
        {
            if (ni == null)
                return false;

            if (this.InterfaceGuid != ni.InterfaceGuid)
                return false;

            if (this.Bssid != ni.Bssid)
                return false;

            if (this.Ssid != ni.Ssid)
                return false;

            return true;
        }

        public bool IsSameConfiguration(profile profile)
        {
            if (profile == null)
                return false;

            if (this.InterfaceGuid != profile.adapter_id)
                return false;

            if (this.Bssid != profile.bssid)
                return false;

            if (this.Ssid != profile.ssid)
                return false;

            if (this.IsDhcpEnabled != (profile.dhcp_enabled > 0 ? true : false))
                return false;

            if (this.IsDhcpEnabled == false)
            {
                if (this.IpAddress != profile.ip_address)
                    return false;

                if (this.SubnetMask != profile.subnet_mask)
                    return false;

                if (this.Gateway != profile.gateway)
                    return false;
            }

            if (this.IsDnsAuto != (profile.dns_auto > 0 ? true : false))
                return false;

            if (this.IsDnsAuto == false)
            {
                if (this.PreferredDnsServer != profile.preferred_dns_server)
                    return false;

                if (this.AlternateDnsServer != (profile.alternate_dns_server == null ? "" : profile.alternate_dns_server))
                    return false;
            }

            return true;
        }

        public NetworkInfo Clone()
        {
            var ni = new NetworkInfo();

            ni.InterfaceGuid = this.InterfaceGuid;
            ni.Bssid = this.Bssid;
            ni.Ssid = this.Ssid;
            ni.IsDhcpEnabled = this.IsDhcpEnabled;
            ni.IpAddress = this.IpAddress;
            ni.SubnetMask = this.SubnetMask;
            ni.Gateway = this.Gateway;
            ni.IsDnsAuto = this.IsDnsAuto;
            ni.PreferredDnsServer = this.PreferredDnsServer;
            ni.AlternateDnsServer = this.AlternateDnsServer;

            return ni;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
