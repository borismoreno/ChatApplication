using Microsoft.Extensions.Configuration;
using System;

namespace ChatApplication.Bot
{
    internal class Worker
    {
        private readonly IConfiguration _configuration;

        private QueueConfiguration _queueConfiguration;

        QueueProcess _queueProcess;
        public Worker(IConfiguration configuration)
        {
            _configuration = configuration;
            _queueConfiguration = GetConfiguration();
            _queueProcess = new QueueProcess(_queueConfiguration);
        }

        private QueueConfiguration GetConfiguration()
        {
            return new QueueConfiguration {
                Server = _configuration.GetValue<string>("Server"),
                IncomingQueue = _configuration.GetValue<string>("IncomingQueue"),
                OutgoingQueue = _configuration.GetValue<string>("OutgoingQueue"),
                AllowedCommands = _configuration.GetSection("AllowedCommands").Get<string[]>()
            };
        }

        public void DoWork()
        {
            try
            {
                _queueProcess.ReceiveMessages();
            }
            catch (Exception ex)
            {
                Console.WriteLine("There was an error:", ex.Message);
            }
        }
    }
}
