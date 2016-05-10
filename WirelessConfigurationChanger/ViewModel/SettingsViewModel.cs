using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using Microsoft.Win32.TaskScheduler;
using System;
using System.Reflection;
using System.Windows;
using WirelessConfigurationChanger.Message;

namespace WirelessConfigurationChanger.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        #region " StartAtWindowsStartup "

        /// <summary>
        /// The <see cref="StartAtWindowsStartup" /> property's name.
        /// </summary>
        public const string StartAtWindowsStartupPropertyName = "StartAtWindowsStartup";

        private bool _startAtWindowsStartup = false;

        /// <summary>
        /// Sets and gets the StartAtWindowsStartup property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool StartAtWindowsStartup
        {
            get
            {
                return _startAtWindowsStartup;
            }

            set
            {
                if (_startAtWindowsStartup == value)
                {
                    return;
                }

                _startAtWindowsStartup = value;
                RaisePropertyChanged(StartAtWindowsStartupPropertyName);
            }
        }

        #endregion

        #region " MinimizeAtStartup "

        /// <summary>
        /// The <see cref="MinimizeAtStartup" /> property's name.
        /// </summary>
        public const string MinimizeAtStartupPropertyName = "MinimizeAtStartup";

        private bool _minimizeAtStartup = false;

        /// <summary>
        /// Sets and gets the MinimizeAtStartup property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool MinimizeAtStartup
        {
            get
            {
                return _minimizeAtStartup;
            }

            set
            {
                if (_minimizeAtStartup == value)
                {
                    return;
                }

                _minimizeAtStartup = value;
                RaisePropertyChanged(MinimizeAtStartupPropertyName);
            }
        }

        #endregion

        #region " Constructor "

        public SettingsViewModel()
        {
        }

        #endregion

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
                        LoadStartAtWindowsStartup();
                        LoadMinimizeAtStartup();
                    }));
            }
        }

        #endregion

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
                        SetStartAtWindowsStartup();
                        SetMinimizeAtStartup();
                        Messenger.Default.Send<WindowCloseMessage>(new WindowCloseMessage(), "SettingsWindow");
                    }));
            }
        }

        #endregion

        private void LoadStartAtWindowsStartup()
        {
            /*
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", false);

            if (key.GetValue("Wireless Configuration Changer") == null)
                this.StartAtWindowsStartup = false;
            else
                this.StartAtWindowsStartup = true;
            */
            using (TaskService ts = new TaskService())
            {
                var task = ts.FindTask("Wireless Configuration Changer", true);

                if (task != null)
                    this.StartAtWindowsStartup = true;
                else
                    this.StartAtWindowsStartup = false;
            }
        }

        private void SetStartAtWindowsStartup()
        {
            /*
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (this.StartAtWindowsStartup)
                key.SetValue("Wireless Configuration Changer", "\"" + System.Reflection.Assembly.GetEntryAssembly().Location + "\"");
            else
                key.DeleteValue("Wireless Configuration Changer", false);
            */
            if (this.StartAtWindowsStartup)
            {
                using (TaskService ts = new TaskService())
                {
                    TaskDefinition td = ts.NewTask();
                    td.RegistrationInfo.Description = "Change wireless configuration by SSID";

                    var lt = new LogonTrigger();
                    lt.UserId = null;
                    lt.StartBoundary = DateTime.MinValue;

                    td.Triggers.Add(lt);
                    td.Actions.Add(new ExecAction(Assembly.GetExecutingAssembly().Location));
                    td.Principal.RunLevel = TaskRunLevel.Highest;
                    td.Settings.StopIfGoingOnBatteries = false;
                    td.Settings.DisallowStartIfOnBatteries = false;

                    ts.RootFolder.RegisterTaskDefinition("Wireless Configuration Changer", td);
                }
            }
            else
            {
                using (TaskService ts = new TaskService())
                {
                    ts.RootFolder.DeleteTask("Wireless Configuration Changer", false);
                }
            }

        }

        private void LoadMinimizeAtStartup()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\ZemnaSoft\\Wireless Configuration Changer", false);

            if (key == null)
            {
                this.MinimizeAtStartup = false;
                return;
            }

            var val = key.GetValue("MinimizeAtStartup");

            if (val == null)
                this.MinimizeAtStartup = false;
            else
            {
                this.MinimizeAtStartup = val.Equals(1);
            }
        }

        private void SetMinimizeAtStartup()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\ZemnaSoft\\Wireless Configuration Changer");

            key.SetValue("MinimizeAtStartup", this.MinimizeAtStartup ? 1 : 0);
        }
    }
}