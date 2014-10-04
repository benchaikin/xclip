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
        private ClipboardAdapter _clipboardAdapter;
        private IClipClient _client;
        private Notify _notify;

        public Configuration()
        {
            InitializeComponent();
            _notify = new Notify();
            _notify.Show();
            Loaded += OnLoaded;
            Closing += OnClosing;

            // Temp
            _client = new SignalRClient();
            _client.ClipReceived += OnClipReceived;
        }

        void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _notify.Close();
        }
    

        private void OnClipReceived(Clip clip)
        {
            _clipboardAdapter.SetClipboard(clip);
        }


        void OnClipAvailable(object sender, ClipAvailableEventArgs e)
        {
            _client.SendClip(e.NewClip);
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            _clipboardAdapter = new ClipboardAdapter(new WindowInteropHelper(this).Handle);
            _clipboardAdapter.ClipAvailable += OnClipAvailable; 
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream("c:\\test.bin", FileMode.Open))
            {
                Clip clip = (Clip)formatter.Deserialize(stream);
                _clipboardAdapter.SetClipboard(clip);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _client.Login(UserName.Text, Password.Password);
        }
    }
}
