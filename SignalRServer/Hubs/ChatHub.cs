using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace SignalRServer.Hubs
{
    public class ChatHub: Hub
    {
        public override Task OnConnectedAsync()  // get fired when a new conneciton is on this hub
        {
            Console.WriteLine("--> Connection opened: "+ Context.ConnectionId);
            Clients.Client(Context.ConnectionId).SendAsync("ReceiveConnID", Context.ConnectionId); // connection send back connID to client, "Context" object is hub callter context
            return base.OnConnectedAsync();
        }

        public async Task SendMessageAsync(string message)  // this method is called by client
        {
            var routeObj = JsonConvert.DeserializeObject<dynamic>(message);
            Console.WriteLine("To: " + routeObj.To.ToString());
            Console.WriteLine("Message Received on: " + Context.ConnectionId);
            if (routeObj.To.ToString() == string.Empty)
            {
                Console.WriteLine("Broadcast");
                await Clients.All.SendAsync("ReceiveMessage", message);
            }
            else
            {
                string toClient = routeObj.To;
                Console.WriteLine("Targeted on: " + toClient);

                await Clients.Client(toClient).SendAsync("ReceiveMessage", message);
            }
        }
    }
}