using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XClip.Client.Core;

namespace XClip.Client.Communications
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public ClipMessage Message { get; protected set; }

        public MessageReceivedEventArgs(ClipMessage message)
        {
            Message = message;
        }
    }
}
