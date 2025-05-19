using ChatWebSocket.Domain.Entities;
using ChatWebSocket.Domain.RequestModel;
using ChatWebSocket.Domain.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatWebSocket.Domain.Interfaces.Services
{
    public interface INotificationService
    {
        Task CreateAsync(MessageRequest req, string userId, string roomId);
        Task<List<Message>> GetByFilterAsync(MessageFilterRequest req);
    }
}
