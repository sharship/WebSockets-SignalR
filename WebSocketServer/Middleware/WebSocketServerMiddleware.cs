using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebSocketServer.Middleware
{
    public class WebSocketServerMiddleware
    {
        private readonly RequestDelegate _next;  // a private request delegate instance

        // private readonly WebSocketServerConnectionManager _manager = new WebSocketServerConnectionManager();
        private readonly WebSocketServerConnectionManager _manager;

        public WebSocketServerMiddleware(RequestDelegate next, WebSocketServerConnectionManager manager)
        {
            _next = next;
            _manager = manager;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

                var connID = _manager.AddSocket(webSocket);
                
                await SendConnIDAsync(webSocket, connID);

                await ReceiveMessage(webSocket, async (result, buffer) =>
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        Console.WriteLine($"Receive -> Text");
                        Console.WriteLine($"Message: {Encoding.UTF8.GetString(buffer, 0, result.Count)}");
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
                await _next(context);  // call the next Request Delegate
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

        private async Task SendConnIDAsync(WebSocket socket, string connID)
        {
            var buffer = Encoding.UTF8.GetBytes("ConnID: "+ connID);
            await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }


    }
}
