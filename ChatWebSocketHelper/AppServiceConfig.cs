using Microsoft.Extensions.Configuration;
using System;

namespace ChatWebSocket.Helper
{
    public static class AppServiceConfig
    {
        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        private static IConfiguration Configuration { get; set; }
        public static string PrivateKey => Configuration?.GetSection("Jwt:PrivateKey")?.Value;
        public static string PublicKey => Configuration?.GetSection("Jwt:PublicKey")?.Value;
        public static double DayDuration => Convert.ToDouble(Configuration?.GetSection("Jwt:DayDuration")?.Value);
        public static string Issuer => Configuration?.GetSection("Jwt:Issuer")?.Value;
        public static string Audience => Configuration?.GetSection("Jwt:Audience")?.Value;
        public static string WebSocketHost => Configuration?.GetSection("WebSocket:Host")?.Value;
        public static string WebSocketAccessKey => Configuration?.GetSection("WebSocket:AccessKey")?.Value;
        public static string WebSocketSecretKey => Configuration?.GetSection("WebSocket:SecretKey")?.Value;
    }
}
