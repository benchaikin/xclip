using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XClip.Client.Communications;
using XClip.Core;

namespace XClip.Client.Views
{
    /// <summary>
    /// Interaction logic for Configuration.xaml
    /// </summary>
    public partial class Configuration : Window
    {

        private Notify _notify;

        public Configuration()
        {
            InitializeComponent();
            _notify = new Notify();
            _notify.Show();
        }

        void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _notify.Close();
        }
    

    }
}
