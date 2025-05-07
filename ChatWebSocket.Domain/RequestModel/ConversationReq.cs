using System;
using System.Collections.Generic;
using System.Text;

namespace ChatWebSocket.Domain.RequestModel
{

    public class ConversationRequest
    {
        public string UserId { get; set; }
        public int Limit { get; set; } = 20;
        public int Offset { get; set; } = 0;
    }
}
