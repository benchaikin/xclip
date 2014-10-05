using System;
using XClip.Core;

namespace XClip.Client.Views
{
    public interface IClipListView : IView
    {
        event Action ToggleConnect;
        event Action Exit;
        event Action ShowOptions;
        event Action<Clip> ClipSelected;

        int MaxClipCount { set; }
        void AddClip(Clip clip);
        bool IsConnected { set; } 
        IntPtr WindowHandle { get; }
    }
}
