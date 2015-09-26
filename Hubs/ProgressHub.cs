using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.AspNet.SignalR;

//namespace Battle.WebUI.Hubs
namespace RealTimeProgressBar
{
    public class ProgressHub : Hub
    {
        public static void SendMessage(string msg, int count)
        {
            //var message = "Process completed for " + msg;
            var message = msg;
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();
            hubContext.Clients.All.sendMessage(string.Format(message), count);
        }
    }
}