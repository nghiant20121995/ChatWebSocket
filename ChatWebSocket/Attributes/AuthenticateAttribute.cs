using ChatWebSocket.Helper;
using ChatWebSocketHelper;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ChatWebSocket.Attributes
{
    public class Authenticate : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var headers = context.HttpContext.Request.Headers;
            var authenticationHeader = headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authenticationHeader)) throw new UnauthorizedAccessException("Unauthorized");
            var token = authenticationHeader.Replace("Bearer ", string.Empty);
            var rs = JwtHandler.ValidateToken(token, AppServiceConfig.PublicKey, AppServiceConfig.Issuer, AppServiceConfig.Audience);
            if (!rs) throw new UnauthorizedAccessException("Unauthorized");
        }
    }
}
