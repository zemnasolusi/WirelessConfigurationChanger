using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using NativeWifi;
using System.Collections.ObjectModel;
using WirelessConfigurationChanger.Enum;
using WirelessConfigurationChanger.Message;
using WirelessConfigurationChanger;
using System.Linq;
using System.Net;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using WirelessConfigurationChanger.Database;
using SQLite;
using System.IO;
using System;
using System.Diagnostics.CodeAnalysis;

namespace WirelessConfigurationChanger.ViewModel
{
    public class ProfileViewModel : ViewModelBase
    {
        private string _localDbPath;

        #region " NetworkAdapters "

        /// <summary>
        /// The <see cref="NetworkAdapters" /> property's name.
        /// </summary>
        public const string NetworkAdaptersPropertyName = "NetworkAdapters";

        private ObservableCollection<WlanClient.WlanInterface> _networkAdapters;

        /// <summary>
        /// Sets and gets the NetworkAdapters property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<WlanClient.WlanInterface> NetworkAdapters
        {
            get
            {
                return _networkAdapters;
            }

            set
            {
                if (_networkAdapters == value)
                {
                    return;
                }

                _networkAdapters = value;
                RaisePropertyChanged(NetworkAdaptersPropertyName);
            }
        }

        #endregion

        #region " SelectedNetworkAdapter "

        /// <summary>
        /// The <see cref="SelectedNetworkAdapter" /> property's name.
        /// </summary>
        public const string SelectedNetworkAdapterPropertyName = "SelectedNetworkAdapter";

        private WlanClient.WlanInterface _selectedNetworkAdapter;

        /// <summary>
        /// Sets and gets the SelectedNetworkAdapter property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public WlanClient.WlanInterface SelectedNetworkAdapter
        {
            get
            {
                return _selectedNetworkAdapter;
            }

            set
            {
                if (_selectedNetworkAdapter == value)
                {
                    return;
                }

                _selectedNetworkAdapter = value;
                RaisePropertyChanged(SelectedNetworkAdapterPropertyName);
            }
        }

        #endregion

        #region " Title "

        /// <summary>
        /// The <see cref="Title" /> property's name.
        /// </summary>
        public const string TitlePropertyName = "Title";

        private string _title;

        /// <summary>
        /// Sets and gets the Title property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                if (_title == value)
                {
                    return;
                }

