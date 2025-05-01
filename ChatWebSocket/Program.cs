using ChatWebSocket.Helper;
using ChatWebSocket.Middlewares;
using ChatWebSocket.Extensions;
var builder = WebApplication.CreateBuilder(args);
AppServiceConfig.Initialize(builder.Configuration);
builder.LoadSecretKey();
builder.ConfigServices();
builder.Services.AddControllers();
builder.Services.AddCors((options) =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

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
app.UseCors("AllowAll");

await app.RunAsync();