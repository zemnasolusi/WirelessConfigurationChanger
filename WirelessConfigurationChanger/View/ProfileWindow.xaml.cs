using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using WirelessConfigurationChanger.Message;

namespace WirelessConfigurationChanger.View
{
    /// <summary>
    /// Description for ProfileWindow.
    /// </summary>
    public partial class ProfileWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the ProfileWindow class.
        /// </summary>
        public ProfileWindow()
        {
            InitializeComponent();

            Messenger.Default.Register<WindowCloseMessage>(this, "ProfileWindow", (msg) =>
            {
                this.Close();
            });
        }
    }
}