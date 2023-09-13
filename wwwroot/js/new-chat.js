"use strict"

var connection;

// ������� ��� �������� ����������
async function startConnection() {
    connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
    
    document.getElementById("sendButton").disabled = true;

    connection.on("ReceiveMessageInChatsList", function (message, chatId) {
        var element = document.getElementById(chatId);
        if (element) {
            element.textContent = message;
        }
    });

    // ��������� ��������� ����� signalR
    let lastMessageDate = "";// ���������� ���� ���������� ���������
    connection.on("ReceiveMessage", function (senderId, message, time, date, postTitle, postDescription) {

        // senderId ������ ���������
        // message - ��������� ���������� ���������, dateTime - ���� ����� �������� 
        // postTitle, postDescription - ���� ���������� ����, �� ���������� ��� ��������� � ��������� ����������

        var li = document.createElement("li");
        li.classList.add("messages-positionary"); // ��������� ����� 
        document.getElementById("messagesList").appendChild(li);

        let messageDate = "";
        if (lastMessageDate !== date) { // ���������� ���� ����������� ��������� � ��������� ����������� �����
            lastMessageDate = document.getElementById("lastMessageDate").value;
            if (lastMessageDate !== date) { // ���������� ���� ����������� ��������� � ����� ���������� ��������� �� ��
                messageDate = `<li class="small center">${date}</li>`;
                lastMessageDate = date;
            }
        }

        let messageContent;
        const currentUserId = document.getElementById("currentUserId").value; // ���� ������������, ���������� ��������

        if (senderId === currentUserId) { // ���� ����������� ��������� - ������� ������������ - �� ��������� ������ � �����
            messageContent = `<li class="message blue right"> ${message} </li>
                              <li class="small right mb-3"> ${time} </li>`;
        }

        if (senderId !== currentUserId) { // ���� ����������� �� ������� ������������ - �� ��������� ����� � �����
            messageContent = `<li class="message gray left"> ${message} </li>
                              <li class="small left mb-3"> ${time} </li>`;
        }
        

        if (postTitle !== " " && postDescription !== " ") {
            var postContent = `<div class="post"><strong>${postTitle}</strong><br>${postDescription}</div>`;
            li.innerHTML = messageDate + postContent + messageContent;
        } else {
            li.innerHTML = messageDate + messageContent;
        }

        scrollToBottom();
    });

    connection.on("NewChat", function (chatId, latestMessage, name, surname, id) { // id name surname ��������, ������� ���������� ��� ���������

        var li = document.createElement("li");
        li.classList.add("list-group-item");
        li.classList.add("list-group-item-action");
        document.getElementById("chatsList").appendChild(li);

        //var chatElement = `<a href="#" data-reciever-id=${id} class="chat-link">Chat with ${name} ${surname}</a>`
        var chatElement = `<a href="#" data-reciever-id=${id} class="chat-link">
                               <div class="d-flex w-100 align-items-center justify-content-between">
                                   <strong class="mb-1">${name} ${surname}</strong>
                               </div>
                               <div id=${chatId} class="col-10 mb-1 small latest-message">${latestMessage}</div>
                           </a>`

        li.innerHTML = chatElement;

    });

    const chatId = document.getElementById("selectedChatId").value; // id ���� (������), �� �������� ����� ��������� ������������� � ������ ����
    connection.start().then(function () {
        document.getElementById("sendButton").disabled = false;

        // ��������� ��������������� � ��� ������������ � ������ 
        connection.invoke("JoinChat", chatId).catch(function (err) {
            return console.error(err.toString());
        });

    }).catch(function (err) {
        return console.error(err.toString());
    });


    // �������� ��������� �� ������� ������
    document.getElementById("sendButton").addEventListener("click", function (event) {
        var senderId = document.getElementById("senderId").value;
        var message = document.getElementById("messageInput").value;
        var recieverId = document.getElementById("recieverId").value;

        if (message !== "") {
            connection.invoke("SendMessageToUser", senderId, recieverId, message, chatId).catch(function (err) {
                return console.error(err.toString());
            });
        }

        clearTextarea();

        event.preventDefault();
    });

    // ���������� ������ ��������� ���� (� ����� ���) ��� �������� ��������
    messagesList.scrollTop = messagesList.scrollHeight;

    // ��� �������� ������ ���������, ���������� ������ ��������� ����
    function scrollToBottom() {
        messagesList.scrollTop = messagesList.scrollHeight;
    }

}

// ������� ��� �������� ����������
async function closeConnection() {
    if (connection) {

        await leaveChat();
        await connection.stop().catch(function (err) {
            console.error(err.toString());
        });
    }
}

async function leaveChat() {
    var chatId = document.getElementById("selectedChatId").value;
    await connection.invoke("LeaveChat", chatId).catch(function (err) {
        console.error(err.toString());
    });
}

function clearTextarea() { // ������� ���� ����� ��� �������� ���������
    // �������� ������� textarea �� ��� ID
    var textarea = document.getElementById("messageInput");

    // ������� ���������� textarea
    textarea.value = "";
}

document.addEventListener("DOMContentLoaded", async function () {
    const initialRecieverId = document.getElementById('initialRecieverId').value;
    if (initialRecieverId !== "") {

        closeConnection();

        const response = await fetch(`/Chat/Detail?recieverId=${initialRecieverId}`, {
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        });
        const responseHtml = await response.text();
        //const chatContent = extractChatContent(responseHtml);

        // ������� ���������� � ������� ������ ��������
        document.querySelector('.messages-box').innerHTML = responseHtml;

        startConnection();
    }

});

document.getElementById("chatsList").addEventListener("click", async function (event) {
    // ���������, ��� �������� �� ������
    if (event.target.closest(".chat-link")) {        

        closeConnection();

        const recieverId = event.target.closest(".chat-link").getAttribute('data-reciever-id');
        const response = await fetch(`/Chat/Detail?recieverId=${recieverId}`, {
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        });
        const responseHtml = await response.text();
        //const chatContent = extractChatContent(responseHtml);

        // ������� ���������� � ������� ������ ��������
        document.querySelector('.messages-box').innerHTML = responseHtml;

        startConnection();
    }
});
