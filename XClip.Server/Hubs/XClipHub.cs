using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using System.Web;
using XClip.Core;

namespace XClip.Server.Hubs
{
    [HubName("XClipHub")]
    [Authorize]
    public class XClipHub : Hub<IXClipClient>
    {
        public async Task Login()
        {
            await Task.Factory.StartNew(() =>
                {
                    var user = Context.User.Identity.Name;
                    if (user != null)
                    {
                        Groups.Add(Context.ConnectionId, user);
                        Clients.Client(Context.ConnectionId).ConnectionEstablished();
                    }
                });
        }

        public async Task SendClip(Clip clip)
        {
            await Task.Factory.StartNew(() =>
                {
                    var name = Context.User.Identity.Name;
                    
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        Clients.OthersInGroup(name).ClipReceived(clip);
                    }
                });
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }

    }
}