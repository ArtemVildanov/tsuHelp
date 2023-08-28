var connection;

// Функция для открытия соединения
function startConnection() {
    connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

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


    document.getElementById("sendButton").addEventListener("click", function (event) {
        var senderId = document.getElementById("senderId").value;
        var message = document.getElementById("messageInput").value;
        var recieverId = document.getElementById("recieverId").value;

        connection.invoke("SendMessageToUser", senderId, recieverId, message).catch(function (err) {
            return console.error(err.toString());
        });

        event.preventDefault();
    });

    //document.addEventListener("DOMContentLoaded", function () {// отображение прикрепленного поста над текстовым полем
    //    var postIdInput = document.getElementById("postId");
    //    var postBox = document.getElementById("postBox");
    //    var messageInput = document.getElementById("messageInput");
    //    var sendButton = document.getElementById("sendButton");
    //    if (postIdInput.value !== "-1") {
    //        postBox.style.display = "block";
    //    }
    //    sendButton.addEventListener("click", function () {
    //        if (postIdInput.value !== "-1") {
    //            postBox.style.display = "none";
    //        }
    //    });
    //});

    // Получите элемент списка сообщений
    //var messagesList = document.getElementById("messagesList");

    // Прокрутите список сообщений вниз (в самый низ) при загрузке страницы
    messagesList.scrollTop = messagesList.scrollHeight;

    // При отправке нового сообщения, прокрутите список сообщений вниз
    function scrollToBottom() {
        messagesList.scrollTop = messagesList.scrollHeight;
    }

}

// Функция для закрытия соединения
function closeConnection() {
    if (connection) {
        connection.stop().catch(function (err) {
            console.error(err.toString());
        });
    }
}


function extractChatContent(html) {
    const parser = new DOMParser();
    const doc = parser.parseFromString(html, 'text/html');
    const chatContent = doc.querySelector('.chat-content').innerHTML;
    return chatContent;
}

document.addEventListener("DOMContentLoaded", async function () {
    const _recieverId = document.getElementById('recieverId').value;
    if (_recieverId !== "") {
        closeConnection();

        const response = await fetch(`/Chat/Detail?recieverId=${_recieverId}`, {
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        });
        const responseHtml = await response.text();
        //const chatContent = extractChatContent(responseHtml);

        // Очистка контейнера и вставка нового контента
        document.querySelector('.selected-chat').innerHTML = responseHtml;

        startConnection();
    }

    const chatLinks = document.querySelectorAll('.chat-link');

    chatLinks.forEach(link => {
        link.addEventListener('click', async (event) => {
            event.preventDefault();

            closeConnection();

            const recieverId = link.getAttribute('data-reciever-id');
            const response = await fetch(`/Chat/Detail?recieverId=${recieverId}`, {
                headers: {
                    'X-Requested-With': 'XMLHttpRequest'
                }
            });
            const responseHtml = await response.text();
            //const chatContent = extractChatContent(responseHtml);

            // Очистка контейнера и вставка нового контента
            document.querySelector('.selected-chat').innerHTML = responseHtml;

            startConnection();
        });
    });
});