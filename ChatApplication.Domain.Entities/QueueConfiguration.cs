using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApplication.Domain.Entities
{
    public class QueueConfiguration
    {
        public int Id { get; set; }

        public string Server { get; set; }

        public string IncomingQueue { get; set; }

        public string OutgoingQueue { get; set; }
    }
}
