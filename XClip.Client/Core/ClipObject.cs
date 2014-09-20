using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace XClip.Client.Core
{
    [Serializable]
    public class ClipObject
    {
        public ClipObject(string format, byte[] data, bool isStream)
        {
            Format = format;
            Data = data;
            IsStream = isStream;
        }

        public string Format { get; protected set; }
        public byte[] Data { get; protected set; }
        public bool IsStream { get; protected set; }
    }
}
