using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XClip.Client.Core;
using XClip.Client.Properties;

namespace XClip.Client.Communications
{
    public class SignalRClient : IClipClient
    {
        public event Action ConnectionEstablished;
        public event Action InvalidLogin;
        public event Action<Clip> ClipReceived;

        private HubConnection _connection;
        private IHubProxy _server;
        private const string _hubName = "XClipHub";

        public void Connect()
        {
            // Create connection and proxy
            _connection = new HubConnection(Settings.Default.ServerUrl);
            _server = _connection.CreateHubProxy(_hubName);

            // Subscribe to messages
            _server.On("ConnectionEstablished", OnConnectionEstablished);
            _server.On("InvalidLogin", OnInvalidLogin);
            _server.On("ClipReceived", clip => OnClipReceived(clip));

            // Start listening
            _connection.Start().Wait();
        }

        public void Login(string username, string password)
        {
            _server.Invoke("Login", username, password);
        }

        public void SendClip(Clip clip)
        {
            _server.Invoke("SendClip", clip);
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
            }
        }

        private void OnConnectionEstablished()
        {
            if (ConnectionEstablished != null)
            {
                ConnectionEstablished();
            }
        }

        private void OnInvalidLogin()
        {
            if (InvalidLogin != null)
            {
                InvalidLogin();
            }
        }

        private void OnClipReceived(Clip clip)
        {
            if (ClipReceived != null)
            {
                ClipReceived(clip);
            }
        }
    }
}
