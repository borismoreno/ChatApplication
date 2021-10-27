using ChatApplication.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApplication.Domain.Services
{
    public interface IChatService
    {
        IEnumerable<Chat> GetRoomsToJoin(string userId);

        IEnumerable<Chat> GetChatsUser(string userId);

        Chat GetChat(int id);

        Task CreateRoom(string name, string userId);

        Task JoinRoom(int chatId, string userId);

        Task<Message> CreateMessage(int chatId, string message, string userId, string roomName);

        List<Response> GetMessage(string roomName);
    }
}
