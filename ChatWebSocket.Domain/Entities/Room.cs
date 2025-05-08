using Amazon.DynamoDBv2.DataModel;
using ChatWebSocket.Domain.RequestModel;

namespace ChatWebSocket.Domain.Entities
{
    [DynamoDBTable("Rooms")]
    public class Room : BaseEntity
    {
        public string RoomName { get; set; }
        public MessageRequest LatestMessage { get; set; }
    }
}
