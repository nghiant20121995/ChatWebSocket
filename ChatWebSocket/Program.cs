using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using ChatWebSocket.Domain.Interfaces.Services;
using ChatWebSocket.Helper;
using ChatWebSocket.Middlewares;
using ChatWebSocket.Services;
using ChatWebSocket.Services.Extensions;
using ChatWebSocketHelper;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Text;
var builder = WebApplication.CreateBuilder(args);
AppServiceConfig.Initialize(builder.Configuration);
builder.LoadSecretKey();
builder.ConfigServices();
builder.Services.AddControllers();

var app = builder.Build();
app.UseWebSockets();


app.UseExceptionHandler(builder =>
{
    builder.UseGlobalExceptionProcess();
});


app.Map("/ws", wsApp =>
{
    wsApp.UseMiddleware<WebSocketAuthenticate>();
    wsApp.Run(async context =>
    {
        var userService = context.RequestServices.GetService<IUserService>();
        string? token = context.Request.Query["token"];

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var email = jwtToken.Claims.FirstOrDefault(e => e.Type.Equals("Email"));
        if (email == null) throw new Exception("User is not found");

        var currentUser = await userService!.GetByEmailAsync(email.Value);
        if (currentUser == null) throw new Exception("User doesn't exist");
        using WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

        WebSocketConnectionManager.AddSocket(webSocket, currentUser.Id);

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
                    WebSocketConnectionManager.RemoveSocket(webSocket, currentUser.Id);

                }
                else
                {
                    //var lsMsg = new List<string>();
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
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
    });
});

app.MapControllers();

await app.RunAsync();