using ChatWebSocket.Domain.Interfaces.Services;
using ChatWebSocket.Domain.RequestModel;
using ChatWebSocket.Domain.Response;
using Microsoft.AspNetCore.Mvc;

namespace ChatWebSocket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public LoginController(IUserService userService, IConfiguration configuration) : base()
        {
            _configuration = configuration;
            _userService = userService;
        }
        [HttpPost(Name = "LoginPost")]
        public async Task<LoginResponse> Post(LoginReq req)
        {
            var res = await _userService.LoginAsync(req);
            return res;
        }
    }
}
