using Microsoft.AspNetCore.SignalR;

namespace ChatApplication.Domain.Hubs
{
    public class ChatHub : Hub
    {
        public string GetConnectionId() =>
            Context.ConnectionId;
    }
}
