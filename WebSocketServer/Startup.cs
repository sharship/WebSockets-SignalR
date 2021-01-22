using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebSocketServer.Middleware;

namespace WebSocketServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWebSocketServerConnectionManager();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseWebSockets();

            // add customized WebSocket request delegate
            app.UseWebSocketServer();

            app.Run(async context => 
            {
                Console.WriteLine("Hello from terminal (Run) Request delegate");
                await context.Response.WriteAsync("Hello from terminal (Run) Request delegate");
            }
            );
        }    

    }
}
