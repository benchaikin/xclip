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
using XClip.Client.Core;

namespace XClip.Client.Views
{
    /// <summary>
    /// Interaction logic for Configuration.xaml
    /// </summary>
    public partial class Configuration : Window
    {
        private ClipboardAdapter _clipboardAdapter;
        private PeerManager _discoveryService;
        private Notify _notify;

        public Configuration()
        {

            InitializeComponent();
            _notify = new Notify();
            _notify.Show();
            Loaded += Configuration_Loaded;
            ClipReceiver.Instance.StartReceiving();
            ClipReceiver.Instance.MessageReceived += OnMessageReceived;
            _discoveryService = new PeerManager();
            _discoveryService.PeerListUpdated += OnPeerListUpdated;
            Closing += Configuration_Closing;
        }

        void Configuration_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _notify.Close();
        }
    

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            _notify.DisplayClipMessage(e.Message);
            _clipboardAdapter.SetClipboard(e.Message.Payload);
        }

        void OnPeerListUpdated(object sender, EventArgs e)
        {
            Dispatcher.Invoke((Action)UpdatePeerList);
        }

        private void UpdatePeerList()
        {
            _peerList.ItemsSource = _discoveryService.Peers;
        }

        void OnClipAvailable(object sender, Core.ClipAvailableEventArgs e)
        {
            _discoveryService.SendClipToPeers(e.NewClip);
        }

        void Configuration_Loaded(object sender, RoutedEventArgs e)
        {
            _clipboardAdapter = new ClipboardAdapter(new WindowInteropHelper(this).Handle);
            _clipboardAdapter.ClipAvailable += OnClipAvailable;
            _discoveryService.StartDiscovery();
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
    }
}
