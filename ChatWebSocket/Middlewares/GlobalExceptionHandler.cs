using ChatWebSocket.Domain.Exceptions;
using ChatWebSocket.Domain.Response;
using ChatWebSocket.Helper;
using ChatWebSocketHelper;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace ChatWebSocket.Middlewares
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var exceptionFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = exceptionFeature?.Error;

            var resp = new BaseResponse<object>();
            resp.Code = -1;

            if (exception is ValidateException)
            {
                Log.Information("Validation error: {Message}", exception?.Message);
                resp.Message = exception?.Message;
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (exception is UnauthorizedAccessException || exception is SecurityTokenExpiredException)
            {
                resp.Message = exception?.Message;
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                Log.Error(exception, StringHelper.GetInnerException(exception));
                resp.Message = "Something went wrong. Please try again!";
            }
            var jsonRes = JsonSerializer.Serialize(resp);
            await httpContext.Response.WriteAsync(jsonRes);
        }
    }
}
