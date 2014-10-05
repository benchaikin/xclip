using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using XClip.Client.Properties;

namespace XClip.Client.Views
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : BaseView, IOptionsView
    {
        public event Action Save;
        public event Action Cancel;

        public Options()
        {
            InitializeComponent();
        }

        public Settings Model
        {
            get
            {
                return DataContext as Settings;
            }
            set
            {
                DataContext = value;
            }
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            if (Save != null)
            {
                Save();
            }
        }

        protected override void OnClosing(object sender, CancelEventArgs e)
        {
            if (Cancel != null)
            {
                Cancel();
            }
            
            base.OnClosing(sender, e);
        }
    }
}
