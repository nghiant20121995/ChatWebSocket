using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using ChatWebSocket.Domain.Interfaces.Repository;
using ChatWebSocket.Domain.Interfaces.Services;
using ChatWebSocket.Helper;
using ChatWebSocket.Infrastructure.Repository;
using ChatWebSocket.Services;

namespace ChatWebSocket
{
    public class LambdaStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });

            services.AddScoped<IUserService, UserService>();
            #region repository
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IUserRoomRepository, UserRoomRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            #endregion
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IUserRoomService, UserRoomService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddSingleton<IAmazonDynamoDB>(sp =>
            {
                return new AmazonDynamoDBClient(Amazon.RegionEndpoint.APSoutheast1);
            });

            services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
