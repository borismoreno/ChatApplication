using Microsoft.AspNetCore.SignalR;

namespace ChatApplication.Hubs
{
    public class ChatHub : Hub
    {
        public string GetConnectionId() =>
            Context.ConnectionId;
    }
}