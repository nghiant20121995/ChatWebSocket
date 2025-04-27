using ChatWebSocket.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatWebSocket.Domain.Interfaces.Services
{
    public interface IRoomService
    {
        Task<Room> GetByIdAsync(string id);
        Task<Room> CreateRoomAsync(string id, string roomName);
    }
}
