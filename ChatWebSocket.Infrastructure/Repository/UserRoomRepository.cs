using Amazon.DynamoDBv2.DataModel;
using ChatWebSocket.Domain.Entities;
using ChatWebSocket.Domain.Interfaces.Repository;

namespace ChatWebSocket.Infrastructure.Repository
{
    public class UserRoomRepository : BaseRepository<UserRoom>, IUserRoomRepository
    {
        public UserRoomRepository(IDynamoDBContext context) : base(context)
        {
        }
    }
}
