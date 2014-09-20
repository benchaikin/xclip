using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XClip.Client.Communications
{
    [Serializable]
    public class Identity
    {
        public Identity(string name, Guid id)
        {
            Name = name;
            Id = id;
        }

        public string Name { get; set; }
        public Guid Id { get; set; }
    }
}
