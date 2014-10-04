using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin.Security;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using XClip.Core;
using XClip.Server.Models;

namespace XClip.Server.Hubs
{
    [HubName("XClipHub")]
    public class XClipHub : Hub<IXClipClient>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private const string _authHeader = "xclip-auth";

        public XClipHub()
        {
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }

        public void Login()
        {
            var user = GetUserFromToken();
            if (user != null)
            {
                Groups.Add(Context.ConnectionId, user);
                Clients.Client(Context.ConnectionId).ConnectionEstablished();
            }
            else
            {
                Clients.Client(Context.ConnectionId).InvalidLogin();
            }
        }

        public async Task SendClip(Clip clip)
        {


            await Task.Factory.StartNew(() =>
                {
                    var name = GetUserFromToken();
                    
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        Clients.OthersInGroup(name).ClipReceived(clip);
                    }
                });
        }

        private string SignInAsync(ApplicationUser user)
        {
            //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            //var identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            var cookie = FormsAuthentication.GetAuthCookie(user.UserName, false);
            return cookie.Value;
            //AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, identity);
            
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }

        private string GetUserFromToken()
        {
            var user = Context.Request.GetHttpContext().GetOwinContext().Authentication.User;
            return user.Identity.Name;
        }
    }
}