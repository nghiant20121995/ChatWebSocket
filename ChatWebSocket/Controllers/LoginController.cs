using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using ChatWebSocket.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace ChatWebSocket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IDynamoDBContext _context;
        public LoginController(IDynamoDBContext dynamoDBContext) : base() {
            _context = dynamoDBContext;
        }
        [HttpPost(Name = "LoginPost")]
        public async Task<bool> Post(LoginRequest req)
        {
            try
            {
                await _context.SaveAsync(new User
                {
                    UserName = "john123",
                    Email = "john@example.com"
                });

                var conditions = new List<ScanCondition>
                {
                    new ScanCondition("Email", ScanOperator.Equal, req.Email)
                };
                var response = await _context.ScanAsync<User>(conditions).GetNextSetAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
