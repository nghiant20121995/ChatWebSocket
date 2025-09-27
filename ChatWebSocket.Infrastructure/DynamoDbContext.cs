using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatWebSocket.Infrastructure.Repository
{
    public class DynamoContext : IDbNoSQLContext
    {
        private readonly IDynamoDBContext _context;
        public DynamoContext(IDynamoDBContext context)
        {
            _context = context;
        }
        public Task AddAsync<T>(T entity, CancellationToken cancellationToken = default)
        {
            return _context.SaveAsync(entity, cancellationToken);
        }

        public Task DeleteAsync<T>(T entity, CancellationToken cancellationToken = default)
        {
            return _context.SaveAsync(entity, cancellationToken);
        }

        public Task<List<T>> GetAllAsync<T>(CancellationToken cancellationToken = default)
        {
            var query = _context.ScanAsync<T>(new List<ScanCondition>());
            return query.GetNextSetAsync(cancellationToken);
        }

        public Task<T> GetByIdAsync<T>(string id, CancellationToken cancellationToken = default)
        {
            return _context.LoadAsync<T>(id, cancellationToken);
        }

        public Task<List<T>> GetByIdsAsync<T>(IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync<T>(T entity, CancellationToken cancellationToken = default)
        {
            return _context.SaveAsync(entity, cancellationToken);
        }
    }
}
