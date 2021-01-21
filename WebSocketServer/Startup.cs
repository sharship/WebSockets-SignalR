using System;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
                WriteRequestParam(context, env);

                if(context.WebSockets.IsWebSocketRequest) 
                {
                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    Console.WriteLine("WebSocket connected.");
                }
                else
                {
                    Console.WriteLine("Hello from 2nd request delegate - Not WebSocket");
                    await next();
                }
            }
            );

            app.Run(async context => 
            {
                Console.WriteLine("Hello from terminal (Run) Request delegate");
                await context.Response.WriteAsync("Hello from terminal (Run) Request delegate");
            }
            );
        }

        public void WriteRequestParam(HttpContext context, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                Console.WriteLine("Request Method: " + context.Request.Method);
                Console.WriteLine("Request Protocol: " + context.Request.Protocol);

                if (context.Request.Headers != null)
                {
                    Console.WriteLine("Request Headers: ");
                    foreach (var hd in context.Request.Headers)
                    {
                        Console.WriteLine("--> " + hd.Key + ": " + hd.Value);
                    }
                }
            }
        }
    }
}
