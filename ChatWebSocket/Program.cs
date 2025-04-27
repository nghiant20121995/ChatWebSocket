using ChatWebSocket.Helper;
using ChatWebSocket.Middlewares;
using ChatWebSocket.Extensions;
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
    wsApp.UseMiddleware<WebSocketHandler>();
});

app.MapControllers();

await app.RunAsync();