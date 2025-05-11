using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using ChatWebSocket.Domain.Entities;
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
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly IDynamoDBContext _context;
        public BaseRepository(IDynamoDBContext context)
        {
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


        public virtual DynamoDBEntry[] GetDateRange(DateTime? fromDate, DateTime? toDate)
        {
            DynamoDBEntry[] range;
            if (fromDate != null && toDate != null)
            {
                range = new DynamoDBEntry[] { fromDate.Value, toDate.Value };
            }
            else if (fromDate != null)
            {
                range = new DynamoDBEntry[] { fromDate.Value, DateTime.MaxValue };
            }
            else if (toDate != null)
            {
                range = new DynamoDBEntry[] { DateTime.MinValue, toDate.Value };
            }
            else
            {
                range = null;
            }
            return range;
        }

        public async Task<Dictionary<string, T>> GetByListIdsAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            var batch = _context.CreateBatchGet<T>();

            foreach (var key in ids)
            {
                batch.AddKey(key);
            }

            await batch.ExecuteAsync();

            var results = batch.Results.ToDictionary(e => e.Id);
            return results;
        }

        public async Task<List<T>> GetByIdsAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            var batch = _context.CreateBatchGet<T>();

            foreach (var key in ids)
            {
                batch.AddKey(key);
            }

            await batch.ExecuteAsync();

            var results = batch.Results;
            return results;
        }
    }
}
