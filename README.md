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

### .NET Middleware & Asynchronous programming

#### Middleware
Middlewares are different components that are added to _Request Pipline_, to handle requests.  

![Middleware request delegate pipline](https://github.com/sharship/WebSockets-SignalR/blob/main/imgs/request-delegate-pipeline.png "Middleware request delegate pipline")  
Fig. 4 Middleware request delegate pipline  

_Request Delegates_ are used to build pipline, using one of following extension methods:  
- _Run_: used at the **end** of pipline to terminate request handling;  
- _Map_: used to **branch** the pipline;  
- _Use_: used to either **shortcut** (by not calling _next_ request delegate), or **chain up** next Request Delegate in pipline (by calling _next_ request delegate);  

#### Asynchronous programming
Asynchronous methods:  
- use _async_ modifier;  
- return a _Task/Task<T>_ \(with optional type\);  
- usually have _"Async"_ as name suffix as convention;  

- Returned tasks represent ongoing work, and will eventually return with required result or exception;

Await  
- _Await_ statement can be used in Async methods to designate "suspension point", i.e. it can't continue past this point until the awaited process is complete;
- The "awaited" method itself must also be asynchronous;
- While "awaiting", control is returned back to the caller of the Async method;

![Task asynchronous programming](https://github.com/sharship/WebSockets-SignalR/blob/main/imgs/async-program.png "Task asynchronous programming")  
Fig. 5 Task asynchronous programming  

1. As shown in Fig. 5, _GetStringAsync\(url\)_ returns a Task, and it is assigned to a variable _getStringTask_;  
2. Independent work can keep going while async _getStringTask_ waiting its Task\<string\> result;  
3. When the result of _getStringTask_ is needed, but may not have been returned, a _await_ statement is put in front of it to "await" Task\<string\> result, and return control back to method caller.  


#### Custom ASP.NET Core Middleware
Middleware is generally  
- encapsulated in a _middleware class_;  
- and then exposed with an _extension method_;  

##### Middleware Class  
The middleware class **must** include:  
- A public constructor with a parameter of type _RequestDelegate_;  
- A public method named __Invoke__ or __InvokeAsync__, this method **must**:  
1. Return a **Task**;  
2. Accept a first parameter of type _HttpContext__;  

Additional parameters for constructor and/or Invoke/InvokeAsync are populated by _Dependency Injection\(DI\)_.  
![Build custom middleware](https://github.com/sharship/WebSockets-SignalR/blob/main/imgs/custom-middleware.png "Build custom middleware")  
Fig. 6 Build custom middleware  

##### Middleware Extension Method  
The extension method _exposes_ the middleware through _IApplicationBuilder_:  
![Expose custom middleware through IApplicationBuilder extension method.](https://github.com/sharship/WebSockets-SignalR/blob/main/imgs/expose-middleware.png "Expose custom middleware through IApplicationBuilder extension method.")  
Fig. 7 Expose custom middleware through IApplicationBuilder extension method.  


### Phase 1: Get connected 
On client side, initiate a WebSocket instance:  
`   socket = new WebSocket(connectionUrl.value);` 
`   socket.onopen = function (event) {}` 
`   socket.onmessage = function (event) {}`  
`   socket.onclose = function (event) {}`  
`   socket.onerror = function (event) {}`  
  
`   connectionUrl.value = "ws://localhost:5000";`  

On server side, add middleware to _Startup.Configure()_ method:  
`   app.UseWebSockets();`  
`   WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();`  

![Middleware request delegate pipline with WebSocket](https://github.com/sharship/WebSockets-SignalR/blob/main/imgs/websocket-pipline.jpg "Middleware request delegate pipline with WebSocket")  
Fig. 8 Middleware request delegate pipline with WebSocket  

### Phase 2: Send messages 
#### Client: 
- Send a message to WebSocket \(Client to Server\);  

#### Server: 
- Add an _Asynchronous_ method to receive message, to receive _events_ on our WebSocket;  
- Call this new Async method from within "WebSocket Request Delegate";  

### Phase 3: Upgrade Our Middleware
- Move the Request Delegate in _Startup.Configure\(\)_ to a separate "Middleware" class;  
- Expose customized Middleware class via _IApplicationBuilder_;  


