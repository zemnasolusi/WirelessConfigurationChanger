using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Win32;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WirelessConfigurationChanger.Message;
using WirelessConfigurationChanger.View;
using WirelessConfigurationChanger.ViewModel;

namespace WirelessConfigurationChanger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();

            Messenger.Default.Register<ProfileMessage>(this, OnProfileMessage);
            Messenger.Default.Register<NotificationMessage>(this, OnNotificationMessage);
            Messenger.Default.Register<AboutMessage>(this, OnAboutMessage);
            Messenger.Default.Register<WindowActivateMessage>(this, "MainWindow", OnWindowActivateMessage);
            Messenger.Default.Register<ShowBalloonTipMessage>(this, OnShowBalloonTipMessage);
            
            bool isMinimized = false;
            var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\ZemnaSoft\\Wireless Configuration Changer", false);
            if (key != null)
            {
                var val = key.GetValue("MinimizeAtStartup");
                if (val != null)
                {
                    if (val.Equals(1))
                        isMinimized = true;
                }
            }

            if (isMinimized)
            {
                this.WindowState = WindowState.Minimized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void OnShowBalloonTipMessage(ShowBalloonTipMessage msg)
        {
            taskbarIcon.ShowBalloonTip(msg.Title, msg.Text, msg.Symbol);
        }

        private void OnWindowActivateMessage(WindowActivateMessage obj)
        {
            this.WindowState = WindowState.Normal;
            this.Activate();
        }

        private void OnAboutMessage(AboutMessage msg)
        {
            var win = new AboutWindow();

            win.Owner = this;

            win.ShowDialog();
        }

        private void OnProfileMessage(ProfileMessage msg)
        {
            var win = new ProfileWindow();

            win.Owner = this;

            var vm = ServiceLocator.Current.GetInstance<ProfileViewModel>();
            vm.SetMode(msg.Mode, msg.Profile);

            win.ShowDialog();
        }

        private void OnNotificationMessage(NotificationMessage msg)
        {
            switch (msg.Notification)
            {
                case "Settings":
                    var win = new SettingsWindow();
                    win.Owner = this;
                    win.ShowDialog();
                    break;
                default:
                    break;
            }
        }

        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }

            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness(0);
            }
        }
    }
}