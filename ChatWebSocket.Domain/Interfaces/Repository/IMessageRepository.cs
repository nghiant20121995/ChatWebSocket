using ChatWebSocket.Domain.Entities;
using ChatWebSocket.Domain.RequestModel;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChatWebSocket.Domain.Interfaces.Repository
{
    public interface IMessageRepository : IBaseRepository<Message>
    {
        Task<List<Message>> GetByFilterAsync(MessageFilterRequest req, CancellationToken cancellationToken = default);
    }
}
