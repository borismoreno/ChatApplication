using ChatApplication.Domain.Entities;
using ChatApplication.Domain.Hubs;
using ChatApplication.Domain.Repositories;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChatApplication.Domain.Services
{
    public class ChatService : IChatService
    {
        private IChatRepository _chatRepository;

        private IQueueService _queueService;

        public ChatService(IChatRepository chatRepository, IQueueService queueService)
        {
            _chatRepository = chatRepository;
            _queueService = queueService;
        }
        
        public async Task<Message> CreateMessage(int chatId, string message, string userId, string roomName)
        {
            try
            {
                var Message = new Message
                {
                    ChatId = chatId,
                    Text = message,
                    Name = userId,
                    Timestamp = DateTime.Now
                };
                if (!CheckCommand(message))
                    await _chatRepository.CreateMessage(Message);
                else
                    _queueService.SendMessage(roomName + "|" + message);
                return Message;
            }
            catch (Exception ex)
            {
                CreateError(ex);
                return null;
            }
            
        }

        public async Task CreateRoom(string name, string userId)
        {
            try
            {
                await _chatRepository.CreateRoom(name, userId);
            }
            catch (Exception ex)
            {
                CreateError(ex);
            }
            
        }

        public Chat GetChat(int id)
        {
            try
            {
                return _chatRepository.GetChat(id);
            }
            catch (Exception ex)
            {
                CreateError(ex);
                return null;
            }
            
        }

        public IEnumerable<Chat> GetChatsUser(string userId)
        {
            try
            {
                return _chatRepository.GetChatsUser(userId);
            }
            catch (Exception ex)
            {
                CreateError(ex);
                return null;
            }
            
        }

        public List<Response> GetMessage(string roomName)
        {
            try
            {
                List<Response> responses = new List<Response>();
                var results = _queueService.ReceieveMessage();
                if (results != null)
                {
                    foreach (var result in results)
                    {
                        if (result.RoomName == roomName)
                            responses.Add(result);
                        else
                            _queueService.SendMessage(result.RoomName + "|" + result.Message, true);
                    }
                    return responses;
                }
                return null;
            }
            catch (Exception ex)
            {
                CreateError(ex);
                return null;
            }
            
        }

        public IEnumerable<Chat> GetRoomsToJoin(string userId)
        {
            try
            {
                return _chatRepository.GetRoomsToJoin(userId);
            }
            catch (Exception ex)
            {
                CreateError(ex);
                return null;
            }
        }

        public async Task JoinRoom(int chatId, string userId)
        {
            try
            {
                await _chatRepository.JoinRoom(chatId, userId);
            }
            catch (Exception ex)
            {
                CreateError(ex);
            }
        }

        private bool CheckCommand(string message)
        {
            var commands = _chatRepository.GetCommands();
            foreach (var command in commands)
            {
                if (Regex.IsMatch(message, $"^{command.Name}="))
                    return true;
            }
            return false;
        }

        private void CreateError(Exception exception)
        {
            _chatRepository.CreateError(new Error
            {
                Source = exception.Source,
                Message = exception.Message,
                StackTrace = exception.StackTrace
            });
        }
    }
}
