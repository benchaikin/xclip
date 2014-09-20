using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using XClip.Client.Communications;

namespace XClip.Client.Views
{
    /// <summary>
    /// Interaction logic for Notify.xaml
    /// </summary>
    public partial class Notify : Window
    {
        private Timer _timer;
        private int _visibleFor = 3;
        private Storyboard _storyboard;
        public Notify()
        {
            Opacity = 0;
            InitializeComponent();
        }

        public void DisplayClipMessage(ClipMessage message)
        {
            SetLocation();
            _timer = new Timer(OnTimerElapsed, null, _visibleFor * 1000, Timeout.Infinite);

            Dispatcher.Invoke((Action)(() =>
            {
                _clipMessage.DataContext = message;

                DoubleAnimation fadeIn = new DoubleAnimation();
                fadeIn.From = Opacity;
                fadeIn.To = 1d;
                fadeIn.Duration = new Duration(new TimeSpan(0, 0, 1));

                Storyboard.SetTargetName(fadeIn, this.Name);
                Storyboard.SetTargetProperty(fadeIn, new PropertyPath
                    (UIElement.OpacityProperty));

                _storyboard = new Storyboard();
                _storyboard.Children.Add(fadeIn);
                _storyboard.Begin(this, true);
            }));
        }

        public void OnTimerElapsed(object state)
        {
            Dispatcher.Invoke((Action)(() =>
            {
                DoubleAnimation fadeOut = new DoubleAnimation();
                fadeOut.From = Opacity;
                fadeOut.To = 0;
                fadeOut.Duration = new Duration(new TimeSpan(0, 0, 1));

                Storyboard.SetTargetName(fadeOut, this.Name);
                Storyboard.SetTargetProperty(fadeOut, new PropertyPath
                    (UIElement.OpacityProperty));

                _storyboard = new Storyboard();
                _storyboard.Children.Add(fadeOut);
                _storyboard.Begin(this, true);
            }));
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (_storyboard != null) _storyboard.Stop(this);
            Opacity = 1;
            if (_timer != null) _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (_timer != null) _timer.Change(_visibleFor * 1000, Timeout.Infinite);
        }

        private void SetLocation()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
            {
                var workingArea = SystemParameters.WorkArea;
                var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
                var corner = transform.Transform(new Point(workingArea.Right, workingArea.Bottom));

                this.Left = corner.X - this.ActualWidth - 100;
                this.Top = corner.Y - this.ActualHeight;
            }));
        }
    }

}
