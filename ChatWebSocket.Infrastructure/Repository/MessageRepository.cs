using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using ChatWebSocket.Domain.Entities;
using ChatWebSocket.Domain.Interfaces.Repository;
using ChatWebSocket.Domain.RequestModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChatWebSocket.Infrastructure.Repository
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        public MessageRepository(IDynamoDBContext context) : base(context)
        {
        }

        public Task<List<Message>> GetByFilterAsync(MessageFilterRequest req, CancellationToken cancellationToken = default)
        {
            var filters = new List<ScanCondition>();
            if (!string.IsNullOrEmpty(req.SenderId))
            {
                filters.Add(new ScanCondition("SenderId", ScanOperator.Equal, req.SenderId));
            }
            if (!string.IsNullOrEmpty(req.ReceiverId))
            {
                filters.Add(new ScanCondition("ReceiverId", ScanOperator.Equal, req.ReceiverId));
            }
            var config = new DynamoDBOperationConfig()
            {
                IndexName = "Message_Room_CreatedDate",
                QueryFilter = filters
            };

            var range = GetDateRange(req.FromDate, req.ToDate);
            AsyncSearch<Message> query;
            if (range != null)
            {
                query = _context.QueryAsync<Message>(req.RoomId, QueryOperator.Between, range, config);
            }
            else
            {
                query = _context.QueryAsync<Message>(req.RoomId, config);
            }
            return query.GetNextSetAsync(cancellationToken);
        }
    }
}
