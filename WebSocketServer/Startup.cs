using System.Threading;
using System;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

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

                    await ReceiveMessage(webSocket, async (result, buffer) => {
                        if (result.MessageType == WebSocketMessageType.Text)
                        {
                            Console.WriteLine($"Receive -> Text");
                            return;
                        }
                        else if (result.MessageType == WebSocketMessageType.Close)
                        {
                            Console.WriteLine($"Receive -> Close");
                            return;
                        }
                    });
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
    
        private async Task ReceiveMessage(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage) // 2nd parameter handleMessage is an Action Delegate which we use to pass back "result" and "message"
        {
            var buffer = new byte[1024*4];
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer), 
                                                       cancellationToken: CancellationToken.None); // "await" the result from the socket

                handleMessage(result, buffer); // Once "awaited" result is received from socket's async method, we use Action Delegate parameter (handleMessage) to pass back the result and message (stored in the buffer);  
            }
        }

    }
}
