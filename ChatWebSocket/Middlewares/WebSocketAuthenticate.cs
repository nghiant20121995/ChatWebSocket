using ChatWebSocket.Helper;
using ChatWebSocketHelper;

namespace ChatWebSocket.Middlewares
{
    public class WebSocketAuthenticate
    {
        private readonly RequestDelegate _next;

        public WebSocketAuthenticate(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest) throw new Exception("Protocol is not accept");
            string? token = context.Request.Query["token"];

            var rs = JwtHandler.ValidateToken(token, AppServiceConfig.PublicKey, AppServiceConfig.Issuer, AppServiceConfig.Audience);
            if (!rs) throw new Exception("Unauthorized");

            await _next(context);
        }
    }
}
