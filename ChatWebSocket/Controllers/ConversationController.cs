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
    public class ConversationController : BaseController
    {
        private readonly IUserRoomService _userRoomService;

        public ConversationController(IConfiguration configuration, IUserRoomService userRoomService) : base(configuration)
        {
            _userRoomService = userRoomService;
        }

        [HttpGet]
        public async Task<BaseResponse<List<Room>>> Get([FromQuery] ConversationRequest req)
        {
            var res = await _userRoomService.GetLatestRoomByUserIdAsync(req.UserId);
            return Ok(res);
        }
    }
}
