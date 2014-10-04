﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XClip.Core;

namespace XClip.Client.Views
{
    public interface IClipListView : IView
    {
        event Action<Clip> ClipSelected;
        void AddClip(Clip clip);
        IntPtr WindowHandle { get; }
    }
}