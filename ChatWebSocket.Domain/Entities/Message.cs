using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatWebSocket.Domain.Entities
{
    [DynamoDBTable("Messages")]
    public class Message : BaseEntity
    {
        [DynamoDBHashKey]
        public override string Id { get; set; }
        public string ReceiverId { get; set; }
        public string SenderId { get; set; }
        public string Content { get; set; }
        public string ParentId { get; set; }
        [DynamoDBGlobalSecondaryIndexHashKey("Message_Room_CreatedDate")]
        public string RoomId {  get; set; }
        [DynamoDBGlobalSecondaryIndexRangeKey("Message_Room_CreatedDate")]
        public override DateTime CreatedDate { get; set; }
    }
}
