using ChatWebSocket.Helper;
using ChatWebSocketHelper;
using System.Net.WebSockets;
using System.Security.Cryptography;
var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddScoped<>
var app = builder.Build();
app.LoadSecretKey();
app.UseWebSockets();

app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        string? token = context.Request.Query["token"];
        using WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
        Console.WriteLine("WebSocket connected!" + webSocket.ToString());

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

app.Map("/login", async (HttpContext context, IConfiguration configuration) =>
{
    var jwtSection = configuration.GetSection("Jwt");
    var privateKey = File.ReadAllText(jwtSection.GetValue<string>("PrivateKeyPath") ?? "");
    var exp = DateTime.UtcNow.AddDays(jwtSection.GetValue<int>("DayDuration"));
    var jwtToken = JwtHandler.GenerateToken("nghiant", privateKey, exp, jwtSection.GetValue<string>("Issuer"), jwtSection.GetValue<string>("Audience"));
});

await app.RunAsync();