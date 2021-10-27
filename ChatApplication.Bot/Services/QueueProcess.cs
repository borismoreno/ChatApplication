using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChatApplication.Bot
{
    public class QueueProcess
    {
        private QueueConfiguration _queueConfiguration;

        private const string MSG_OK = "{0}|{1} quote is ${2} per share.";
        private const string MSG_EMPTY = "No results for the code.";
        private const string MSG_MALFORMED = "Wrong query format.";

        Api api = new Api();
        public QueueProcess(QueueConfiguration queueConfiguration)
        {
            _queueConfiguration = queueConfiguration;
        }
        public IConnection GetConnection()
        {
            try
            {
                return new ConnectionFactory()
                {
                    HostName = _queueConfiguration.Server
                }.CreateConnection();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error conecting server {_queueConfiguration.Server}, error: {ex.Message}");
                Console.ResetColor();
                throw;
            }
        }

        private IModel GetChanel(string queue)
        {
            var channel = GetConnection().CreateModel();
            channel.QueueDeclare(queue: queue,
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);
            return channel;
        }

        public void ReceiveMessages()
        {
            var channel = GetChanel(_queueConfiguration.IncomingQueue);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;
            channel.BasicConsume(queue: _queueConfiguration.IncomingQueue, autoAck: true, consumer: consumer);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Chequing {_queueConfiguration.IncomingQueue} queue at {_queueConfiguration.Server}");
            Console.ResetColor();
        }

        private async void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            await ProcessMessage(message);
            Console.WriteLine($"Message received {message}");
        }

        public async Task<string> ProcessMessage(string message)
        {
            try
            {
                var splitString = message.Split('|');
                if (splitString.Length != 2)
                {
                    SendMessage(MSG_MALFORMED);
                    return MSG_MALFORMED;
                }
                foreach (var command in _queueConfiguration.AllowedCommands)
                {
                    if (Regex.IsMatch(splitString[1], $"^{command}="))
                    {
                        var res = await api.Consume(splitString[1].Split('=')[1]);

                        return SendResult(splitString[0], res);
                    }
                }
                SendMessage($"{splitString[0]}|{MSG_MALFORMED}");
                return MSG_MALFORMED;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:", ex);
                throw;
            }
        }

        private string SendResult(string roomName, Csv csv)
        {
            if (csv.Close == "N/D")
            {
                SendMessage($"{roomName}|{MSG_EMPTY}");
                return MSG_EMPTY;
            }
            else
            {
                SendMessage(string.Format(MSG_OK, roomName, csv.Symbol, csv.Close));
                return MSG_OK;
            }
        }

        public void SendMessage(string message)
        {
            var channel = GetChanel(_queueConfiguration.OutgoingQueue);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                routingKey: _queueConfiguration.OutgoingQueue,
                                basicProperties: null,
                                body: body);
            Console.WriteLine($"Message sent {message}");
        }
    }
}
