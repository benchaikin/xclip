﻿using System;
using XClip.Core;

namespace XClip.Client.Communications
{
    public interface IClipClient : IDisposable
    {
        event Action ConnectionEstablished;
        event Action InvalidLogin;
        event Action<Clip> ClipReceived;
     
        void Login(string username, string password);
        void Disconnect();
        void SendClip(Clip clip);
        bool IsConnected { get; }
    }
}
