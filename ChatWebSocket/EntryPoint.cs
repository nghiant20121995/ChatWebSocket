using Amazon.Lambda.AspNetCoreServer;

namespace ChatWebSocket
{
    public class EntryPoint : APIGatewayProxyFunction
    {
        protected override void Init(IWebHostBuilder builder)
        {
            builder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<LambdaStartup>();
            });
        }
    }
}
