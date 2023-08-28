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

        connection.onclose(() => {
            console.log("Connection closed unexpectedly.");
        });

        await connection.start();

        // В этом месте можно добавить код для отслеживания состояния соединения.
        // Например, вы можете использовать connection.state и проверять, что оно 'Connected' перед отправкой.
        if (connection.state === signalR.HubConnectionState.Connected) {
            await connection.invoke("SendMessageByModal", senderId, recieverId, message, postId);
        } else {
            console.log("Connection is not in the 'Connected' state.");
        }

        await connection.stop();
    } catch (err) {
        console.error(err.toString());
    }

    event.preventDefault();
});

