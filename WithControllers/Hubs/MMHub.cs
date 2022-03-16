using Microsoft.AspNetCore.SignalR;

namespace MM.Hubs
{
    public class MMHub : Hub
    {
        public async Task UpdatePage()
        {
            await Clients.All.SendAsync("ReceiveUpdate");
        }
    }
}