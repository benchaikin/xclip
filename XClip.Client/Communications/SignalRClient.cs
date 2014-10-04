using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XClip.Core;
using XClip.Client.Properties;
using System.Net;

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
        private const string _authHeader = "xclip-auth";
        private bool _isConnected = false;
        private string _authToken = null;


        public void Login(string username, string password)
        {
            var authHelper = new AuthorizationClient();
            string token = authHelper.AuthenticateUser(username, password);

            // Create connection and proxy
            _connection = new HubConnection(Settings.Default.ServerUrl + "/signalr");
            _connection.CookieContainer = new CookieContainer();
            _connection.CookieContainer.Add(new Cookie(".AspNet.ApplicationCookie", token, "/", "localhost"));
            _connection.Closed += OnConnectionClosed;
            _server = _connection.CreateHubProxy(_hubName);

            // Subscribe to messages
            _server.On("ConnectionEstablished", OnConnectionEstablished);
            _server.On("InvalidLogin", OnInvalidLogin);
            _server.On("ClipReceived", (Clip clip) => OnClipReceived(clip));

            // Start listening
            _connection.Start().Wait();
        
            _server.Invoke("Login");
            
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
            _isConnected = true;
            if (ConnectionEstablished != null)
            {
                ConnectionEstablished();
            }
        }

        private void OnConnectionClosed()
        {
            _authToken = null;
            _isConnected = false;
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
