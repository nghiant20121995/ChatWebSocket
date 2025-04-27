using ChatWebSocket.Domain.Entities;
using ChatWebSocket.Domain.Interfaces.Services;
using ChatWebSocket.Domain.Response;
using ChatWebSocketHelper;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using ChatWebSocket.Domain.RequestModel;
using ChatWebSocket.Helper;
using ChatWebSocket.Domain.Interfaces.Repository;

namespace ChatWebSocket.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public UserService(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }
        public async Task<LoginResponse> LoginAsync(LoginReq req)
        {
            var existingUser = await _userRepository.GetByEmailAsync(req.Email);
            if (existingUser == null) throw new Exception("User doesn't exist");

            // Missing Authentication step

            var token = JwtHandler.GenerateToken(existingUser.Email,
                existingUser.Id,
                AppServiceConfig.PrivateKey,
                DateTime.UtcNow.AddDays(AppServiceConfig.DayDuration),
                AppServiceConfig.Issuer,
                AppServiceConfig.Audience);
            var retVal = new LoginResponse()
            {
                Email = existingUser.Email,
                Token = token,
                Id = existingUser.Id
            };
            return retVal;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var existingUser = await _userRepository.GetByEmailAsync(email);
            return existingUser;
        }

        public async Task<User> GetByIdAsync(string id)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            return existingUser;
        }
    }
}
