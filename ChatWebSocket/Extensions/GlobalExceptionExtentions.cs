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
                else if (exception is UnauthorizedAccessException)
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

        public static void LoadSecretKey(this IHostApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            var jwtSection = configuration.GetSection("Jwt");
            if (jwtSection != null)
            {
                var privateKey = File.ReadAllText(jwtSection?.GetSection("PrivateKeyPath")?.Value ?? string.Empty);
                if (!string.IsNullOrEmpty(privateKey))
                {
                    configuration["Jwt:PrivateKey"] = privateKey;
                }
                var publicKey = File.ReadAllText(jwtSection?.GetSection("PublicKeyPath")?.Value ?? string.Empty);
                if (!string.IsNullOrEmpty(publicKey))
                {
                    configuration["Jwt:PublicKey"] = publicKey;
                }
            }
        }

        public static void ConfigServices(this IHostApplicationBuilder builder)
        {
            #region repository
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddScoped<IUserRoomRepository, UserRoomRepository>();
            #endregion
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IMessageService, MessageService>();
            builder.Services.AddScoped<IRoomService, RoomService>();
            builder.Services.AddScoped<IUserRoomService, UserRoomService>();
            builder.Services.AddSingleton<IAmazonDynamoDB>(sp =>
            {
                var config = new AmazonDynamoDBConfig
                {
                    ServiceURL = AppServiceConfig.DynamoDbHost, // for local DynamoDB
                };

                return new AmazonDynamoDBClient(
                    new Amazon.Runtime.BasicAWSCredentials(AppServiceConfig.DynamoDbAccessKey, AppServiceConfig.DynamoDbSecretKey),
                    config
                );
            });

            builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
        }

        public static void AddRedis(this IHostApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                return ConnectionMultiplexer.Connect(AppServiceConfig.RedisConnectionString);
            });

            builder.Services.AddScoped<ICacheClient, CacheClient>();
        }

        public static void AddContext(this IHostApplicationBuilder builder)
        {
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ChatExecutionContext>(sp =>
            {
                var nonexistentUser = new ChatExecutionContext()
                {
                    SessionId = string.Empty,
                    UserId = string.Empty,
                    FullName = string.Empty,
                    Email = string.Empty
                };
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var context = httpContextAccessor.HttpContext!;
                var sessionId = context.Request.Cookies["chat-session-id"];
                if (string.IsNullOrEmpty(sessionId)) return nonexistentUser;

                var cacheClient = context.RequestServices.GetService<ICacheClient>()!;
                var sessionKey = string.Format(RedisKeys.UserSessionKey, sessionId);
                var sessionCache = cacheClient.GetString(sessionKey);
                if (string.IsNullOrEmpty(sessionCache)) return nonexistentUser;
                
                var user = JsonSerializer.Deserialize<User>(sessionCache)!;
                return new ChatExecutionContext()
                {
                    SessionId = sessionId,
                    UserId = user.Id,
                    FullName = user.FullName,
                    Email = user.Email
                };
            });
        }
    }
}
