var connection;

// ������� ��� �������� ����������
async function startConnection() {
    connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
    
    document.getElementById("sendButton").disabled = true;



    // ��������� ��������� ����� signalR
    connection.on("ReceiveMessage", function (name, surname, message, dateTime, postTitle, postDescription) {

        // name surname ������ ���������
        // message - ��������� ���������� ���������, dateTime - ���� ����� �������� 
        // postTitle, postDescription - ���� ���������� ����, �� ���������� ��� ��������� � ��������� ����������

        var li = document.createElement("li");
        li.classList.add("message"); // ��������� ����� "message"
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

    connection.on("NewChat", function (name, surname, id) { // id name surname ��������, ������� ���������� ��� ���������

        var li = document.createElement("li");
        document.getElementById("chatsList").appendChild(li);

        var chatElement = `<a href="#" data-reciever-id=${id} class="chat-link">Chat with ${name} ${surname}</a>`
        

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

        connection.invoke("SendMessageToUser", senderId, recieverId, message, chatId).catch(function (err) {
            return console.error(err.toString());
        });

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

        // ������� ���������� � ������� ������ ��������
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

    //        // ������� ���������� � ������� ������ ��������
    //        document.querySelector('.selected-chat').innerHTML = responseHtml;

    //        startConnection();
    //    });
    //});
});

document.getElementById("chatsList").addEventListener("click", async function (event) {
    // ���������, ��� �������� �� ������
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

        // ������� ���������� � ������� ������ ��������
        document.querySelector('.selected-chat').innerHTML = responseHtml;

        startConnection();
    }
});
