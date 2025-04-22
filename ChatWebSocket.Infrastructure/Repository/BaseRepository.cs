using Amazon.DynamoDBv2.DataModel;
using ChatWebSocket.Domain.Entities;
using ChatWebSocket.Domain.Interfaces.Repository;
//using MongoDB.Driver;
using System;
using System.Collections.Generic;
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

        public List<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        //public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        //{
        //    //entity.ModifiedDate = DateTime.UtcNow;
        //    //_context.
        //}

        //public virtual Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        //{
        //    //return _collection.DeleteOneAsync(e => e.Id.Equals(entity.Id), cancellationToken);
        //}

        //public virtual List<T> GetAll()
        //{
        //    //return _context.LoadAsync<T>();
        //}

        //public virtual Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
        //{
        //    //return _collection.Find(_ => true).ToListAsync(cancellationToken);
        //}

        //public virtual Task<T> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        //{
        //    //return _collection.Find(e => e.Id.Equals(id)).FirstOrDefaultAsync(cancellationToken);
        //}
    }
}
