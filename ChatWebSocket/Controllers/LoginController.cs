using ChatWebSocket.Domain.Interfaces.Services;
using ChatWebSocket.Domain.RequestModel;
using ChatWebSocket.Domain.Response;
using ChatWebSocket.Helper;
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
            Response.Cookies.Append("chat-session-id", res.SessionId, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(AppServiceConfig.DayDuration)
            });
            return Ok(res);
        }
    }
}
