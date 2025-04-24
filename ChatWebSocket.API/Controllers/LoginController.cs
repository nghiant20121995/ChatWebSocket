using Microsoft.AspNetCore.Mvc;

namespace ChatWebSocket.API.Controllers
{
    public class LoginController : BaseController
    {
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger) : base(logger)
        {
            _logger = logger;
        }
    }
}
