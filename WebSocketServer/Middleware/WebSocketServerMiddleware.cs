using Microsoft.AspNetCore.Http;

namespace WebSocketServer.Middleware
{
    public class WebSocketServerMiddleware
    {
        private readonly RequestDelegate _next;
        public WebSocketServerMiddleware(RequestDelegate next)
        {
            _next = next;

        }

        
    }
}