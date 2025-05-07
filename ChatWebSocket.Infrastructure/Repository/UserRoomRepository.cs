using Amazon.DynamoDBv2.DataModel;
using ChatWebSocket.Domain.Entities;
using ChatWebSocket.Domain.Interfaces.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatWebSocket.Infrastructure.Repository
{
    public class UserRoomRepository : BaseRepository<UserRoom>, IUserRoomRepository
    {
        public UserRoomRepository(IDynamoDBContext context) : base(context)
        {
        }

        public Task<List<UserRoom>> GetByUserIdAsync(string userId)
        {
            var query = _context.QueryAsync<UserRoom>(userId,
                new DynamoDBOperationConfig
                {
                    IndexName = "UserId_CreatedDate",
                    BackwardQuery = true
                }
            );
            return query.GetNextSetAsync();
        }
    }
}
