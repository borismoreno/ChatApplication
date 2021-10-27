using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApplication.Domain.Entities;
using ChatApplication.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.Infrastructure.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private AppDbContext _ctx;

        public ChatRepository(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task CreateError(Error error)
        {
            _ctx.Errors.Add(error);
            await _ctx.SaveChangesAsync();
        }

        public async Task CreateMessage(Message message)
        {
            _ctx.Messages.Add(message);
            await _ctx.SaveChangesAsync();
        }

        public async Task CreateRoom(string name, string userId)
        {
            var chat = new Chat
            {
                Name = name
            };

            chat.Users.Add(new ChatUser
            {
                UserId = userId,
                UserRole = UserRole.Admin
            });
            _ctx.Chats.Add(chat);
            await _ctx.SaveChangesAsync();
        }

        public Chat GetChat(int id)
        {
            return _ctx.Chats
                .Include(x => x.Messages
                    .OrderByDescending(y => y.Timestamp).Take(50))
                //.OrderBy(y => y.Timestamp).Skip(10))
                .FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Chat> GetChatsUser(string userId)
        {
            var chats = _ctx.ChatUsers
                .Include(x => x.Chat)
                .Where(x => x.UserId == userId)
                .Select(x => x.Chat)
                .ToList();
            return chats;
        }

        public IEnumerable<Command> GetCommands()
        {
            return _ctx.Commands.ToList();
        }

        public QueueConfiguration GetConfiguration()
        {
            return _ctx.QueueConfigurations
                    .FirstOrDefault();
        }

        public IEnumerable<Chat> GetRoomsToJoin(string userId)
        {
            return _ctx.Chats
                .Include(x => x.Users)
                .Where(x => !x.Users
                    .Any(y => y.UserId == userId))
                .ToList();
        }

        public async Task JoinRoom(int chatId, string userId)
        {
            var chatUser = new ChatUser
            {
                ChatId = chatId,
                UserId = userId,
                UserRole = UserRole.Member
            };
            _ctx.ChatUsers.Add(chatUser);
            await _ctx.SaveChangesAsync();
        }
    }
}
