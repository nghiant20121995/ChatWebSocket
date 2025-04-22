using ChatWebSocket.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using ChatWebSocketHelper;
using System.Security.Cryptography;

namespace ChatWebSocket.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;

        public UserService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> LoginAsync(string username, string password)
        {
            var privateKey = "";
            var expireAt = DateTime.UtcNow.AddDays(7);
            var issuer = "chat-websocket";
            var audience = "chat-websocket";
            return JwtHandler.GenerateToken(username, privateKey, expireAt, issuer, audience);
        }
    }
}
