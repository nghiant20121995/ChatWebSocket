using ChatWebSocket.Domain.Context;
using ChatWebSocket.Domain.Entities;
using ChatWebSocket.Domain.Interfaces.Repository;
using ChatWebSocket.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatWebSocket.Services
{
    public class UserRoomService : IUserRoomService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRoomRepository _userRoomRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IUserRepository _userRepository;

        public UserRoomService(IConfiguration configuration, 
            IUserRoomRepository userRoomRepository, 
            IRoomRepository roomRepository, 
            IUserRepository userRepository, 
            ChatExecutionContext context)
        {
            _configuration = configuration;
            _userRoomRepository = userRoomRepository;
            _roomRepository = roomRepository;
            _userRepository = userRepository;
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

        public async Task<List<UserRoom>> GetLatestRoomByUserIdAsync(string userId)
        {
            var userRooms = await _userRoomRepository.GetByUserIdAsync(userId);
            if (userRooms == null || userRooms.Count == 0)
            {
                return new();
            }
            var rooms = await _roomRepository.GetByListIdsAsync(userRooms.Select(x => x.Id));
            if (rooms == null || rooms.Count == 0)
            {
                return new();
            }
            foreach (var userRoom in userRooms)
            {
                var existingRoom = rooms.ContainsKey(userRoom.Id) ? rooms[userRoom.Id] : null;
                if (existingRoom != null)
                {
                    userRoom.Room = existingRoom;
                    if (!existingRoom.IsGroup)
                    {
                        var userIds = userRoom.Id.Split('_');
                        var members = await _userRepository.GetByIdsAsync(userIds);
                        userRoom.Members = members;
                    }
                }
            }
            return userRooms;
        }
    }
}
