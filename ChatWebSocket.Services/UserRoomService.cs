using ChatWebSocket.Domain.Entities;
using ChatWebSocket.Domain.Interfaces.Repository;
using ChatWebSocket.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ChatWebSocket.Services
{
    public class UserRoomService : IUserRoomService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRoomRepository _userRoomRepository;

        public UserRoomService(IConfiguration configuration, IUserRoomRepository userRoomRepository)
        {
            _configuration = configuration;
            _userRoomRepository = userRoomRepository;
        }

        public async Task<UserRoom> AddMemberToRoomAsync(string roomId, string userId)
        {
            var currentUserRoom = await _userRoomRepository.GetByIdAsync(roomId, userId);
            if (currentUserRoom == null)
            {
                var newUserRoom = new UserRoom()
                {
                    Id = roomId,
                    UserId = userId
                };
                await _userRoomRepository.AddAsync(newUserRoom);
                return newUserRoom;
            }
            return currentUserRoom;
        }
    }
}
