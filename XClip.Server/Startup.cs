using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(XClip.Server.Startup))]
namespace XClip.Server
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }

    }
}
