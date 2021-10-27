using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ChatApplication.Domain.Entities
{
    public class User : IdentityUser
    {
        public ICollection<ChatUser> Chats { get; set; }
    }
}
