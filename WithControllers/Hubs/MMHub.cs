using Microsoft.AspNetCore.SignalR;

namespace MM.Hubs
{
    public class MMHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }   
}
