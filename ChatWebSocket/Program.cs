using ChatWebSocket.Middlewares;
using ChatWebSocket.Extensions;


var builder = WebApplication.CreateBuilder(args);
builder.AddConfig();
builder.Services.AddCache();
builder.Services.AddServices();
builder.Services.AddDbContext();
builder.Services.AddRepositories();
builder.Services.AddExecutionContext();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});
builder.Services.AddCors((options) =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowCredentials()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseExceptionHandler(builder =>
{
    builder.UseGlobalExceptionProcess();
});

app.UseWhen((context) => context.Request.Path.Equals("/ws"), appBuilder =>
{
    appBuilder.UseWebSockets();
    appBuilder.UseMiddleware<WebSocketAuthentication>();
    appBuilder.UseMiddleware<WebSocketHandler>();
});

app.MapControllers();
//app.UseCors("AllowAll");
app.Run();