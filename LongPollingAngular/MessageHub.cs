using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SharedLibraries;

namespace LongPollingAngular
{
    public class MessageHub : Hub
    {
        IPostgreSQLService _postgreSQLService;

        public MessageHub(IPostgreSQLService postgreSQLService)
        {
            _postgreSQLService = postgreSQLService;
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public override async Task OnConnectedAsync()
        {
            _postgreSQLService.AddMessage("Client connected", DateTime.Now);
        }
    }
}

