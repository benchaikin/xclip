using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using XClip.Client.Controls;
using XClip.Core;

namespace XClip.Client.Views
{
    /// <summary>
    /// Interaction logic for ClipList.xaml
    /// </summary>
    public partial class ClipList : BaseView, IClipListView
    {
        public event Action<Clip> ClipSelected;
        public event Action ToggleConnect;
        public event Action Exit;
        public event Action ShowOptions;

        private ObservableCollection<Clip> _clips = new ObservableCollection<Clip>();

        public ClipList()
        {
            InitializeComponent();
            _listView.ItemsSource = _clips;
            Closing += OnClosing;
        }

        public void AddClip(Clip clip)
        {
            Dispatcher.Invoke(() =>
            {
                _clips.Add(clip);
                _noClipsMessage.Visibility = Visibility.Collapsed;
                _listView.Visibility = Visibility.Visible;
            });
        }

        public IntPtr WindowHandle
        {
            get { return new WindowInteropHelper(this).Handle; }
        }

        public bool IsConnected
        {
            set
            {
                Dispatcher.Invoke(() =>
                {
                    if (value)
                    {
                        _connect.IsEnabled = false;
                        _disconnect.IsEnabled = true;
                    }
                    else
                    {
                        _connect.IsEnabled = true;
                        _disconnect.IsEnabled = false;
                    }
                });
            }
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ClipSelected != null)
            {
                var clip = _listView.SelectedItem as Clip;
                ClipSelected(clip);
            }

            _taskbarIcon.TrayPopupResolved.IsOpen = false;
        }

        private void OnToggleConnectClicked(object sender, RoutedEventArgs e)
        {
            if (ToggleConnect != null)
            {
                ToggleConnect();
            }
        }

        private void OnExitClicked(object sender, RoutedEventArgs e)
        {
            if (Exit != null)
            {
                Exit();
            }
        }

        private void OnOptionsClicked(object sender, RoutedEventArgs e)
        {
            if (ShowOptions != null)
            {
                ShowOptions();
            }
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _taskbarIcon.Dispose();
        }
    }
}
