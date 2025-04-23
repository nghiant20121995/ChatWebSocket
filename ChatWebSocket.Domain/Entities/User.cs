using System;
using System.Collections.Generic;
using System.Text;

namespace ChatWebSocket.Domain.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
