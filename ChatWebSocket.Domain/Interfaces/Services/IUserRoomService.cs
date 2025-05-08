using ChatWebSocket.Domain.Entities;
using ChatWebSocket.Domain.RequestModel;
using ChatWebSocket.Domain.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatWebSocket.Domain.Interfaces.Services
{
    public interface IUserRoomService
    {
        Task<UserRoom> AddMemberToRoomAsync(string roomId, string userId);
        Task<List<UserRoom>> GetLatestRoomByUserIdAsync(string userId);
    }
}
