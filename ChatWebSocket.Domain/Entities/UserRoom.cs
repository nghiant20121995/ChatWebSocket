﻿using Amazon.DynamoDBv2.DataModel;
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
        [DynamoDBGlobalSecondaryIndexHashKey("UserId_CreatedDate")]
        public string UserId { get; set; }
        [DynamoDBGlobalSecondaryIndexRangeKey("UserId_CreatedDate")]
        public override DateTime CreatedDate { get; set; }

        [DynamoDBIgnore]
        public Room Room { get; set; }
        [DynamoDBIgnore]
        public List<User> Members { get; set; }
    }
}
