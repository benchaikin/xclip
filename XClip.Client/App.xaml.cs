using System;
using System.Windows;
using XClip.Client.Controllers;
using XClip.Client.DependencyInjection;

namespace XClip.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var container = XClipContainer.CreateContainer();
            var mainController = container.GetInstance<MainController>();

            mainController.StartApplication();
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString());

            // Prevent default unhandled exception processing
            e.Handled = true;

            Environment.Exit(0);
        }
    }
}
