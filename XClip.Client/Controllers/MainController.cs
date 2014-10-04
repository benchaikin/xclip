using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XClip.Client.Communications;
using XClip.Client.Views;
using XClip.Core;

namespace XClip.Client.Controllers
{
    public class MainController : IMainController
    {
        private readonly ILoginView _loginView;
        private readonly IClipboardAdapter _clipboard;
        private readonly IClipClient _client;

        public MainController(ILoginView loginView, IClipboardAdapter clipboard, IClipClient client)
        {
            _loginView = loginView;
            _loginView.CredentialsSubmitted += OnCredentialsSubmitted;

            _clipboard = clipboard;
            _clipboard.ClipAvailable += OnClipAvailable;

            _client = client;
            _client.ClipReceived += OnClipReceived;
            _client.ConnectionEstablished += OnConnectionEstablished;
            _client.InvalidLogin += OnInvalidLogin;

        }

        private void OnClipAvailable(object sender, ClipAvailableEventArgs e)
        {
            throw new NotImplementedException();
        }

        #region Communications events

        public void OnInvalidLogin()
        {
            _loginView.IsProcessing = false;
            _loginView.ErrorMessage = "Invalid login. Please check credentials.";
        }

        public void OnConnectionEstablished()
        {
            _loginView.IsProcessing = false;
        }

        private void OnClipReceived(Clip clip)
        {
            throw new NotImplementedException();
        }

        #endregion

        private void OnCredentialsSubmitted()
        {
            _loginView.IsProcessing = true;
            _loginView.ErrorMessage = "";
            _client.Login(_loginView.Username, _loginView.Password);
        }


        public void StartApplication()
        {
            _loginView.Show();   
        }
    }
}
