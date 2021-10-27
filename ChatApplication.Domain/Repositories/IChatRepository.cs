using ChatApplication.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApplication.Domain.Repositories
{
    public interface IChatRepository
    {
        IEnumerable<Chat> GetRoomsToJoin(string userId);

        IEnumerable<Chat> GetChatsUser(string userId);

        IEnumerable<Command> GetCommands();

        Chat GetChat(int id);

        Task CreateRoom(string name, string userId);

        Task JoinRoom(int chatId, string userId);

        Task CreateMessage(Message message);

        QueueConfiguration GetConfiguration();

        Task CreateError(Error error);
    }
}
