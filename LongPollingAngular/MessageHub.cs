using Microsoft.AspNetCore.SignalR;

namespace LongPollingAngular
{
    public class MessageHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}

