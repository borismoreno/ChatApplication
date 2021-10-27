using System.Collections.Generic;

namespace ChatApplication.Bot
{
    public class QueueConfiguration
    {
        public string Server { get; set; }

        public string IncomingQueue { get; set; }

        public string OutgoingQueue { get; set; }

        public IEnumerable<string> AllowedCommands { get; set; }
    }
}
