using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using XClip.Core;

namespace XClip.Client.Views
{
    /// <summary>
    /// Interaction logic for ClipList.xaml
    /// </summary>
    public partial class ClipList : BaseView, IClipListView
    {
        private ObservableCollection<Clip> _clips = new ObservableCollection<Clip>();

        public ClipList()
        {
            InitializeComponent();
            _listView.ItemsSource = _clips;
        }

        public void AddClip(Clip clip)
        {
            Dispatcher.Invoke(() =>
            {
                _clips.Add(clip);
                _taskbarIcon.TrayPopup.Visibility = System.Windows.Visibility.Visible;
            });
        }

        public IntPtr WindowHandle
        {
            get { return new WindowInteropHelper(this).Handle; }
        }
    }
}
