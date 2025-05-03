using Amazon.DynamoDBv2.DataModel;
using ChatWebSocket.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatWebSocket.Domain.Entities
{
    public abstract class BaseEntity : ICreatedDate, IModifiedDate
    {
        [DynamoDBHashKey]
        public virtual string Id { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
