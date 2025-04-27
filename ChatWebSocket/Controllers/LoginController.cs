using ChatWebSocket.Domain.Interfaces.Services;
using ChatWebSocket.Domain.RequestModel;
using ChatWebSocket.Domain.Response;
using Microsoft.AspNetCore.Mvc;

namespace ChatWebSocket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : BaseController
    {
        private readonly IUserService _userService;

        public LoginController(IConfiguration configuration, IUserService userService) : base(configuration)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<BaseResponse<LoginResponse>> Post(LoginReq req)
        {
            var res = await _userService.LoginAsync(req);
            return Ok(res);
        }
    }
}
