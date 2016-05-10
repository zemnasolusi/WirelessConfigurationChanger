using System.Windows;
using GalaSoft.MvvmLight.Threading;
using System.Windows.Threading;
using log4net;
using log4net.Config;
using System;
using System.Threading;
using Microsoft.Shell;
using System.Collections.Generic;

namespace WirelessConfigurationChanger
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, ISingleInstanceApp
    {
        private const string Unique = "{30BE9D63-59FD-4034-88D1-7CF1D0D9F753}";
        private static readonly ILog log = LogManager.GetLogger(typeof(App));

        App()
        {
            InitializeComponent();

            XmlConfigurator.Configure();
            DispatcherHelper.Initialize();
        }

        [STAThread]
        static void Main()
        {
            if (SingleInstance<App>.InitializeAsFirstInstance(Unique))
            {
                App app = new App();
                MainWindow mainWindow = new MainWindow();
                app.Run(mainWindow);

                // Allow single instance code to perform cleanup operations
                SingleInstance<App>.Cleanup();
            }
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            log.Fatal("Unhandled Exception Occured", e.Exception);
            MessageBox.Show(e.Exception.ToString());
        }

        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            // Bring window to foreground
            if (this.MainWindow.WindowState == WindowState.Minimized)
                this.MainWindow.WindowState = WindowState.Normal;

            this.MainWindow.Activate();

            return true;
        }
    }
}
