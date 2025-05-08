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
using ChatWebSocket.Domain.Interfaces.Repository;

namespace ChatWebSocket.Services
{
    public class RoomService : IRoomService
    {
        private readonly IConfiguration _configuration;
        private readonly IDynamoDBContext _context;
        private readonly IRoomRepository _roomRepository;

        public RoomService(IConfiguration configuration, IDynamoDBContext context, IRoomRepository roomRepository)
        {
            _configuration = configuration;
            _context = context;
            _roomRepository = roomRepository;
        }

        public Task<Room> GetByIdAsync(string id)
        {
            return _roomRepository.GetByIdAsync(id);
        }

        public async Task<Room> CreateRoomAsync(string id, string roomName) {
            var existingRoom = await _roomRepository.GetByIdAsync(id);
            if (existingRoom == null)
            {
                var newRoom = new Room()
                {
                    Id = id,
                    RoomName = roomName
                };
                await _roomRepository.AddAsync(newRoom);
                return newRoom;
            }
            return existingRoom;
        }


        public Task UpdateRoomAsync(Room room)
        {
            return _roomRepository.UpdateAsync(room);
        }
    }
}
