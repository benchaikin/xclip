﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XClip.Client.Communications;
using XClip.Client.Properties;
using XClip.Client.Views;
using XClip.Core;

namespace XClip.Client.Controllers
{
    public class MainController : IMainController
    {
        private readonly ILoginView _loginView;
        private readonly IClipboardAdapter _clipboard;
        private readonly IClipClient _client;
        private readonly IClipListView _clipListView;

        public MainController(ILoginView loginView, IClipListView clipListView, IClipboardAdapter clipboard, IClipClient client)
        {
            _loginView = loginView;
            _loginView.RegisterUrl = Settings.Default.ServerUrl;
            _loginView.CredentialsSubmitted += OnCredentialsSubmitted;

            _clipListView = clipListView;
            _clipListView.ClipSelected += OnClipSelected;
            _clipListView.Exit += OnExit;
            _clipListView.ToggleConnect += OnToggleConnect;
            _clipListView.ShowOptions += OnShowOptions;
            _clipListView.ShowWindow();

            _clipboard = clipboard;
            _clipboard.ClipAvailable += OnClipAvailable; 
            _clipboard.Initialize(_clipListView.WindowHandle);
            _clipboard.IsListening = true;

            _client = client;
            _client.ClipReceived += OnClipReceived;
            _client.ConnectionEstablished += OnConnectionEstablished;
            _client.InvalidLogin += OnInvalidLogin;

        }

        #region ClipList events
        private void OnToggleConnect()
        {
            if (_client.IsConnected)
            {
                _client.Disconnect();
            }
            else
            {
                _loginView.ShowWindow();
            }
        }

        private void OnShowOptions()
        {
            throw new NotImplementedException();
        }

        private void OnExit()
        {
            Application.Current.Shutdown();
        }

        private void OnClipSelected(Clip clip)
        {
            _clipboard.SetClipboard(clip);
        }

    #endregion

        private void OnClipAvailable(object sender, ClipAvailableEventArgs e)
        {
            _clipListView.AddClip(e.NewClip);
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
            _loginView.CloseWindow();
            _clipListView.IsConnected = true;
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

        }
    }
}
