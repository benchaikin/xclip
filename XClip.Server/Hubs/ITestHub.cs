using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XClip.Server.Hubs
{
    public interface ITestHub
    {
        void ReceiveMessage(string message);
    }
}
