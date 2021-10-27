using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApplication.Domain.Entities
{
    public class ChatUser
    {
        public string UserId { get; set; }

        public User User { get; set; }

        public int ChatId { get; set; }

        public Chat Chat { get; set; }

        public UserRole UserRole { get; set; }
    }
}
