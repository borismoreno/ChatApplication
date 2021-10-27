using System;
using System.Threading.Tasks;
using ChatApplication.Domain.Hubs;
using ChatApplication.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatApplication.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ChatController : BaseController
    {
        private IHubContext<ChatHub> _chat;
        private IChatService _chatService;

        public ChatController(
            IHubContext<ChatHub> chat,
            IChatService chatService
        )
        {
            _chat = chat;
            _chatService = chatService;
        }

        [HttpPost("[action]/{connectionId}/{roomName}")]
        public async Task<IActionResult> JoinRoom(string connectionId, string roomName)
        {
            await _chat.Groups.AddToGroupAsync(connectionId, roomName);
            await CheckQueueMessage(roomName);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendMessage(
            int chatId,
            string message, 
            string roomName
        )
        {
            await CheckQueueMessage(roomName);
            var Message = await _chatService.CreateMessage(chatId, message, GetUserName(), roomName);
            await _chat.Clients.Group(roomName)
                .SendAsync("RecieveMessage", new
                {
                    Text = Message.Text,
                    Name = Message.Name,
                    Timestamp = Message.Timestamp.ToString("MM/dd/yyyy HH:mm:ss")
                });
            return Ok();
        }

        private async Task CheckQueueMessage(string roomName)
        {
            var respuesta = _chatService.GetMessage(roomName);
            if (respuesta != null)
                foreach (var result in respuesta)
                {
                    await _chat.Clients.Group(result.RoomName)
                    .SendAsync("RecieveMessage", new
                    {
                        Text = result.Message,
                        Name = "Bot",
                        Timestamp = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")
                    });
                }
        }
    }
}