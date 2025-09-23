using Amazon.Lambda.AspNetCoreServer;

namespace ChatWebSocket;

public class EntryPoint : APIGatewayHttpApiV2ProxyFunction
{
    protected override void Init(IWebHostBuilder builder)
    {
        builder.UseStartup<LambdaStartup>();
    }
}