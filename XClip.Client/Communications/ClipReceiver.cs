using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.Text;

namespace XClip.Client.Communications
{
    public class ClipReceiver
    {
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        private readonly ServiceHost _host;
        private readonly PeerService _service;
        private static readonly ClipReceiver _instance = new ClipReceiver();

        public static ClipReceiver Instance { get { return _instance; } }

        private ClipReceiver()
        {
            _service = new PeerService();

            _host = new ServiceHost(_service);
            _service.LocalIdentity = new Identity(_host.Description.Endpoints.First().Address.Uri.ToString(), Guid.NewGuid());
            _service.MessageReceived += OnMessageReceived;
        }

        public Identity LocalIdentity
        {
            get { return _service.LocalIdentity; }
            set { _service.LocalIdentity = value; }
        }

        public Uri LocalUri
        {
            get { return _host.Description.Endpoints.First().Address.Uri; }
        }

        public void StartReceiving()
        {
            _host.Open();
        }

        public void StopReceiving()
        {
            _host.Close();
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (MessageReceived != null)
            {
                MessageReceived(this, new MessageReceivedEventArgs(e.Message));
            }
        }
    }
}
