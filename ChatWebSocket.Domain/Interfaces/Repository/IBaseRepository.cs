using ChatWebSocket.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChatWebSocket.Domain.Interfaces.Repository
{
    public interface IBaseRepository<T>
    {
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task<T> GetByIdAsync(string partitionKey, string sortKey = null, CancellationToken cancellationToken = default);
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
        Task<Dictionary<string, T>> GetByListIdsAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default);
        Task<List<T>> GetByIdsAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default);
    }
}
