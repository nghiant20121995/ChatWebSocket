using System;
using System.Collections.Generic;
using System.Text;

namespace ChatWebSocket.Domain.RequestModel
{
    public class MessageRequest
    {
        public string ReceiverId { get; set; }
        public string Content { get; set; }
        public bool IsGroup { get; set; }
    }
}
