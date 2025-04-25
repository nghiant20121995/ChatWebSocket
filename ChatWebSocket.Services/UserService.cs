using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using ChatWebSocket.Domain.Entities;
using ChatWebSocket.Domain.Interfaces.Services;
using ChatWebSocket.Domain.Response;
using ChatWebSocketHelper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using ChatWebSocket.Domain.RequestModel;
using System.Linq;
using ChatWebSocket.Helper;

namespace ChatWebSocket.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IDynamoDBContext _context;

        public UserService(IConfiguration configuration, IDynamoDBContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task<LoginResponse> LoginAsync(LoginReq req)
        {
            var query = new List<ScanCondition>
            {
                new ScanCondition("Email", ScanOperator.Equal, req.Email)
            };
            var users = await _context.ScanAsync<User>(query).GetNextSetAsync();
            if (users == null || !users.Any()) throw new Exception("User doesn't exist");
            var expAt = DateTime.UtcNow.AddDays(AppServiceConfig.DayDuration);
            var currentUser = users[0];
            var token = JwtHandler.GenerateToken(currentUser.Email,
                AppServiceConfig.PrivateKey,
                expAt,
                AppServiceConfig.Issuer,
                AppServiceConfig.Audience);
            var retVal = new LoginResponse()
            {
                Email = currentUser.Email,
                Token = token,
                Username = currentUser.UserName,
                Id = currentUser.Id
            };
            return retVal;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var query = new List<ScanCondition>
            {
                new ScanCondition("Email", ScanOperator.Equal, email)
            };
            var users = await _context.ScanAsync<User>(query).GetNextSetAsync();
            if (users == null || users.Count == 0) return null;
            return users[0];
        }
    }
}
