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
        private ObservableCollection<Clip> _clips = new ObservableCollection<Clip>();
        public event Action<Clip> ClipSelected;

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
            });
        }

        public void NotifyClip(Clip clip)
        {

        }

        public IntPtr WindowHandle
        {
            get { return new WindowInteropHelper(this).Handle; }
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ClipSelected != null)
            {
                var clip = _listView.SelectedItem as Clip;
                ClipSelected(clip);
            }
        }
    }
}
