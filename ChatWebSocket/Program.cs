using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using ChatWebSocket.Domain.Interfaces.Services;
using ChatWebSocket.Services;
using ChatWebSocket.Services.Extensions;
using ChatWebSocketHelper;
using System.Net.WebSockets;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IAmazonDynamoDB>(sp =>
{
    var config = new AmazonDynamoDBConfig
    {
        ServiceURL = "http://localhost:9000", // for local DynamoDB
    };

    return new AmazonDynamoDBClient(
        new Amazon.Runtime.BasicAWSCredentials("gmjfj", "e65lqb"),
        config
    );
});

builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>();

builder.Services.AddControllers();

var app = builder.Build();
app.LoadSecretKey();
app.UseWebSockets();

app.UseExceptionHandler(builder =>
{
    builder.UseGlobalExceptionProcess();
});


app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        string? token = context.Request.Query["token"];
        //JwtHandler.ValidateToken(token);
        using WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

        var buffer = new byte[1024 * 16];

        while (webSocket.State == WebSocketState.Open)
        {
            try
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    Console.WriteLine("WebSocket closed.");
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Bye", CancellationToken.None);

                }
                else
                {
                    //var lsMsg = new List<string>();
                    //var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    //lsMsg.Add(message);
                    //var json = JsonSerializer.Serialize(lsMsg);
                    //var responseMessage = Encoding.UTF8.GetBytes(json);

                    //var allClients = WebSocketConnectionManager.GetAllSockets();
                    //foreach (var client in allClients)
                    //{
                    //    if (client.State == WebSocketState.Open)
                    //    {
                    //        await client.SendAsync(
                    //        new ArraySegment<byte>(responseMessage, 0, responseMessage.Length),
                    //        WebSocketMessageType.Text,
                    //        true,
                    //        CancellationToken.None);
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
    else
    {
        context.Response.StatusCode = 400;
    }
});

app.MapControllers();

await app.RunAsync();