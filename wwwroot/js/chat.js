"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (name, surname, message, dateTime, postTitle, postDescription) {
    var li = document.createElement("li");
    li.classList.add("message"); // Добавляем класс "message"
    document.getElementById("messagesList").appendChild(li);

    var messageContent = `<strong>${name} ${surname}</strong><br>${message}<br><span class="message-time">${dateTime}</span>`;

    if (postTitle !== " " && postDescription !== " ") {
        var postContent = `<div class="post"><strong>${postTitle}</strong><br>${postDescription}</div>`;
        li.innerHTML = postContent + messageContent;
    } else {
        li.innerHTML = messageContent;
    }

    scrollToBottom();
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

var isPostIdSet = false;
document.getElementById("sendButton").addEventListener("click", function (event) {
    var senderId = document.getElementById("senderId").value;
    var message = document.getElementById("messageInput").value;
    var recieverId = document.getElementById("recieverId").value;
    var postId = document.getElementById("postId").value;

    var postIdValue = isPostIdSet ? "-1" : postId;//если есть прикрепленный к сообщению пост, то его айди отправляется только первым сообщением,
                                                  //если пост не был прикреплен к сообщению, то в хаб отправляем айди поста -1

    connection.invoke("SendMessageToUser", senderId, recieverId, message, postIdValue).catch(function (err) {
        return console.error(err.toString());
    });

    if (!isPostIdSet) {
        isPostIdSet = true;
    }

    event.preventDefault();
});

document.addEventListener("DOMContentLoaded", function () {// отображение прикрепленного поста над текстовым полем
    var postIdInput = document.getElementById("postId");
    var postBox = document.getElementById("postBox");
    var messageInput = document.getElementById("messageInput");
    var sendButton = document.getElementById("sendButton");

    if (postIdInput.value !== "-1") {
        postBox.style.display = "block";
    }

    sendButton.addEventListener("click", function () {
        if (postIdInput.value !== "-1") {
            postBox.style.display = "none";
        }
    });
});

const chatLinks = document.querySelectorAll('.chat-list a');

chatLinks.forEach(link => {// перемещение между чатами в списке чатов
    link.addEventListener('click', async (event) => {
        event.preventDefault();
        const recieverId = document.getElementById('chatRecieverId').value; // Предполагается, что вы добавите атрибут с id получателя к вашей ссылке
        const response = await fetch(`/Chat/Detail?recieverId=${recieverId}`);
        const chatContent = await response.text();
        document.querySelector('.selected-chat').innerHTML = chatContent;
    });
});

// Получите элемент списка сообщений
//var messagesList = document.getElementById("messagesList");

// Прокрутите список сообщений вниз (в самый низ) при загрузке страницы
messagesList.scrollTop = messagesList.scrollHeight;

// При отправке нового сообщения, прокрутите список сообщений вниз
function scrollToBottom() {
    messagesList.scrollTop = messagesList.scrollHeight;
}

// Вызов функции прокрутки при отправке нового сообщения
//document.getElementById("sendButton").addEventListener("click", scrollToBottom);