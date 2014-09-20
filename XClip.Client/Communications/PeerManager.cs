using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using XClip.Client.Core;

namespace XClip.Client.Communications
{
    public class PeerManager
    {
        public event EventHandler PeerListUpdated;

        private const string _clientEndpointName = "ClientEndpoint";
        private readonly List<Peer> _peers;
        private int _pollingInterval = 6;

        public PeerManager()
        {
            _peers = new List<Peer>(); 
        }

        public void StartDiscovery()
        {
            Task.Factory.StartNew(SearchForPeers);
            var timer = new Timer(_pollingInterval * 1000);
            timer.Elapsed += OnElapsed;
            timer.Start();
        }

        public void SendClipToPeers(Clip clip)
        {
            var message = new ClipMessage()
            {
                Sender = ClipReceiver.Instance.LocalIdentity,
                Payload = clip
            };

            Parallel.ForEach(_peers, peer =>
            {
                peer.Service.SendClip(message);
            });
        }

        public IEnumerable<Peer> Peers { get { return _peers; } }

        private void SearchForPeers()
        {
            var discoveryClient = new DiscoveryClient(new UdpDiscoveryEndpoint());
            var criteria = new FindCriteria(typeof(IPeerService));
            criteria.Duration = TimeSpan.FromSeconds(5);

            var response = discoveryClient.Find(criteria);

            bool listChanged = false;

            foreach (var endpoint in response.Endpoints)
            {
                Uri uri = endpoint.Address.Uri;
                if (uri == ClipReceiver.Instance.LocalUri) continue;

                if (!_peers.Any(peer => peer.Uri == uri))
                {
                    var service = new ChannelFactory<IPeerService>(_clientEndpointName, new EndpointAddress(uri)).CreateChannel();
                    Identity remoteIdentity = service.Greet(ClipReceiver.Instance.LocalIdentity);

                    Peer newPeer = new Peer(uri, service, remoteIdentity);

                    _peers.Add(newPeer);
                    listChanged = true;
                }
            }

            int numRemoved = _peers.RemoveAll(peer => !response.Endpoints.Any(e => e.Address.Uri == peer.Uri));
            if (numRemoved > 0) listChanged = true;

            if (listChanged && PeerListUpdated != null)
            {
                PeerListUpdated(this, EventArgs.Empty);
            }
        }

        private void OnElapsed(object sender, ElapsedEventArgs e)
        {
            SearchForPeers();
        }
    }
}
