using ChatWebSocket.Attributes;
using ChatWebSocket.Domain.Entities;
using ChatWebSocket.Domain.Interfaces.Services;
using ChatWebSocket.Domain.RequestModel;
using ChatWebSocket.Domain.Response;
using Microsoft.AspNetCore.Mvc;

namespace ChatWebSocket.Controllers
{
    [Route("api/[controller]")]
    [Authenticate]
    [ApiController]
    public class MessageController : BaseController
    {
        private readonly IMessageService _messageService;

        public MessageController(IConfiguration configuration, IMessageService messageService) : base(configuration)
        {
            _messageService = messageService;
        }

        [HttpGet]
        public async Task<BaseResponse<List<Message>>> Get([FromQuery] MessageFilterRequest req)
        {
            var res = await _messageService.GetByFilterAsync(req);
            return Ok(res);
        }
    }
}
