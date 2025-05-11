namespace ChatWebSocket.Domain.Context
{
    public class ChatExecutionContext
    {
        public string SessionId { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
