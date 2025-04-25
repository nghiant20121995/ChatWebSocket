using System;
using System.Collections.Generic;
using System.Text;

namespace ChatWebSocket.Domain.RequestModel
{
    public class LoginReq
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
