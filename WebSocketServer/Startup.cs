using System;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace WebSocketServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseWebSockets();

            app.Use(async (context, next) => 
            {
                if(context.WebSockets.IsWebSocketRequest) 
                {
                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    Console.WriteLine("WebSocket connected.");
                }
                else
                {
                    Console.WriteLine("Hello from second request delegate - Not WebSocket");
                    await next();
                }
            }
            );

            app.Run(async context => 
            {
                Console.WriteLine("Hello from terminal (Run) Request delegate");
                // await context.Response.WriteAsync("");
            }
            );
        }
    }
}
