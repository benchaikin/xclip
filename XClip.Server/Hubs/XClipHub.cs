using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin.Security;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using XClip.Server.Models;

namespace XClip.Server.Hubs
{
    [HubName("XClipHub")]
    public class XClipHub : Hub<IXClipClient>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public XClipHub()
        {
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }

        public async Task Login(string userName, string password)
        {
            var user = await _userManager.FindAsync(userName, password);
            if (user != null)
            {
                await SignInAsync(user);
                await Groups.Add(Context.ConnectionId, userName);
                Clients.Client(Context.ConnectionId).ConnectionEstablished();
            }
            else
            {
                Clients.Client(Context.ConnectionId).InvalidLogin();
            }
        }

        private async Task SignInAsync(ApplicationUser user)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);
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