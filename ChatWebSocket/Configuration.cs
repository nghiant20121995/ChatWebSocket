namespace ChatWebSocket
{
    public class RedisConfig
    {
        public required string ConnectionString { get; set; }
    }

    public class NoSQLDbConfiguration
    {
        public required string HostName { get; set; }
        public required string AccessKey { get; set; }
        public required string SecretKey { get; set; }
    }
}
