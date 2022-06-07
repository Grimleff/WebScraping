using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace WebScrapingWorker.Hubs
{
    public class NotifHub : Hub
    {
        private readonly ILogger<NotifHub> _logger;

        public NotifHub(ILogger<NotifHub> logger)
        {
            _logger = logger;
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}