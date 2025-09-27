using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatWebSocket.Infrastructure
{
    public interface IDbNoSQLContext
    {
        Task AddAsync<T>(T entity, CancellationToken cancellationToken = default);
        Task<T> GetByIdAsync<T>(string Id, CancellationToken cancellationToken = default);
        Task<List<T>> GetAllAsync<T>(CancellationToken cancellationToken = default);
        Task UpdateAsync<T>(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync<T>(T entity, CancellationToken cancellationToken = default);
        Task<List<T>> GetByIdsAsync<T>(IEnumerable<string> ids, CancellationToken cancellationToken = default);
    }
}
