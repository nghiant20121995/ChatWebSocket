using ChatWebSocket.Attributes;
using ChatWebSocket.Domain.Context;
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
    public class ConversationController : BaseController
    {
        private readonly IUserRoomService _userRoomService;
        private readonly ChatExecutionContext _context;

        public ConversationController(IConfiguration configuration, IUserRoomService userRoomService, ChatExecutionContext context) : base(configuration)
        {
            _userRoomService = userRoomService;
            _context = context;
        }

        [HttpGet]
        public async Task<BaseResponse<List<UserRoom>>> Get([FromQuery] ConversationRequest req)
        {
            var res = await _userRoomService.GetLatestRoomByUserIdAsync(_context.UserId);
            return Ok(res);
        }
    }
}
