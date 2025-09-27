using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using ChatWebSocket.Domain.Exceptions;
using ChatWebSocket.Domain.Interfaces.Services;
using ChatWebSocket.Domain.Response;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;
using ChatWebSocket.Helper;
using ChatWebSocket.Domain.Interfaces.Repository;
using ChatWebSocket.Services;
using ChatWebSocket.Infrastructure.Repository;
using System.Net;
using StackExchange.Redis;
using ChatWebSocket.Domain.Interfaces.Cache;
using ChatWebSocket.Infrastructure.Cache;
using ChatWebSocket.Domain.Context;
using ChatWebSocket.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using ChatWebSocket.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace ChatWebSocket.Extensions
{
    public static class GlobalExceptionExtentions
    {
        public static void UseGlobalExceptionProcess(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Run(async (httpContext) =>
            {
                var logger = httpContext.RequestServices.GetService<ILogger>();
                var exceptionFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
                var exception = exceptionFeature?.Error;

                var resp = new BaseResponse<object>();
                resp.Code = -1;

                if (exception is ValidateException)
                {
                    logger?.LogInformation(exception, exception?.Message);
                    resp.Message = exception?.Message;
                }
                else if (exception is UnauthorizedAccessException || exception is SecurityTokenExpiredException)
                {
                    resp.Message = exception?.Message;
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                }
                else 
                {
                    logger?.LogError(exception, exception?.Message);
                    resp.Message = "Something went wrong. Please try again!";
                }
                var jsonRes = JsonSerializer.Serialize(resp);
                await httpContext.Response.WriteAsync(jsonRes);
            });
        }

        public static void AddConfig(this IHostApplicationBuilder builder)
        {
            builder.Services.Configure<NoSQLDbConfiguration>(builder.Configuration.GetSection(nameof(NoSQLDbConfiguration)));
            builder.Services.Configure<RedisConfig>(builder.Configuration.GetSection(nameof(RedisConfig)));
        }

        public static void AddRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<IMessageRepository, MessageRepository>();
            serviceCollection.AddScoped<IRoomRepository, RoomRepository>();
            serviceCollection.AddScoped<IUserRoomRepository, UserRoomRepository>();
            serviceCollection.AddScoped<INotificationRepository, NotificationRepository>();
        }

        public static void AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUserService, UserService>();
            serviceCollection.AddScoped<IMessageService, MessageService>();
            serviceCollection.AddScoped<IRoomService, RoomService>();
            serviceCollection.AddScoped<IUserRoomService, UserRoomService>();
            serviceCollection.AddScoped<INotificationService, NotificationService>();
        }

        public static void AddDbContext(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IAmazonDynamoDB>(provider =>
            {
                var dbConfig = provider.GetRequiredService<IOptions<NoSQLDbConfiguration>>().Value;
                var config = new AmazonDynamoDBConfig
                {
                    ServiceURL = dbConfig.HostName, // for local DynamoDB
                    Timeout = TimeSpan.FromSeconds(20)
                };

                return new AmazonDynamoDBClient(
                    new Amazon.Runtime.BasicAWSCredentials(dbConfig.AccessKey, dbConfig.SecretKey),
                config
                );
            });

            serviceCollection.AddScoped<IDynamoDBContext, DynamoDBContext>();
            serviceCollection.AddScoped<IDbNoSQLContext, DynamoContext>();
        }

        public static void AddCache(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IConnectionMultiplexer>(provider =>
            {
                var configuration = provider.GetRequiredService<IOptions<RedisConfig>>();
                return ConnectionMultiplexer.Connect(configuration.Value.ConnectionString);
            });

            serviceCollection.AddScoped<ICacheClient, CacheClient>();
        }

        public static void AddExecutionContext(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpContextAccessor();
            serviceCollection.AddScoped(sp =>
            {
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var context = httpContextAccessor.HttpContext!;
                var user = context.User;
                return new ChatExecutionContext
                {
                    UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    FullName = user.FindFirst("FullName")?.Value ?? user.Identity?.Name,
                    Email = user.FindFirst(ClaimTypes.Email)?.Value
                };
            });
        }
    }
}
