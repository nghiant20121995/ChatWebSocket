using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWebSocket.Services
{
    public class BaseService<T>
    {
        private readonly IDynamoDBContext _dbContext;
        public BaseService(IDynamoDBContext dynamoDBContext)
        {
            _dbContext = dynamoDBContext;
        }
        public Task<T> GetByIdAsync(string id)
        {
            return _dbContext.LoadAsync<T>(id);
        }
    }
}
