using Amazon.DynamoDBv2.DataModel;
using ChatWebSocket.Domain.Entities;
using ChatWebSocket.Domain.Interfaces.Repository;

namespace ChatWebSocket.Infrastructure.Repository
{
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        public RoomRepository(IDbNoSQLContext context) : base(context)
        {
        }
    }
}
