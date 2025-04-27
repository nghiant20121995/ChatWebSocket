using Amazon.DynamoDBv2.DataModel;
using ChatWebSocket.Domain.Entities;
using ChatWebSocket.Domain.Interfaces.Repository;
//using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace ChatWebSocket.Infrastructure.Repository
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly IDynamoDBContext _context;
        public BaseRepository(IDynamoDBContext context) {
            _context = context;
        }
        public virtual Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(entity.Id)) entity.Id = Guid.NewGuid().ToString();
            entity.CreatedDate = DateTime.UtcNow;
            return _context.SaveAsync(entity, cancellationToken);
        }

        public virtual Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            // Scan all items
            var query = _context.ScanAsync<T>(new List<ScanCondition>());
            return query.GetNextSetAsync(cancellationToken);
        }

        public virtual Task<T> GetByIdAsync(string partitionKey, string sortKey = null, CancellationToken cancellationToken = default)
        {
            return _context.LoadAsync<T>(partitionKey, sortKey, cancellationToken);
        }

        public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            entity.ModifiedDate = DateTime.UtcNow;
            return _context.SaveAsync(entity, cancellationToken);
        }

        public virtual Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            entity.IsDeleted = true;
            return _context.SaveAsync(entity, cancellationToken);
        }
    }
}
