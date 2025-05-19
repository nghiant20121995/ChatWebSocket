using Amazon.DynamoDBv2.DataModel;
using ChatWebSocket.Domain.RequestModel;

namespace ChatWebSocket.Domain.Entities
{
    [DynamoDBTable("Notifications")]
    public class Notification : BaseEntity
    {
        public string Message { get; set; }
        public byte Type { get; set; }
        public string UserId { get; set; }
        public bool IsRead { get; set; }
        public string DocumentId { get; set; }
        public string SenderId { get; set; }
    }
}
