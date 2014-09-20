using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XClip.Client.Core
{
    public class ClipAvailableEventArgs : EventArgs
    {
        public Clip NewClip { get; protected set; }

        public ClipAvailableEventArgs(Clip clip)
        {
            NewClip = clip;
        }
    }
}
