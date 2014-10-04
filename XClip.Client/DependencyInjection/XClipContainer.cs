using SimpleInjector;
using XClip.Client.Communications;
using XClip.Client.Views;
using XClip.Core;

namespace XClip.Client.DependencyInjection
{
    public static class XClipContainer
    {
        public static Container CreateContainer()
        {
            Container container = new Container();

            // Register all services here
            container.Register<IAuthorizationClient, AuthorizationClient>();
            container.Register<IClipboardAdapter, ClipboardAdapter>();
            container.Register<IClipClient, SignalRClient>();

            container.Register<ILoginView, Login>();
            container.Register<IClipListView, ClipList>();

            return container;
        }
    }
}
