using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Hardcodet.Wpf.TaskbarNotification;
using log4net;
using NativeWifi;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WirelessConfigurationChanger.Database;
using WirelessConfigurationChanger.Enum;
using WirelessConfigurationChanger.Message;
using WirelessConfigurationChanger.Properties;

namespace WirelessConfigurationChanger.ViewModel
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public class MainViewModel : ViewModelBase
    {
        #region " Properties "

        private static readonly ILog log = LogManager.GetLogger(typeof(MainViewModel));

        private string _localDbPath = string.Empty;

        private NetworkInterface _networkInterface = null;

        private BackgroundWorker _worker = null;

        #region " NetworkAdapters "

        /// <summary>
        /// The <see cref="NetworkAdapters" /> property's name.
        /// </summary>
        public const string NetworkAdaptersPropertyName = "NetworkAdapters";

        private ObservableCollection<WlanClient.WlanInterface> _networkAdapters = null;

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

        private WlanClient.WlanInterface _selectedNetworkAdapter = null;

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
                _networkInterface = value.NetworkInterface;

                LoadProfiles();
            }
        }

        #endregion

        #region " CurrentNetworkInfo "

        /// <summary>
        /// The <see cref="CurrentNetworkInfo" /> property's name.
        /// </summary>
        public const string CurrentNetworkInfoPropertyName = "CurrentNetworkInfo";

        private NetworkInfo _currentNetworkInfo = null;

        /// <summary>
        /// Sets and gets the CurrentNetworkInfo property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public NetworkInfo CurrentNetworkInfo
        {
            get
            {
                return _currentNetworkInfo;
            }

            set
            {
                if (_currentNetworkInfo == value)
                {
                    return;
                }

                _currentNetworkInfo = value;
                RaisePropertyChanged(CurrentNetworkInfoPropertyName);
            }
        }

        #endregion

        #region " Profiles "

        /// <summary>
        /// The <see cref="Profiles" /> property's name.
        /// </summary>
        public const string ProfilesPropertyName = "Profiles";

        private ObservableCollection<profile> _profiles = null;

        /// <summary>
        /// Sets and gets the Profiles property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<profile> Profiles
        {
            get
            {
                return _profiles;
            }

            set
            {
                if (_profiles == value)
                {
                    return;
                }

                _profiles = value;
                RaisePropertyChanged(ProfilesPropertyName);
            }
        }

        #endregion

        #region " SelectedProfile "

        /// <summary>
        /// The <see cref="SelectedProfile" /> property's name.
        /// </summary>
        public const string SelectedProfilePropertyName = "SelectedProfile";

        private profile _selectedProfile = null;

        /// <summary>
        /// Sets and gets the SelectedProfile property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public profile SelectedProfile
        {
            get
            {
                return _selectedProfile;
            }

            set
            {
                if (_selectedProfile == value)
                {
                    return;
                }

                _selectedProfile = value;
                RaisePropertyChanged(SelectedProfilePropertyName);

                CmdEditProfile.RaiseCanExecuteChanged();
                CmdApplyProfile.RaiseCanExecuteChanged();
                CmdDeleteProfile.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region " CurrentProfile "

        /// <summary>
        /// The <see cref="CurrentProfile" /> property's name.
        /// </summary>
        public const string CurrentProfilePropertyName = "CurrentProfile";

        private profile _currentProfile = null;

        /// <summary>
        /// Sets and gets the CurrentProfile property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public profile CurrentProfile
        {
            get
            {
                return _currentProfile;
            }

            set
            {
                if (_currentProfile == value)
                {
                    return;
                }

                _currentProfile = value;
                RaisePropertyChanged(CurrentProfilePropertyName);

                CmdAddProfile.RaiseCanExecuteChanged();
                CmdUpdateProfile.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region " ProfileName "

        /// <summary>
        /// The <see cref="ProfileName" /> property's name.
        /// </summary>
        public const string ProfileNamePropertyName = "ProfileName";

        private string _profileName = string.Empty;

        /// <summary>
        /// Sets and gets the ProfileName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ProfileName
        {
            get
            {
                return _profileName;
            }

            set
            {
                if (_profileName == value)
                {
                    return;
                }

                _profileName = value;
                RaisePropertyChanged(ProfileNamePropertyName);
            }
        }

        #endregion

        #region " IsAddingProfile "

        /// <summary>
        /// The <see cref="IsAddingProfile" /> property's name.
        /// </summary>
        public const string IsAddingProfilePropertyName = "IsAddingProfile";

        private bool _isAddingProfile = false;

        /// <summary>
        /// Sets and gets the IsAddingProfile property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsAddingProfile
        {
            get
            {
                return _isAddingProfile;
            }

            set
            {
                if (_isAddingProfile == value)
                {
                    return;
                }

                _isAddingProfile = value;
                RaisePropertyChanged(IsAddingProfilePropertyName);
            }
        }

        #endregion

        #region " ProfileStatus "

        /// <summary>
        /// The <see cref="ProfileStatus" /> property's name.
        /// </summary>
        public const string ProfileStatusPropertyName = "ProfileStatus";

        private ProfileStatus _profileStatus = ProfileStatus.NotSet;

        /// <summary>
        /// Sets and gets the ProfileStatus property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ProfileStatus ProfileStatus
        {
            get
            {
                return _profileStatus;
            }

            set
            {
                if (_profileStatus == value)
                {
                    return;
                }

                _profileStatus = value;
                RaisePropertyChanged(ProfileStatusPropertyName);
            }
        }

        #endregion

        #region " ChangeInfo "

        /// <summary>
        /// The <see cref="ChangeInfo" /> property's name.
        /// </summary>
        public const string ChangeInfoPropertyName = "ChangeInfo";

        private ChangeInfo _changeInfo = null;

        /// <summary>
        /// Sets and gets the ChangeInfo property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ChangeInfo ChangeInfo
        {
            get
            {
                return _changeInfo;
            }

            set
            {
                if (_changeInfo == value)
                {
                    return;
                }

                _changeInfo = value;
                RaisePropertyChanged(ChangeInfoPropertyName);
            }
        }

        #endregion

        #region " StatusMessage "

        /// <summary>
        /// The <see cref="StatusMessage" /> property's name.
        /// </summary>
        public const string StatusMessagePropertyName = "StatusMessage";

        private string _statusMessage = "";

        /// <summary>
        /// Sets and gets the StatusMessage property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string StatusMessage
        {
            get
            {
                return _statusMessage;
            }

            set
            {
                if (_statusMessage == value)
                {
                    return;
                }

                _statusMessage = value;
                RaisePropertyChanged(StatusMessagePropertyName);
            }
        }

        #endregion

        #endregion

        #region " Constructor / Destructor "

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            // Set Path of Local Database File
            _localDbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Wireless Configuration Changer", "local.sqlite");

            // Create a new SQLite file if not exist
            if (File.Exists(_localDbPath) == false)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_localDbPath));
                SQLiteDb db = new SQLiteDb(_localDbPath);
                db.Create();
            }

            Messenger.Default.Register<ProfileChangedMessage>(this, (msg) =>
            {
                LoadProfiles();
            });
        }

        public override void Cleanup()
        {
            _worker.CancelAsync();
            _worker.Dispose();

            base.Cleanup();
        }
        
        #endregion

        #region " Commands "

        #region " CmdLoaded "

        private RelayCommand _cmdLoaded;

        /// <summary>
        /// Gets the CmdLoaded.
        /// </summary>
        public RelayCommand CmdLoaded
        {
            get
            {
                return _cmdLoaded
                    ?? (_cmdLoaded = new RelayCommand(
                    () =>
                    {
                        LoadNetworkAdapters();
                        StartMainWorker();
                    }));
            }
        }

        #endregion

        #region " CmdShowProfile "

        private RelayCommand _cmdShowProfile;

        /// <summary>
        /// Gets the CmdShowProfile.
        /// </summary>
        public RelayCommand CmdShowProfile
        {
            get
            {
                return _cmdShowProfile
                    ?? (_cmdShowProfile = new RelayCommand(
                    () =>
                    {
                        if (!CmdShowProfile.CanExecute(null))
                        {
                            return;
                        }


                    },
                    () =>
                    {
                        return this.SelectedProfile != null;
                    }));
            }
        }

        #endregion

        #region " CmdEditProfile "

        private RelayCommand _cmdEditProfile;

        /// <summary>
        /// Gets the CmdEditProfile.
        /// </summary>
        public RelayCommand CmdEditProfile
        {
            get
            {
                return _cmdEditProfile
                    ?? (_cmdEditProfile = new RelayCommand(
                    () =>
                    {
                        if (!CmdEditProfile.CanExecute(null))
                        {
                            return;
                        }

                        Messenger.Default.Send<ProfileMessage>(new ProfileMessage(ProfileModes.Edit, this.SelectedProfile));
                    },
                    () =>
                    {
                        return this.SelectedProfile != null;
                    }));
            }
        }

        #endregion

        #region " CmdDeleteProfile "

        private RelayCommand _cmdDeleteProfile;

        /// <summary>
        /// Gets the CmdDeleteProfile.
        /// </summary>
        public RelayCommand CmdDeleteProfile
        {
            get
            {
                return _cmdDeleteProfile
                    ?? (_cmdDeleteProfile = new RelayCommand(() =>
                    {
                        if (!CmdDeleteProfile.CanExecute(null))
                        {
                            return;
                        }

                        if (MessageBox.Show("Are you sure?", "Delete Profile", MessageBoxButton.OKCancel,
                                MessageBoxImage.Warning) == MessageBoxResult.Cancel)
                            return;

                        DeleteProfile();
                    },
                    () =>
                    {
                        return this.SelectedProfile != null;
                    }));
            }
        }

        #endregion

        #region " CmdAddProfile "

        private RelayCommand _cmdAddProfile;

        /// <summary>
        /// Gets the CmdAddProfile.
        /// </summary>
        public RelayCommand CmdAddProfile
        {
            get
            {
                return _cmdAddProfile
                    ?? (_cmdAddProfile = new RelayCommand(
                    () =>
                    {
                        if (!CmdAddProfile.CanExecute(null))
                        {
                            return;
                        }

                        var profile = new profile();

                        profile.name = this.CurrentNetworkInfo.Ssid;
                        profile.adapter_id = this.CurrentNetworkInfo.InterfaceGuid;
                        profile.bssid = this.CurrentNetworkInfo.Bssid;
                        profile.ssid = this.CurrentNetworkInfo.Ssid;
                        profile.dhcp_enabled = this.CurrentNetworkInfo.IsDhcpEnabled ? (byte)1 : (byte)0;
                        if (this.CurrentNetworkInfo.IsDhcpEnabled == false)
                        {
                            profile.ip_address = this.CurrentNetworkInfo.IpAddress;
                            profile.subnet_mask = this.CurrentNetworkInfo.SubnetMask;
                            profile.gateway = this.CurrentNetworkInfo.Gateway;
                        }
                        profile.dns_auto = this.CurrentNetworkInfo.IsDnsAuto ? (byte)1 : (byte)0;
                        if (this.CurrentNetworkInfo.IsDnsAuto == false)
                        {
                            profile.preferred_dns_server = this.CurrentNetworkInfo.PreferredDnsServer;
                            profile.alternate_dns_server = this.CurrentNetworkInfo.AlternateDnsServer;
                        }

                        Messenger.Default.Send<ProfileMessage>(new ProfileMessage(ProfileModes.Add, profile));
                    },
                    () =>
                    {
                        if (this.IsAddingProfile)
                            return false;

                        return this.CurrentNetworkInfo != null;
                    }));
            }
        }

        #endregion

        #region " CmdApplyProfile "

        private RelayCommand<profile> _cmdApplyProfile;

        /// <summary>
        /// Gets the CmdApplyProfile.
        /// </summary>
        public RelayCommand<profile> CmdApplyProfile
        {
            get
            {
                return _cmdApplyProfile
                    ?? (_cmdApplyProfile = new RelayCommand<profile>(
                    p =>
                    {
                        if (!CmdApplyProfile.CanExecute(p))
                        {
                            return;
                        }

                        ApplyProfile(p);
                    },
                    p =>
                    {
                        return p != null;
                    }));
            }
        }

        #endregion

        #region " CmdUpdateProfile "

        private RelayCommand _cmdUpdateProfile;

        /// <summary>
        /// Gets the CmdUpdateProfile.
        /// </summary>
        public RelayCommand CmdUpdateProfile
        {
            get
            {
                return _cmdUpdateProfile
                    ?? (_cmdUpdateProfile = new RelayCommand(
                    () =>
                    {
                        if (!CmdUpdateProfile.CanExecute(null))
                        {
                            return;
                        }

                        var profile = new profile();

                        profile.id = this.CurrentProfile.id;
                        profile.name = this.CurrentProfile.name;
                        profile.adapter_id = this.CurrentNetworkInfo.InterfaceGuid;
                        profile.bssid = this.CurrentNetworkInfo.Bssid;
                        profile.ssid = this.CurrentNetworkInfo.Ssid;
                        profile.dhcp_enabled = this.CurrentNetworkInfo.IsDhcpEnabled ? (byte)1 : (byte)0;
                        if (this.CurrentNetworkInfo.IsDhcpEnabled == false)
                        {
                            profile.ip_address = this.CurrentNetworkInfo.IpAddress;
                            profile.subnet_mask = this.CurrentNetworkInfo.SubnetMask;
                            profile.gateway = this.CurrentNetworkInfo.Gateway;
                        }
                        profile.dns_auto = this.CurrentNetworkInfo.IsDnsAuto ? (byte)1 : (byte)0;
                        if (this.CurrentNetworkInfo.IsDnsAuto == false)
                        {
                            profile.preferred_dns_server = this.CurrentNetworkInfo.PreferredDnsServer;
                            profile.alternate_dns_server = this.CurrentNetworkInfo.AlternateDnsServer;
                        }

                        Messenger.Default.Send<ProfileMessage>(new ProfileMessage(ProfileModes.Update, profile));
                    },
                    () =>
                    {
                        if (this.ProfileStatus != Enum.ProfileStatus.Different)
                            return false;

                        return true;
                    }));
            }
        }

        #endregion

        #region " CmdSettings "

        private RelayCommand _cmdSettings;

        /// <summary>
        /// Gets the CmdSettings.
        /// </summary>
        public RelayCommand CmdSettings
        {
            get
            {
                return _cmdSettings
                    ?? (_cmdSettings = new RelayCommand(
                    () =>
                    {
                        Messenger.Default.Send(new NotificationMessage("Settings"));
                    }));
            }
        }

        #endregion

        #region " CmdHelp "

        private RelayCommand _cmdHelp;

        /// <summary>
        /// Gets the CmdHelp.
        /// </summary>
        public RelayCommand CmdHelp
        {
            get
            {
                return _cmdHelp
                    ?? (_cmdHelp = new RelayCommand(
                    () =>
                    {
                        Process.Start(Settings.Default.UserGuideUrl);
                    }));
            }
        }

        #endregion

        #region " CmdAbout "

        private RelayCommand _cmdAbout;

        /// <summary>
        /// Gets the CmdAbout.
        /// </summary>
        public RelayCommand CmdAbout
        {
            get
            {
                return _cmdAbout
                    ?? (_cmdAbout = new RelayCommand(
                    () =>
                    {
                        Messenger.Default.Send<AboutMessage>(new AboutMessage());
                    }));
            }
        }

        #endregion

        #region " CmdActivate "

        private RelayCommand _cmdActivate;

        /// <summary>
        /// Gets the CmdActivate.
        /// </summary>
        public RelayCommand CmdActivate
        {
            get
            {
                return _cmdActivate
                    ?? (_cmdActivate = new RelayCommand(
                    () =>
                    {
                        Messenger.Default.Send<WindowActivateMessage>(new WindowActivateMessage(), "MainWindow");
                    }));
            }
        }

        #endregion

        #region " CmdExit "

        private RelayCommand _cmdExit;

        /// <summary>
        /// Gets the CmdExit.
        /// </summary>
        public RelayCommand CmdExit
        {
            get
            {
                return _cmdExit
                    ?? (_cmdExit = new RelayCommand(
                    () =>
                    {
                        Application.Current.Shutdown();
                    }));
            }
        }

        #endregion

        #endregion

        #region " Private Methods "

        private async void LoadNetworkAdapters()
        {
            Task task = new Task(() =>
            {
                // Load Wireless Network Adapters
                WlanClient client = new WlanClient();
                this.NetworkAdapters = new ObservableCollection<WlanClient.WlanInterface>(client.Interfaces.ToList());
                this.SelectedNetworkAdapter = this.NetworkAdapters.FirstOrDefault();

                if (this.SelectedNetworkAdapter != null)
                {
                    this.StatusMessage = "Network adapter loaded successfully";
                }
                else
                {
                    this.StatusMessage = "No network adapter found";
                }
            });

            task.Start();

            await task;
        }

        private void StartMainWorker()
        {
            _worker = new BackgroundWorker();

            _worker.DoWork += worker_DoWork;
            _worker.WorkerReportsProgress = false;
            _worker.WorkerSupportsCancellation = true;

            _worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            while (!worker.CancellationPending)
            {
                if (this.SelectedNetworkAdapter == null || this.Profiles == null)
                {
                    Thread.Sleep(1000);
                    continue;
                }

                try
                {
                    var ni = LoadCurrentNetworkInfo();

                    if (ni == null)
                    {
                        if (this.ProfileStatus != Enum.ProfileStatus.Disconnected)
                        {
                            this.StatusMessage = "Disconnected...";
                            this.ProfileStatus = Enum.ProfileStatus.Disconnected;
                            this.CurrentProfile = null;
                            this.ProfileName = string.Empty;
                            //this.ChangeInfo = null;
                        }
                        Thread.Sleep(1000);
                        continue;
                    }

                    bool isNetworkChanged = ! ni.IsSameConnection(this.CurrentNetworkInfo);

                    // Check saved profile
                    var profile = this.Profiles.FirstOrDefault(a => a.adapter_id == ni.InterfaceGuid &&
                        a.bssid == ni.Bssid &&
                        a.ssid == ni.Ssid);

                    if (profile == null)
                    {
                        this.CurrentProfile = null;
                        this.ProfileName = string.Empty;
                        //this.ChangeInfo = null;

                        if (this.ProfileStatus != Enum.ProfileStatus.New)
                        {
                            this.StatusMessage = "No profile found...";

                            Messenger.Default.Send<ShowBalloonTipMessage>(
                                new ShowBalloonTipMessage(
                                    "No profile found",
                                    "Please save this network configuration", BalloonIcon.Warning
                                    ));

                            this.ProfileStatus = Enum.ProfileStatus.New;
                        }

                        this.CurrentNetworkInfo = ni;
                        Thread.Sleep(3000);
                        continue;
                    }

                    if (ni.IsSameConfiguration(profile))
                    {
                        //this.ChangeInfo = null;
                        this.ProfileName = profile.name;

                        if (this.ProfileStatus != Enum.ProfileStatus.Same)
                        {
                            this.StatusMessage = profile.name + " profile found...";
                            this.ProfileStatus = Enum.ProfileStatus.Same;
                        }
                        this.CurrentNetworkInfo = ni;
                        Thread.Sleep(3000);
                        continue;
                    }

                    this.ProfileStatus = Enum.ProfileStatus.Different;

                    if (isNetworkChanged)
                    {
                        ApplyProfile(profile, ni);
                        if (this.ProfileStatus != Enum.ProfileStatus.Changing)
                        {
                            Messenger.Default.Send<ShowBalloonTipMessage>(
                                new ShowBalloonTipMessage(
                                    "Profile found",
                                    "Applying " + profile.name + " profile...", BalloonIcon.Info
                                    ));

                            this.ProfileStatus = Enum.ProfileStatus.Changing;
                        }
                    }
                    else
                    {
                        this.StatusMessage = profile.name + " profile found but has different configuration...";
                        if (this.ProfileStatus != Enum.ProfileStatus.Different)
                        {
                            Messenger.Default.Send<ShowBalloonTipMessage>(
                                new ShowBalloonTipMessage(
                                    "Profile different",
                                    "Your profile is different with current configuration...", BalloonIcon.Warning
                                    ));

                            this.ProfileStatus = Enum.ProfileStatus.Different;
                        }
                    }

                    this.CurrentProfile = profile;
                    this.ProfileName = profile.name;

                    this.CurrentNetworkInfo = ni;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message, ex);
                }

                Thread.Sleep(3000);
            }
        }

        private NetworkInfo LoadCurrentNetworkInfo()
        {
            if (this.SelectedNetworkAdapter.InterfaceState != Wlan.WlanInterfaceState.Connected)
                return null;

            var currCon = this.SelectedNetworkAdapter.CurrentConnection;

            var ni = new NetworkInfo();
            ni.InterfaceGuid = this.SelectedNetworkAdapter.InterfaceGuid.ToString();
            ni.Bssid = BitConverter.ToString(currCon.wlanAssociationAttributes.dot11Bssid);
            ni.Ssid = Encoding.ASCII.GetString(currCon.wlanAssociationAttributes.dot11Ssid.SSID, 0, (int)currCon.wlanAssociationAttributes.dot11Ssid.SSIDLength);

            var nam = new NetworkAdapterManager(_networkInterface);

            ni.IsDhcpEnabled = nam.IsDhcpEnabled();
            ni.IpAddress = nam.GetIPAddress().ToString();
            ni.SubnetMask = nam.GetSubnetMask().ToString();
            var gateway = nam.GetGateway();
            if (gateway != null)
                ni.Gateway = gateway.ToString();
            ni.IsDnsAuto = nam.IsDnsServerAuto();
            var nameServers = nam.GetNameServers();
            if (nameServers.Count() > 0)
                ni.PreferredDnsServer = nameServers.First().ToString();
            if (nameServers.Count() > 1)
                ni.AlternateDnsServer = nameServers.ElementAt(1).ToString();

            return ni;
        }

        private async void LoadProfiles()
        {
            Task task = new Task(() =>
            {
                this.StatusMessage = "Loading profiles...";

                string adapter_id = this.SelectedNetworkAdapter.InterfaceGuid.ToString();

                using (var conn = new SQLiteConnection(_localDbPath))
                {
                    var items = conn.Table<profile>().Where(a => a.adapter_id == adapter_id)
                        .OrderBy(a => a.ssid);

                    this.Profiles = new ObservableCollection<profile>(items);
                }
            });

            task.Start();

            await task;
        }

        private async void AddProfile()
        {
            Task task = new Task(() =>
            {
                using (var conn = new SQLiteConnection(_localDbPath))
                {
                    profile profile = new profile();

                    profile.adapter_id = this.CurrentNetworkInfo.InterfaceGuid;
                    profile.bssid = this.CurrentNetworkInfo.Bssid;
                    profile.ssid = this.CurrentNetworkInfo.Ssid;
                    profile.dhcp_enabled = this.CurrentNetworkInfo.IsDhcpEnabled ? (byte)1 : (byte)0;
                    if (this.CurrentNetworkInfo.IsDhcpEnabled == false)
                    {
                        profile.ip_address = this.CurrentNetworkInfo.IpAddress;
                        profile.subnet_mask = this.CurrentNetworkInfo.SubnetMask;
                        profile.gateway = this.CurrentNetworkInfo.Gateway;
                    }
                    profile.dns_auto = this.CurrentNetworkInfo.IsDnsAuto ? (byte)1 : (byte)0;
                    if (this.CurrentNetworkInfo.IsDnsAuto == false)
                    {
                        profile.preferred_dns_server = this.CurrentNetworkInfo.PreferredDnsServer;
                        profile.alternate_dns_server = this.CurrentNetworkInfo.AlternateDnsServer;
                    }

                    conn.Insert(profile);
                }
            });

            this.IsAddingProfile = true;

            task.Start();

            await task;

            this.IsAddingProfile = false;

            LoadProfiles();
        }

        private async void DeleteProfile()
        {
            Task task = new Task(() =>
            {
                using (var conn = new SQLiteConnection(_localDbPath))
                {
                    if (this.SelectedProfile == null)
                        return;

                    var profile = conn.Table<profile>().SingleOrDefault(a => a.id == this.SelectedProfile.id);

                    if (profile != null)
                        conn.Delete(profile);
                }
            });

            task.Start();

            await task;

            LoadProfiles();
        }

        private void ApplyProfile(profile profile)
        {
            ApplyProfile(profile, this.CurrentNetworkInfo);
        }

        private async void ApplyProfile(profile profile, NetworkInfo netInfo)
        {
            Task task = new Task(() =>
            {
                this.ChangeInfo = new ChangeInfo();

                var nam = new NetworkAdapterManager(_networkInterface);

                while (true)
                {
                    if (profile.dhcp_enabled > 0 && this.ChangeInfo.IpAddress == ChangeStatus.Changing)
                    {
                        if (nam.EnableDhcp())
                            this.ChangeInfo.IpAddress = ChangeStatus.Success;
                    }
                    
                    if (profile.dhcp_enabled == 0 && this.ChangeInfo.IpAddress == ChangeStatus.Changing)
                    {
                        bool ipChanged, gwChanged;
                        ipChanged = nam.SetIPAddress(IPAddress.Parse(profile.ip_address), IPAddress.Parse(profile.subnet_mask));
                        gwChanged = nam.SetGatewy(IPAddress.Parse(profile.gateway));
 
                        if (ipChanged && gwChanged)
                            this.ChangeInfo.IpAddress = ChangeStatus.Success;
                    }

                    if (profile.dns_auto > 0 && this.ChangeInfo.Dns == ChangeStatus.Changing)
                    {
                        if (nam.SetNameServers(new List<IPAddress>()))
                            this.ChangeInfo.Dns = ChangeStatus.Success;
                    }
                    
                    if (profile.dns_auto == 0 && this.ChangeInfo.Dns == ChangeStatus.Changing)
                    {
                        var dnsServers = new List<IPAddress>();
                        dnsServers.Add(IPAddress.Parse(profile.preferred_dns_server));
                        if (!string.IsNullOrEmpty(profile.alternate_dns_server))
                            dnsServers.Add(IPAddress.Parse(profile.alternate_dns_server));

                        if (nam.SetNameServers(dnsServers))
                            this.ChangeInfo.Dns = ChangeStatus.Success;
                    }

                    if (this.ChangeInfo.ChangeStatus == ChangeStatus.Success)
                        break;

                    if (this.ChangeInfo.ChangeStatus == ChangeStatus.Changing)
                    {
                        this.ChangeInfo.TryCount++;

                        if (this.ChangeInfo.TryCount > 3)
                        {
                            if (this.ChangeInfo.IpAddress == ChangeStatus.Changing)
                                this.ChangeInfo.IpAddress = ChangeStatus.Failed;
                            if (this.ChangeInfo.Dns == ChangeStatus.Changing)
                                this.ChangeInfo.Dns = ChangeStatus.Failed;

                            this.StatusMessage = "Failed to apply " + profile.name + " profile...";
                            Messenger.Default.Send<ShowBalloonTipMessage>(
                                new ShowBalloonTipMessage(
                                    "Applying Profile",
                                    "Failed to apply " + profile.name + " profile", BalloonIcon.Warning
                                    ));

                            break;
                        }
                    }

                    Thread.Sleep(500);
                }
            });

            this.StatusMessage = "Applying " + profile.name + " profile...";

            task.Start();

            await task;
        }

        #endregion
    }
}