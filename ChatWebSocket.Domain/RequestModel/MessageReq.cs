using System;
using System.Collections.Generic;
using System.Text;

namespace ChatWebSocket.Domain.RequestModel
{
    public class MessageRequest
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Content { get; set; }
        public bool IsGroup { get; set; }
    }
}
