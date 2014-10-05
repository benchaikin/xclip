using SimpleInjector;
using XClip.Client.Communications;
using XClip.Client.Controllers;
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
            container.Register<IDataObjectConverter, DataObjectConverter>();
            container.Register<IClipClient, SignalRClient>();

            container.Register<IMainController, MainController>();
            container.Register<IOptionsController, OptionsController>();

            container.Register<ILoginView, Login>();
            container.Register<IClipListView, ClipList>();
            container.Register<IOptionsView, Options>();

            return container;
        }
    }
}
