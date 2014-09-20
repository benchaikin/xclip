using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using XClip.Client.Core;

namespace XClip.Client.Communications
{
    [DataContract]
    public class ClipMessage
    {
        [DataMember]
        public Identity Sender { get; set; }

        [DataMember]
        public Clip Payload { get; set; }
    }
}
