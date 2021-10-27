using System.Security.Claims;
using ChatApplication.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplication.ViewComponents
{
    public class RoomViewComponent : ViewComponent
    {
        private IChatService _chatService;

        public RoomViewComponent(IChatService chatService)
        {
            _chatService = chatService;
        }
        public IViewComponentResult Invoke()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var chats = _chatService.GetChatsUser(userId);
            return View(chats);
        }
    }
}