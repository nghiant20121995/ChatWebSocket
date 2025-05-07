using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatWebSocket.Domain.Entities
{
    [DynamoDBTable("UserRooms")]
    public class UserRoom : BaseEntity
    {
        [DynamoDBHashKey("RoomId")]
        public override string Id { get; set; }
        [DynamoDBRangeKey]
        [DynamoDBGlobalSecondaryIndexHashKey("UserId")]
        public string UserId { get; set; }
        [DynamoDBGlobalSecondaryIndexRangeKey("CreatedDate")]
        public override DateTime CreatedDate { get; set; }
    }
}
