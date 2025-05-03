using Amazon.DynamoDBv2.DataModel;
using ChatWebSocket.Domain.Entities;
using ChatWebSocket.Domain.Interfaces.Repository;
using ChatWebSocket.Domain.RequestModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChatWebSocket.Infrastructure.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IDynamoDBContext context) : base(context)
        {
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var query = _context.QueryAsync<User>(email,
                new DynamoDBOperationConfig
                {
                    IndexName = "User_Email"
                }
            );
            var users = await query.GetNextSetAsync();
            return users.FirstOrDefault();
        }

        public Task<List<User>> GetAllAsync(UserFilterReq req, CancellationToken cancellationToken = default)
        {
            var filters = new List<ScanCondition>();
            DynamoDBOperationConfig config = null;
            if (!string.IsNullOrEmpty(req.Keyword))
            {
                filters.Add(new ScanCondition("FullName", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Contains, req.Keyword));
                config = new DynamoDBOperationConfig()
                {
                    IndexName = "User_FullName",
                };
            }
            // Scan all items
            var query = _context.ScanAsync<User>(filters, config);
            return query.GetNextSetAsync(cancellationToken);
        }
    }
}
