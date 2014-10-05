using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XClip.Core;

namespace XClip.Client.Views
{
    public interface IClipListView : IView
    {
        event Action ToggleConnect;
        event Action Exit;
        event Action ShowOptions;
        event Action<Clip> ClipSelected;
        void AddClip(Clip clip);
        bool IsConnected { set; } 
        IntPtr WindowHandle { get; }
    }
}
