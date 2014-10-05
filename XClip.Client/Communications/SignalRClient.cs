using Microsoft.AspNet.SignalR.Client;
using System;
using System.Net;
using XClip.Client.Properties;
using XClip.Core;

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
        private bool _isConnected = false;

        private readonly IAuthorizationClient _authorizationClient;

        public SignalRClient(IAuthorizationClient authorizationClient)
        {
            _authorizationClient = authorizationClient;
        }

        public void Login(string username, string password)
        {
            string token = null;

            if (_authorizationClient.AuthenticateUser(username, password, out token))
            {
                var serverUri = new Uri(Settings.Default.ServerUrl);
                // Create connection and proxy
                _connection = new HubConnection(Settings.Default.ServerUrl + "/signalr");
                _connection.CookieContainer = new CookieContainer();
                _connection.CookieContainer.Add(new Cookie(AuthorizationClient.AuthCookieName, token, "/", serverUri.Host));
                _connection.Closed += OnConnectionClosed;
                _server = _connection.CreateHubProxy(_hubName);

                // Subscribe to messages
                _server.On("ConnectionEstablished", OnConnectionEstablished);
                _server.On("ClipReceived", (Clip clip) => OnClipReceived(clip));

                // Start listening
                _connection.Start().Wait();

                _server.Invoke("Login");
            }
            else
            {
                if (InvalidLogin != null)
                {
                    InvalidLogin();
                }
            }
            
        }

        public void Disconnect()
        {
            _isConnected = false;
            _connection.Stop();
        }

        public bool IsConnected
        {
            get { return _isConnected; }
        }

        public void SendClip(Clip clip)
        {
            if (_isConnected)
            {
                _server.Invoke("SendClip", clip);
            }
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
            _isConnected = true;
            if (ConnectionEstablished != null)
            {
                ConnectionEstablished();
            }
        }

        private void OnConnectionClosed()
        {
            _isConnected = false;
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
