using ChatWebSocket.Attributes;
using ChatWebSocket.Domain.Entities;
using ChatWebSocket.Domain.Interfaces.Services;
using ChatWebSocket.Domain.RequestModel;
using ChatWebSocket.Domain.Response;
using Microsoft.AspNetCore.Mvc;

namespace ChatWebSocket.Controllers
{
    [Authenticate]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IConfiguration configuration, IUserService userService) : base(configuration)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<BaseResponse<List<User>>> Get([FromQuery] UserFilterReq req)
        {
            var res = await _userService.GetAllAsync(req);
            return Ok(res);
        }
    }
}
