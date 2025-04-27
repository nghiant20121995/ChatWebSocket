using ChatWebSocket.Domain.Entities;
using ChatWebSocket.Domain.Interfaces.Services;
using ChatWebSocket.Domain.RequestModel;
using ChatWebSocket.Domain.Response;
using Microsoft.AspNetCore.Mvc;

namespace ChatWebSocket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IConfiguration _configuration;

        public BaseController(IConfiguration configuration) : base()
        {
            _configuration = configuration;
        }

        protected BaseResponse<T> Ok<T>(T res)
        {
            var rs = new BaseResponse<T>();
            rs.Data = res;
            return rs;
        }
    }
}
