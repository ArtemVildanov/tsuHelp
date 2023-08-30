var connection;

// ‘ункци€ дл€ открыти€ соединени€
async function startConnection() {
    connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
    
    document.getElementById("sendButton").disabled = true;



    // получение сообщений через signalR
    connection.on("ReceiveMessage", function (name, surname, message, dateTime, postTitle, postDescription) {

        // name surname автора сообщени€
        // message - текстовое содержимое сообщени€, dateTime - дата врем€ отправки 
        // postTitle, postDescription - если прикреплен пост, то отображаем его заголовок и текстовое содержимое

        var li = document.createElement("li");
        li.classList.add("message"); // ƒобавл€ем класс "message"
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

    connection.on("NewChat", function (name, surname, id) { // id name surname человека, который отправл€ет вам сообщение

        var li = document.createElement("li");
        document.getElementById("chatsList").appendChild(li);

        var chatElement = `<a href="#" data-reciever-id=${id} class="chat-link">Chat with ${name} ${surname}</a>`
        

        li.innerHTML = chatElement;

    });

    const chatId = document.getElementById("selectedChatId").value; // id чата (группы), по которому будем добавл€ть пользователей в группу чата
    connection.start().then(function () {
        document.getElementById("sendButton").disabled = false;

        // добавл€ем подключившегос€ в чат пользовател€ в группу 
        connection.invoke("JoinChat", chatId).catch(function (err) {
            return console.error(err.toString());
        });

    }).catch(function (err) {
        return console.error(err.toString());
    });


    // отправка сообщени€ по нажатию кнопки
    document.getElementById("sendButton").addEventListener("click", function (event) {
        var senderId = document.getElementById("senderId").value;
        var message = document.getElementById("messageInput").value;
        var recieverId = document.getElementById("recieverId").value;

        connection.invoke("SendMessageToUser", senderId, recieverId, message, chatId).catch(function (err) {
            return console.error(err.toString());
        });

        event.preventDefault();
    });

    // ѕрокрутите список сообщений вниз (в самый низ) при загрузке страницы
    messagesList.scrollTop = messagesList.scrollHeight;

    // ѕри отправке нового сообщени€, прокрутите список сообщений вниз
    function scrollToBottom() {
        messagesList.scrollTop = messagesList.scrollHeight;
    }

}

// ‘ункци€ дл€ закрыти€ соединени€
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


function extractChatContent(html) {
    const parser = new DOMParser();
    const doc = parser.parseFromString(html, 'text/html');
    const chatContent = doc.querySelector('.chat-content').innerHTML;
    return chatContent;
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

        // ќчистка контейнера и вставка нового контента
        document.querySelector('.selected-chat').innerHTML = responseHtml;

        startConnection();
    }

    //const chatLinks = document.querySelectorAll('.chat-link');

    //chatLinks.forEach(link => {
    //    link.addEventListener('click', async (event) => {
    //        event.preventDefault();

    //        closeConnection();

    //        const recieverId = link.getAttribute('data-reciever-id');
    //        const response = await fetch(`/Chat/Detail?recieverId=${recieverId}`, {
    //            headers: {
    //                'X-Requested-With': 'XMLHttpRequest'
    //            }
    //        });
    //        const responseHtml = await response.text();
    //        //const chatContent = extractChatContent(responseHtml);

    //        // ќчистка контейнера и вставка нового контента
    //        document.querySelector('.selected-chat').innerHTML = responseHtml;

    //        startConnection();
    //    });
    //});
});

document.getElementById("chatsList").addEventListener("click", async function (event) {
    // ѕровер€ем, что кликнули по ссылке
    if (event.target.classList.contains("chat-link")) {        

        closeConnection();

        const recieverId = event.target.getAttribute('data-reciever-id');
        const response = await fetch(`/Chat/Detail?recieverId=${recieverId}`, {
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        });
        const responseHtml = await response.text();
        //const chatContent = extractChatContent(responseHtml);

        // ќчистка контейнера и вставка нового контента
        document.querySelector('.selected-chat').innerHTML = responseHtml;

        startConnection();
    }
});
