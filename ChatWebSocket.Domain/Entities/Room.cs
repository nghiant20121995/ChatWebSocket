using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatWebSocket.Domain.Entities
{
    [DynamoDBTable("Rooms")]
    public class Room : BaseEntity
    {
        public string RoomName { get; set; }
    }
}
