using Microsoft.AspNetCore.Mvc;

namespace ChatWebSocket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : BaseController
    {

        public PingController(IConfiguration configuration) : base(configuration)
        {
        }

        [HttpGet]
        public string Get()
        {
            return "pong";
        }
    }
}
