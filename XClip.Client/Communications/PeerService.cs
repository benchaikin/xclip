using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using XClip.Client.Core;

namespace XClip.Client.Communications
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)]
    public class PeerService : IPeerService
    {
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;
        public Identity LocalIdentity { get; set; }

        public void SendClip(ClipMessage message)
        {
            if (MessageReceived != null)
            {
                MessageReceived(this, new MessageReceivedEventArgs(message));
            }
        }

        public Identity Greet(Identity identity)
        {
            return LocalIdentity;
        }
    }
}
