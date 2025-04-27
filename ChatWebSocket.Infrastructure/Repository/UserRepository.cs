using Amazon.DynamoDBv2.DataModel;
using ChatWebSocket.Domain.Entities;
using ChatWebSocket.Domain.Interfaces.Repository;
using System.Linq;
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
    }
}
