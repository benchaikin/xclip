using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace XClip.Client.Communications
{
    public class Peer
    {
        public Peer(Uri endpoint, IPeerService service, Identity identity)
        {
            Uri = endpoint;
            Service = service;
            Identity = identity;
        }

        public Uri Uri { get; protected set; }
        public IPeerService Service { get; protected set; }
        public Identity Identity { get; set; }
    }
}
