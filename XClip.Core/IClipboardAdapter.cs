using System;

namespace XClip.Core
{
    public interface IClipboardAdapter
    {
        void Initialize(IntPtr windowHandle);
        event EventHandler<ClipAvailableEventArgs> ClipAvailable;
        bool IsListening { get; set; }
        void SetClipboard(Clip clip);
    }
}