                _title = value;
                RaisePropertyChanged(TitlePropertyName);
            }
        }

        #endregion

        #region " Mode "

        /// <summary>
        /// The <see cref="Mode" /> property's name.
        /// </summary>
        public const string ModePropertyName = "Mode";

        private ProfileModes _mode;

        /// <summary>
        /// Sets and gets the Mode property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ProfileModes Mode
        {
            get
            {
                return _mode;
            }

            set
            {
                if (_mode == value)
                {
                    return;
                }

                _mode = value;
                RaisePropertyChanged(ModePropertyName);

                if (value == ProfileModes.Add)
                    this.Title = "Add Profile";
                else if (value == ProfileModes.Update)
                    this.Title = "Update Profile";
                else
                    this.Title = "Edit Profile";
            }
        }

        #endregion

        #region " Name "

        /// <summary>
        /// The <see cref="Name" /> property's name.
        /// </summary>
        public const string NamePropertyName = "Name";

        private string _name;

        /// <summary>
        /// Sets and gets the Name property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (_name == value)
                {
                    return;
                }

                _name = value;
                RaisePropertyChanged(NamePropertyName);
            }
        }

        #endregion

        #region " InterfaceName "

        /// <summary>
        /// The <see cref="NetworkAdapterName" /> property's name.
        /// </summary>
        public const string NetworkAdapterNamePropertyName = "NetworkAdapterName";

        private string _networkAdapterName;

        /// <summary>
        /// Sets and gets the NetworkAdapterName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string NetworkAdapterName
        {
            get
            {
                return _networkAdapterName;
            }

            set
            {
                if (_networkAdapterName == value)
                {
                    return;
                }

                _networkAdapterName = value;
                RaisePropertyChanged(NetworkAdapterNamePropertyName);
            }
        }

        #endregion

        #region " BSSID "

        /// <summary>
        /// The <see cref="BSSID" /> property's name.
        /// </summary>
        public const string BSSIDPropertyName = "BSSID";

        private string _bssid;

        /// <summary>
        /// Sets and gets the BSSID property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string BSSID
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
                RaisePropertyChanged(BSSIDPropertyName);
            }
        }

        #endregion

        #region " SSID "

        /// <summary>
        /// The <see cref="SSID" /> property's name.
        /// </summary>
        public const string SSIDPropertyName = "SSID";

        private string _ssid = string.Empty;

        /// <summary>
        /// Sets and gets the SSID property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string SSID
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
                RaisePropertyChanged(SSIDPropertyName);
            }
        }

        #endregion

        #region " DhcpOption "

        /// <summary>
        /// The <see cref="DhcpOption" /> property's name.
        /// </summary>
        public const string DhcpOptionPropertyName = "DhcpOption";

        private DhcpOption _dhcpOption = DhcpOption.Enabled;

        /// <summary>
        /// Sets and gets the DhcpOption property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DhcpOption DhcpOption
        {
            get
            {
                return _dhcpOption;
            }

            set
            {
                if (_dhcpOption == value)
                {
                    return;
                }

                _dhcpOption = value;
                RaisePropertyChanged(DhcpOptionPropertyName);
            }
        }

        #endregion

        #region " IpAddress "

        /// <summary>
        /// The <see cref="IpAddress" /> property's name.
        /// </summary>
        public const string IpAddressPropertyName = "IpAddress";

        private string _ipAddress = string.Empty;

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

        private string _subnetMask = string.Empty;

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

        private string _gateway = string.Empty;

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

        #region " DnsOption "

        /// <summary>
        /// The <see cref="DnsOption" /> property's name.
        /// </summary>
        public const string DnsOptionPropertyName = "DnsOption";

        private DnsOption _dnsOption = DnsOption.Auto;

        /// <summary>
        /// Sets and gets the DnsOption property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DnsOption DnsOption
        {
            get
            {
                return _dnsOption;
            }

            set
            {
                if (_dnsOption == value)
                {
                    return;
                }

                _dnsOption = value;
                RaisePropertyChanged(DnsOptionPropertyName);
            }
        }

        #endregion

        #region " PreferredDnsServer "

        /// <summary>
        /// The <see cref="PreferredDnsServer" /> property's name.
        /// </summary>
        public const string PreferredDnsServerPropertyName = "PreferredDnsServer";

        private string _preferredDnsServer = string.Empty;

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

        private string _alternateDnsServer = string.Empty;

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

        private profile _profile = null;

        /// <summary>
        /// Initializes a new instance of the ProfileViewModel class.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProfileViewModel()
        {
            _localDbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Wireless Configuration Changer", "local.sqlite");

            WlanClient client = new WlanClient();
            this.NetworkAdapters = new ObservableCollection<WlanClient.WlanInterface>(client.Interfaces.ToList());
        }

        public void SetMode(ProfileModes mode, profile profile)
        {
            this.Mode = mode;
            _profile = profile;

            if (profile != null)
            {
                this.Name = _profile.name;
                this.SelectedNetworkAdapter = this.NetworkAdapters.SingleOrDefault(a => profile.adapter_id.Equals(a.InterfaceGuid.ToString()));
                this.NetworkAdapterName = this.SelectedNetworkAdapter.InterfaceName;
                this.BSSID = _profile.bssid;
                this.SSID = _profile.ssid;
                this.DhcpOption = _profile.dhcp_enabled > 0 ? DhcpOption.Enabled : DhcpOption.Disabled;
                this.IpAddress = _profile.ip_address;
                this.SubnetMask = _profile.subnet_mask;
                this.Gateway = _profile.gateway;
                this.DnsOption = _profile.dns_auto > 0 ? DnsOption.Auto : DnsOption.Manual;
                this.PreferredDnsServer = _profile.preferred_dns_server;
                this.AlternateDnsServer = _profile.alternate_dns_server;
            }
        }

        #region " CmdOK "

        private RelayCommand _cmdOK;

        /// <summary>
        /// Gets the CmdOK.
        /// </summary>
        public RelayCommand CmdOK
        {
            get
            {
                return _cmdOK
                    ?? (_cmdOK = new RelayCommand(
                    () =>
                    {
                        if (!CmdOK.CanExecute(null))
                        {
                            return;
                        }

                        SaveProfile();
                    },
                    () =>
                    {
                        if (string.IsNullOrEmpty(this.Name))
                            return false;

                        if (this.DhcpOption == DhcpOption.Disabled)
                        {
                            IPAddress ip;
                            var valid = IPAddress.TryParse(this.IpAddress, out ip);
                            if (valid == false)
                                return false;

                            valid = IPAddress.TryParse(this.SubnetMask, out ip);
                            if (valid == false)
                                return false;

                            valid = IPAddress.TryParse(this.Gateway, out ip);
                            if (valid == false)
                                return false;
                        }

                        if (this.DnsOption == DnsOption.Manual)
                        {
                            IPAddress ip;
                            var valid = IPAddress.TryParse(this.PreferredDnsServer, out ip);
                            if (valid == false)
                                return false;

                            if (string.IsNullOrEmpty(this.AlternateDnsServer) == false)
                            {
                                valid = IPAddress.TryParse(this.AlternateDnsServer, out ip);
                                if (valid == false)
                                    return false;
                            }
                        }

                        return true;
                    }));
            }
        }

        #endregion

        private void SaveProfile()
        {
            using (var conn = new SQLiteConnection(_localDbPath))
            {
                // Check item already exist or not
                var existItem = conn.Table<profile>().SingleOrDefault(a => a.adapter_id == this.SelectedNetworkAdapter.InterfaceGuid.ToString() &&
                    a.bssid == this.BSSID &&
                    a.ssid == this.SSID);

                if (_mode == ProfileModes.Add && existItem != null)
                {
                    MessageBox.Show("Profile already existed.", "Save Profile", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                profile profile = null;

                if (_mode == ProfileModes.Add)
                    profile = new profile();
                else
                    profile = existItem;

                profile.name = this.Name;
                profile.adapter_id = this.SelectedNetworkAdapter.InterfaceGuid.ToString();
                profile.bssid = this.BSSID;
                profile.ssid = this.SSID;
                profile.dhcp_enabled = this.DhcpOption == DhcpOption.Enabled ? (byte)1 : (byte)0;
                if (this.DhcpOption == Enum.DhcpOption.Enabled)
                {
                    profile.ip_address = null;
                    profile.subnet_mask = null;
                    profile.gateway = null;
                }
                else
                {
                    profile.ip_address = this.IpAddress;
                    profile.subnet_mask = this.SubnetMask;
                    profile.gateway = this.Gateway;
                }
                profile.dns_auto = this.DnsOption == DnsOption.Auto ? (byte)1 : (byte)0;
                if (this.DnsOption == Enum.DnsOption.Auto)
                {
                    profile.preferred_dns_server = null;
                    profile.alternate_dns_server = null;
                }
                else
                {
                    profile.preferred_dns_server = this.PreferredDnsServer;
                    profile.alternate_dns_server = this.AlternateDnsServer;
                }

                if (_mode == ProfileModes.Add)
                    conn.Insert(profile);
                else
                    conn.Update(profile);

                Messenger.Default.Send<WindowCloseMessage>(new WindowCloseMessage(), "ProfileWindow");
                Messenger.Default.Send<ProfileChangedMessage>(new ProfileChangedMessage());
            }
        }
    }
}