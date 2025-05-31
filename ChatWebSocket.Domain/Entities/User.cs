using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatWebSocket.Domain.Entities
{
    [DynamoDBTable("Users")]
    public class User : BaseEntity
    {
        [DynamoDBHashKey]
        public override string Id { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string PhoneNumber { get; set; }
        [DynamoDBGlobalSecondaryIndexHashKey("User_Email")]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
