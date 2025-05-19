using Amazon.DynamoDBv2.DataModel;
using ChatWebSocket.Domain.Entities;
using ChatWebSocket.Domain.Interfaces.Repository;
using ChatWebSocket.Domain.Interfaces.Services;
using ChatWebSocket.Domain.RequestModel;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatWebSocket.Services
{
    public class MessageService : IMessageService
    {
        private readonly IConfiguration _configuration;
        private readonly IMessageRepository _messageRepository;

        public MessageService(IConfiguration configuration, IMessageRepository messageRepository)
        {
            _configuration = configuration;
            _messageRepository = messageRepository;
        }

        public Task CreateAsync(MessageRequest req, string userId, string roomId)
        {
            var msg = new Message()
            {
                Content = req.Content,
                ReceiverId = req.ReceiverId,
                SenderId = userId,
                RoomId = roomId,
                IsSeen = false
            };
            return _messageRepository.AddAsync(msg);
        }

        public Task<List<Message>> GetByFilterAsync(MessageFilterRequest req)
        {
            return _messageRepository.GetByFilterAsync(req);
        }
    }
}
