using ChatWebSocket.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChatWebSocket.Domain.Interfaces.Repository
{
    public interface IUserRoomRepository : IBaseRepository<UserRoom>
    {
        Task<List<UserRoom>> GetByUserIdAsync(string userId);
    }
}
