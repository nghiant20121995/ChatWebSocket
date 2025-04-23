using Microsoft.AspNetCore.Mvc;

namespace ChatWebSocket.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;

        public BaseController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "PostLogin")]
        public IEnumerable<WeatherForecast> Post()
        {

        }
    }
}
