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
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }

    public class MessageFilterRequest
    {
        public string RoomId { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Limit { get; set; } = 20;
        public int Offset { get; set; } = 0;
    }
}
