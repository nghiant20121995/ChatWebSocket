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
    }
}
