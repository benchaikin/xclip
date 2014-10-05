using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
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

        private int _maxClipCount = 5;

        private readonly ObservableCollection<Clip> _clips;

        public ClipList()
        {
            _clips = new ObservableCollection<Clip>();
            InitializeComponent();
            _listView.ItemsSource = _clips;
            Closing += OnClosing;
        }

        public void AddClip(Clip clip)
        {
            Dispatcher.Invoke(() =>
            {
                _clips.Add(clip);
                CheckNumberOfClips();
            });
        }


        public void RemoveClip(Clip clip)
        {
            Dispatcher.Invoke(() =>
            {
                _clips.Remove(clip);
                CheckNumberOfClips();
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

        public int MaxClipCount
        {
            set
            {
                _maxClipCount = value;
                CheckNumberOfClips();
            }
        }

        private void CheckNumberOfClips()
        {
            Dispatcher.Invoke(() =>
            {
                if (_clips.Any())
                {
                    _noClipsMessage.Visibility = Visibility.Collapsed;
                    _listView.Visibility = Visibility.Visible;

                    int maxClips = _maxClipCount;
                    if (_clips.Count > maxClips)
                    {
                        int numExtra = _clips.Count - maxClips;
                        var oldClips = _clips.OrderBy(c => c.TimeStamp).Take(numExtra);

                        foreach (var oldClip in oldClips)
                        {
                            _clips.Remove(oldClip);
                        }
                    }
                }
                else
                {
                    _noClipsMessage.Visibility = Visibility.Visible;
                    _listView.Visibility = Visibility.Collapsed;
                }
            });
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

        protected override void OnClosing(object sender, CancelEventArgs e)
        {
            _taskbarIcon.Dispose();
            base.OnClosing(sender, e);
        }
    }
}
