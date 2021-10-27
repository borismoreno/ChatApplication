using System.Linq;
using System.Threading.Tasks;
using ChatApplication.Domain.Entities;
using ChatApplication.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplication.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private IChatService _chatService;
        public HomeController(IChatService chatService) => _chatService = chatService;

        public IActionResult Index() {
            var chats = _chatService.GetRoomsToJoin(GetUserId());
            return View(chats);
        } 

        [HttpGet("{id}")]
        public IActionResult Chat(int id)
        {
            var chat = _chatService.GetChat(id);
            if (chat == null)
                chat = new Chat();
            chat.Messages = chat.Messages.OrderBy(x => x.Timestamp).ToList();
            return View(chat);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(string name)
        {
            await _chatService.CreateRoom(name, GetUserId());
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> JoinRoom(int id)
        {
            await _chatService.JoinRoom(id, GetUserId());
            return RedirectToAction("Chat", "Home", new { id = id });
        }
    }
}