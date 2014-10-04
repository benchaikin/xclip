using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace XClip.Core
{
    public partial class ClipboardAdapter : IClipboardAdapter
    {
        public event EventHandler<ClipAvailableEventArgs> ClipAvailable;
        public bool IsListening { get; set; }

        private IntPtr _nextClipboardViewer;
        private HwndSourceHook _hook;
        private HwndSource _source;

        public void Initialize(IntPtr handle)
        {           
            _source = HwndSource.FromHwnd(handle);
            _hook = new HwndSourceHook(WndProc);
            _nextClipboardViewer = SetClipboardViewer(_source.Handle); 
            _source.AddHook(_hook);
        }

        ~ClipboardAdapter()
        {
            if (_source != null)
            {
                ChangeClipboardChain(_source.Handle, _nextClipboardViewer);
                _source.RemoveHook(_hook);
            }
        }

        public void SetClipboard(Clip clip)
        {
            Thread thread = new Thread(() =>
                {
                    IDataObject obj = new DataObjectConverter().CreateDataObject(clip);
                    _source.RemoveHook(_hook);
                    Clipboard.SetDataObject(obj);
                    _source.AddHook(_hook);
                });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // defined in winuser.h
            const int WM_DRAWCLIPBOARD = 0x308;
            const int WM_CHANGECBCHAIN = 0x030D;

            switch (msg)
            {
                case WM_DRAWCLIPBOARD:
                    var obj = System.Windows.Clipboard.GetDataObject();

                    var clip = new DataObjectConverter().CreateClip(obj);

                    if (IsListening && ClipAvailable != null)
                    {
                        Task.Factory.StartNew(() => ClipAvailable(this, new ClipAvailableEventArgs(clip)));
                    }

                    SendMessage(_nextClipboardViewer, msg, wParam, lParam);
                    break;

                case WM_CHANGECBCHAIN:
                    if (wParam == _nextClipboardViewer)
                        _nextClipboardViewer = lParam;
                    else if (_nextClipboardViewer != IntPtr.Zero)
                        SendMessage(_nextClipboardViewer, msg, wParam, lParam);
                    break;
            }

            return IntPtr.Zero;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
    }
}
