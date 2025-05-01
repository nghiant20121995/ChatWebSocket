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
        public static string DynamoDbHost => Configuration?.GetSection("DynamoDb:Host")?.Value;
        public static string DynamoDbAccessKey => Configuration?.GetSection("DynamoDb:AccessKey")?.Value;
        public static string DynamoDbSecretKey => Configuration?.GetSection("DynamoDb:SecretKey")?.Value;
    }
}
