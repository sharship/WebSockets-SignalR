
"use strict";
var connectionUrl = document.getElementById("connectionUrl");
var connectButton = document.getElementById("connectButton");
var stateLabel = document.getElementById("stateLabel");
var sendMessage = document.getElementById("sendMessage");
var sendButton = document.getElementById("sendButton");
var commsLog = document.getElementById("commsLog");
var closeButton = document.getElementById("closeButton");
var recipients = document.getElementById("recipients");
var connID = document.getElementById("connIDLabel");


connectionUrl.value = "http://localhost:5000/chatHub";

// set up a SignalR Hub connection with server url
var hubConnection = new signalR.HubConnectionBuilder().withUrl(connectionUrl.value).build();

//CONNECT BUTTON
connectButton.onclick = function () {
    stateLabel.innerHTML = "Attempting to connect...";

    // This triggers the OnConnectionAsync event on Server Hub, and server hub sends back ConnID
    hubConnection.start().then(function () {
        updateState();

        commsLog.innerHTML += '<tr>' +
            '<td colspan="3" class="commslog-data">Connection opened</td>' +
            '</tr>';
    });
};

closeButton.onclick = function () {
    if (!hubConnection || hubConnection.state !== "Connected") {
        alert("Hub Not Connected");
    }
    // stop() method only requests closure 
    hubConnection.stop().then(function () {

    });
};

//CLOSE EVENT
// Hub connection really closes
hubConnection.onclose(function (event) {
    updateState();
    commsLog.innerHTML += '<tr>' +
        '<td colspan="3" class="commslog-data">Connection disconnected </td>' +
        '</tr>';
});


hubConnection.on("ReceiveMessage", function (message) {
    commsLog.innerHTML += '<tr>' +
        '<td class="commslog-server">Server</td>' +
        '<td class="commslog-client">Client</td>' +
        '<td class="commslog-data">' + htmlEscape(message) + '</td></tr>';
});

hubConnection.on("ReceiveConnID", function (connid) {
    connID.innerHTML = "ConnID: " + connid;
    commsLog.innerHTML += '<tr>' +
        '<td colspan="3" class="commslog-data">Connection ID Received from Hub</td>' +
        '</tr>';
});

sendButton.onclick = function () {
    var message = constructJSONPayload();
    hubConnection.invoke("SendMessageAsync", message);  // hubConnection object invokes server hub's method of the specified name.
    console.debug("SendMessage Invoked, on ID: " + hubConnection.id);
    commsLog.innerHTML += '<tr>' +
        '<td class="commslog-client">Client</td>' +
        '<td class="commslog-server">Server</td>' +
        '<td class="commslog-data">' + htmlEscape(message) + '</td></tr>';
    event.preventDefault();
};

function htmlEscape(str) {
    return str.toString()
        .replace(/&/g, '&amp;')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#39;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;');
}

function constructJSONPayload() {
    return JSON.stringify({
        "From": connID.innerHTML.substring(8, connID.innerHTML.length),
        "To": recipients.value,
        "Message": sendMessage.value
    });
}


function updateState() {
    function disable() {
        sendMessage.disabled = true;
        sendButton.disabled = true;
        closeButton.disabled = true;
        recipients.disabled = true;

    }
    function enable() {
        sendMessage.disabled = false;
        sendButton.disabled = false;
        closeButton.disabled = false;
        recipients.disabled = false;

    }
    connectionUrl.disabled = true;
    connectButton.disabled = true;
    if (!hubConnection) {
        disable();
    } else {
        switch (hubConnection.state) {
            case "Disconnected":
                stateLabel.innerHTML = "Disconnected";
                connID.innerHTML = "ConnID: N/a"
                disable();
                connectionUrl.disabled = false;
                connectButton.disabled = false;
                break;
            case "Connecting":
                stateLabel.innerHTML = "Connecting...";
                disable();
                break;
            case "Connected":
                stateLabel.innerHTML = "Connected";
                enable();
                break;
            default:
                stateLabel.innerHTML = "Unknown WebSocket State: " + htmlEscape(hubConnection.state);
                disable();
                break;
        }
    }
}

