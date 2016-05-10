using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using WirelessConfigurationChanger.Message;

namespace WirelessConfigurationChanger.View
{
    /// <summary>
    /// Description for SettingsWindow.
    /// </summary>
    public partial class SettingsWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the SettingsWindow class.
        /// </summary>
        public SettingsWindow()
        {
            InitializeComponent();

            Messenger.Default.Register<WindowCloseMessage>(this, "SettingsWindow", (msg) =>
            {
                this.Close();
            });
        }
    }
}