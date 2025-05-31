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
using System.Collections.Generic;
using ChatWebSocket.Domain.Interfaces.Cache;
using System.Text.Json;
using Microsoft.AspNet.Identity;

namespace ChatWebSocket.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly ICacheClient _cacheClient;

        public UserService(IConfiguration configuration, IUserRepository userRepository, ICacheClient cacheClient)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _cacheClient = cacheClient;
        }
        public async Task<LoginResponse> LoginAsync(LoginReq req)
        {
            var existingUser = await _userRepository.GetByEmailAsync(req.Email);
            if (existingUser == null) throw new Exception("User doesn't exist");

            var hasher = new PasswordHasher();
            var result = hasher.VerifyHashedPassword(existingUser.Password, req.Password);
            if (result == PasswordVerificationResult.Failed) throw new Exception("Wrong Password");


            // Missing Authentication step
            var token = JwtHandler.GenerateToken(existingUser.Email,
                existingUser.Id,
                AppServiceConfig.PrivateKey,
                DateTime.UtcNow.AddDays(AppServiceConfig.DayDuration),
                AppServiceConfig.Issuer,
                AppServiceConfig.Audience);

            var sessionId = Guid.NewGuid().ToString();
            var serializedUser = JsonSerializer.Serialize(existingUser);

            var sessionKey = string.Format(RedisKeys.UserSessionKey, sessionId);
            // save session
            await _cacheClient.SetStringAsync(sessionKey, serializedUser, TimeSpan.FromDays(AppServiceConfig.DayDuration));

            // set cookie header
            var retVal = new LoginResponse()
            {
                Email = existingUser.Email,
                Token = token,
                Id = existingUser.Id,
                SessionId = sessionId,
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

        public async Task<List<User>> GetAllAsync(UserFilterReq req)
        {
            return await _userRepository.GetAllAsync(req);
        }
    }
}
