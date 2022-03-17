"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/mMHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("NewExpense", function (expense) {
    
    var encodedMsg = JSON.stringify(expense);
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});