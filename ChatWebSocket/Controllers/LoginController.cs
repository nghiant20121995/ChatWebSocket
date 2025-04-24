using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
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
        private readonly IAmazonDynamoDB _amazonDynamoDB;

        public LoginController(IDynamoDBContext dynamoDBContext, IAmazonDynamoDB amazonDynamoDB) : base()
        {
            _context = dynamoDBContext;
            _amazonDynamoDB = amazonDynamoDB;
        }
        [HttpPost(Name = "LoginPost")]
        public async Task<bool> Post(LoginRequest req)
        {
            try
            {
                //var lsTbl = await _amazonDynamoDB.ListTablesAsync();

                //await _amazonDynamoDB.CreateTableAsync(new CreateTableRequest
                //{
                //    TableName = "User",
                //    KeySchema = new List<KeySchemaElement>
                //    {
                //        new KeySchemaElement("Id", KeyType.HASH)
                //    },
                //    AttributeDefinitions = new List<AttributeDefinition>
                //    {
                //        new AttributeDefinition("Id", ScalarAttributeType.S)
                //    },
                //    ProvisionedThroughput = new ProvisionedThroughput(5, 5)
                //});


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
