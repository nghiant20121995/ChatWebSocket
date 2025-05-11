using System;
using System.Collections.Generic;
using System.Text;

namespace ChatWebSocket.Domain.Response
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string SessionId { get; set; }
    }
}
