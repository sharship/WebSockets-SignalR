## 1. HTTP, XMLHttpRequest & WebSockets 
### HTTP 
![HTTP request-response cycle](https://github.com/sharship/WebSockets-SignalR/blob/main/imgs/Http.PNG "HTTP request-response cycle")  
Fig. 1 HTTP request-response cycle  

- Request - Response Cycle; 
- Synchronous; 
- Stateless; 

### XMLHttpRequest\(XHR\) 
Need to "poll for updates" via Request/Response cycle \(keep asking Server what to update\). 
- Partial UI updates; 
- Asynchronous; 
- Request - Response based; 

### Long Polling with XHR 
- XHR request is made to server; 
- "Hold" connection open until server completes its work; 
- Immediately send response back to client; 
- Rinse & Repeat; 

### WebSockets 
Establish and keep a __persisted "full-duplex"__ connection. 
![WebSockets Establishment](https://github.com/sharship/WebSockets-SignalR/blob/main/imgs/WebSockets.PNG "WebSockets Establishment")  
Fig. 2 WebSockets Establishment  

- Start with HTTP request to establish a _persistent_ WebSocket connection; 
- Bi-directional; 

## 2. WebSocket API  
### Events 
- __open__: Fired when a WebSocket connection is opened; 
- __message__: Fired when data is received through a WebSocket; 
- __close__: Fired when a WebSocket connection is closed; 
- __error__: Fired when a WebSocket connection is closed due to an error; 

### Methods 
- __send\(\)__: Enqueues data to be transmitted; 
- __close\(\)__: Close teh connection; 

## WebSocket App Architecture 
![WebSocket App Architecture](https://github.com/sharship/WebSockets-SignalR/blob/main/imgs/WebSocket%20App%20Architecture.png "WebSocket App Architecture")  
Fig. 3 WebSocket App Architecture  

The custom .Net classes to build: 
- WebSocketServerMiddleware\(pipline\); 
- WebSocketServerMiddleExtensions\(for optimization\); 
- WebSocketServerConnectionManager\(for routing?\); 

### Overview of building process 
#### Phase 1: Get connected 
- Build both client and server components; 
- Establish a WebSocket connection; 
- Asynchronous programming in .Net; 
- Reqest pipline & Request delegates; 

#### Phase 2: Send messages 
Send message from client to server, i.e. HTML/JavaScript to .Net. 

#### Phase 3: Upgrade middleware 
- Build custome middle to handle WS connection; 
- Split out Request Delegate into separate class; 

#### Phase 4: Add a manager 
- Manage multiple WS connections; 
- Send messages between clients, within multiple WS connections, e.g. group chat; 
- Introduce "Connection ID" concept; 

#### Phase 5: Add message router 
By adding a router, we could: 
- send message to targeted individual clients; 
- or broadcast to all active connections; 

### Phase 1: 
On client side, initiate a WebSocket instance:  
`   socket = new WebSocket(connectionUrl.value);`
