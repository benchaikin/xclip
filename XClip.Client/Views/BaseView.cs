using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XClip.Client.Views
{
    public class BaseView : Window, IView
    {
        public BaseView()
        {
            Closing += OnClosing;
        }

        protected virtual void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Visibility = Visibility.Hidden;
            e.Cancel = true;
        }
        public void ShowWindow()
        {
            Dispatcher.Invoke(Show);
        }

        public void CloseWindow()
        {
            Dispatcher.Invoke(Close);
        }
    }
}
