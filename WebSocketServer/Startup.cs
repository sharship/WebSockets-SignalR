using System.Threading;
using System;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using WebSocketServer.Middleware;

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
