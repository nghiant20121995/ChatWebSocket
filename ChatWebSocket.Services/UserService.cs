using ChatWebSocket.Domain.Interfaces.Services;
using ChatWebSocketHelper;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

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
            return JwtHandler.GenerateToken(username, privateKey, expireAt, issuer, audience);
        }
    }
}
