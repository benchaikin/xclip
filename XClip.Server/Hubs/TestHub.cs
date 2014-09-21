using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XClip.Server.Hubs
{
    [HubName("TestHub")]
    public class TestHub : Hub<ITestHub>
    {
        public void Echo(string message)
        {
            Clients.All.ReceiveMessage(message);
        }
    }
}