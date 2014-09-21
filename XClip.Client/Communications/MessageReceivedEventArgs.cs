using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XClip.Core;

namespace XClip.Client.Communications
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public Clip Message { get; protected set; }

        public MessageReceivedEventArgs(Clip message)
        {
            Message = message;
        }
    }
}
