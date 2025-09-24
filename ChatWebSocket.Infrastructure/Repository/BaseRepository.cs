using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using ChatWebSocket.Domain.Entities;
using ChatWebSocket.Domain.Interfaces;
using ChatWebSocket.Domain.Interfaces.Repository;
//using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChatWebSocket.Infrastructure.Repository
{
    public abstract class BaseRepository<T> : IBaseRepository<T> 
    {
        protected readonly IDbNoSQLContext _context;
        public BaseRepository(IDbNoSQLContext context)
        {
            _context = context;
        }
        public virtual Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            return _context.AddAsync(entity, cancellationToken);
        }

        public virtual Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return _context.GetAllAsync<T>(cancellationToken);
        }

        public virtual Task<T> GetByIdAsync(string Id, CancellationToken cancellationToken = default)
        {
            return _context.GetByIdAsync<T>(Id, cancellationToken);
        }

        public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            return _context.UpdateAsync(entity, cancellationToken);
        }

        public virtual Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            return _context.DeleteAsync(entity, cancellationToken);
        }

        public Task<List<T>> GetByIdsAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            return _context.GetByIdsAsync<T>(ids, cancellationToken);
        }
    }
}
