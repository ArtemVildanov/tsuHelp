const openMessageModalButton = document.getElementById('openMessageModal');
const messageModal = document.getElementById('messageModal');
const closeMessageModalButton = document.getElementById('closeMessageModal');

openMessageModalButton.addEventListener('click', () => {
    messageModal.style.display = 'block';
});

// При нажатии на крестик или за пределы модального окна, модальное окно закрывается
closeMessageModalButton.addEventListener('click', () => {
    messageModal.style.display = 'none';
});

window.addEventListener('click', (event) => {
    if (event.target === messageModal) {
        messageModal.style.display = 'none';
    }
});

//отправка сообщения из модального окна
document.getElementById("sendButton").addEventListener("click", async function (event) {
    try {
        connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

        var senderId = document.getElementById("senderId").value;
        var message = document.getElementById("messageInput").value;
        var recieverId = document.getElementById("recieverId").value;
        var postId = document.getElementById("postId").value;

        connection.start().catch(function (err) {// запускаем соединение
            console.error(err.toString());
        });
     
        connection.invoke("GetChatId", senderId, recieverId).catch(function (err) {// запрашиваем chatId
            return console.error(err.toString());
        });

        let chatId;
        connection.on("TakeChatId", function (incomeChatId) {// получаем chatId
            chatId = incomeChatId;
        })

        connection.invoke("JoinChat", chatId).catch(function (err) {// подключаемся к группе чата
            return console.error(err.toString());
        });

        if (message !== "") {
            connection.invoke("SendMessageByModal", senderId, recieverId, message, chatId, postId).catch(function (err) {// отправляем сообщение
                return console.error(err.toString());
            });
        }

        connection.invoke("LeaveChat", chatId).catch(function (err) {// отключаемся от группы
            console.error(err.toString());
        });

        connection.stop().catch(function (err) {// закрываем соединение
            console.error(err.toString());
        });

    } catch (err) {
        console.error(err.toString());
    }

    event.preventDefault();
});

