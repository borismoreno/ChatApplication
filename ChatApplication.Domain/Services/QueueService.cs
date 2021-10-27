using ChatApplication.Domain.Entities;
using ChatApplication.Domain.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatApplication.Domain.Services
{
    public class QueueService : IQueueService
    {
        IChatRepository _chatRepository;

        QueueConfiguration configuration;

        public QueueService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
            configuration = _chatRepository.GetConfiguration();
        }

        public IConnection GetConnection()
        {
            var factory = new ConnectionFactory() { HostName = configuration.Server };
            return factory.CreateConnection();
        }

        private IModel GetChanel(string queue)
        {
            var connection = GetConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(queue: queue,
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);
            return channel;
        }
        public List<Response> ReceieveMessage()
        {
            List<Response> list = new List<Response>();
            var channel = GetChanel(configuration.IncomingQueue);
            BasicGetResult result;
            do
            {
                result = channel.BasicGet(queue: configuration.IncomingQueue, autoAck: true);
                if (result != null)
                    list.Add(ProccessMessage(Encoding.UTF8.GetString(result.Body.ToArray())));
            } while (result != null);
            
            return list;
        }

        public void SendMessage(string message, bool incoming = false)
        {
            var queue = configuration.OutgoingQueue;
            if (incoming)
                queue = configuration.IncomingQueue;
            var channel = GetChanel(queue);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                routingKey: queue,
                                basicProperties: null,
                                body: body);
        }

        private Response ProccessMessage(string message)
        {
            var result = message.Split("|");
            return new Response {
                RoomName = result[0],
                Message = result[1]
            };
        }
    }
}
