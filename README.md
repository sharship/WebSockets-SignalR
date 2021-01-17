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

