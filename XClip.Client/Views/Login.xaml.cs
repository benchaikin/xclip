using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace XClip.Client.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : BaseView, ILoginView
    {
        public event Action CredentialsSubmitted; 

        public Login()
        {
            InitializeComponent();
        }

        public string Username
        {
            get { return Dispatcher.Invoke(() => _username.Text); }
            set
            {
                Dispatcher.Invoke(() => 
                {
                    _username.Text = value;
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        _password.Focus();
                    }
                }); 
            }
        }

        public string Password
        {
            get { return Dispatcher.Invoke(() => _password.Password); }
        }

        public string ErrorMessage
        {
            set 
            {
                Dispatcher.Invoke(() =>
                {
                    _errorMessage.Text = value;

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        _errorMessage.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        _errorMessage.Visibility = Visibility.Collapsed;
                    }
                });
            }
        }

        public string RegisterUrl
        {
            set 
            { 
                Dispatcher.Invoke(() => _registerLink.NavigateUri = new Uri(value)); 
            }
        }

        public bool IsProcessing
        {
            set 
            {
                Dispatcher.Invoke(() =>
                {
                    if (value)
                    {
                        _password.IsEnabled = false;
                        _username.IsEnabled = false;
                        _connect.IsEnabled = false;
                        _registerLink.IsEnabled = false;
                        _progressBar.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        _password.IsEnabled = true;
                        _username.IsEnabled = true;
                        _connect.IsEnabled = true;
                        _registerLink.IsEnabled = true;
                        _progressBar.Visibility = Visibility.Collapsed;
                    }
                });
            }
        }
        private void OnConnectClick(object sender, RoutedEventArgs e)
        {
            if (CredentialsSubmitted != null)
            {
                Task.Factory.StartNew(CredentialsSubmitted);
            }
        }

        private void OnRegisterClick(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.ToString());
        }
    }
}
