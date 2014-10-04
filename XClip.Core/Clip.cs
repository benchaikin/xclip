using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace XClip.Core
{
    [Serializable]
    public class Clip 
    {
        public string HostName { get; set; }
        public List<ClipObject> ClipObjects { get; set; }
    }
}
