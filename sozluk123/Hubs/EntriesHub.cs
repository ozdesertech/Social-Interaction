using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace sozluk123.Models
{
    [HubName("entriesHub")]
    public class EntriesHub : Hub
    {
        public static void BroadcastData()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<EntriesHub>();
            
            context.Clients.All.refreshEmployeeData();
        }
    }
}