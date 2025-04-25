using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using ChatWebSocket.Domain.Exceptions;
using ChatWebSocket.Domain.Interfaces.Services;
using ChatWebSocket.Domain.Response;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using System.Reflection.PortableExecutable;
using ChatWebSocket.Helper;

namespace ChatWebSocket.Services.Extensions
{
    public static class GlobalExceptionExtentions
    {
        public static void UseGlobalExceptionProcess(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Run(async (httpContext) =>
            {
                var logger = httpContext.RequestServices.GetService<ILogger>();
                var exceptionFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
                var exception = exceptionFeature.Error;

                var resp = new BaseResponse<object>();
                resp.Code = -1;

                if (exception is ValidateException)
                {
                    logger?.LogInformation(exception, exception?.Message);
                    resp.Message = exception?.Message;
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
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddSingleton<IAmazonDynamoDB>(sp =>
            {
                var config = new AmazonDynamoDBConfig
                {
                    ServiceURL = AppServiceConfig.WebSocketHost, // for local DynamoDB
                };

                return new AmazonDynamoDBClient(
                    new Amazon.Runtime.BasicAWSCredentials(AppServiceConfig.WebSocketAccessKey, AppServiceConfig.WebSocketSecretKey),
                    config
                );
            });

            builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
        }
    }
}
