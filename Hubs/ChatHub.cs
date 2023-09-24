using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Query.Internal;
using tsuHelp.Data;
using tsuHelp.Interfaces;
using tsuHelp.Models;


public class ChatHub : Hub
{
    private readonly ApplicationDbContext _context;
    private readonly IUserRepository _userRepository;
    private readonly IUserChatHubConnectionRepository _userConnectionRepository;
    private readonly IChatRepository _chatRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IPostsRepository _postsRepository;

    public ChatHub(ApplicationDbContext context, IUserRepository userRepository, IUserChatHubConnectionRepository userConnectionRepository,
        IChatRepository chatRepository, IMessageRepository messageRepository, IPostsRepository postsRepository)
    {
        _context = context;
        _userRepository = userRepository;
        _userConnectionRepository = userConnectionRepository;
        _chatRepository = chatRepository;
        _messageRepository = messageRepository;
        _postsRepository = postsRepository;
    }

    //public async Task SendMessage(string currentUserId, string mateUserId, string message)//отправка сообщения всем 
    //{
    //    var currentUser = _userRepository.GetUserById(currentUserId).Name;
    //    await Clients.All.SendAsync("ReceiveMessage", currentUser, message);
    //}

    public override async Task OnConnectedAsync()
    {
        string userId = _userRepository.GetCurrentUserId();
        string connectionId = Context.ConnectionId;

        var newConnection = new UserChatHubConnection
        {
            UserId = userId,
            ConntectionId = connectionId,
        };

        _userConnectionRepository.Add(newConnection);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        string userId = _userRepository.GetCurrentUserId();
        
        var connection = _userConnectionRepository.GetConnectionByUserId(userId);
        _userConnectionRepository.Delete(connection);

        string chatId = Context.GetHttpContext().Request.Query["chatId"];

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);

        await base.OnDisconnectedAsync(exception);
    }


    public async Task JoinChat(string chatId)
    {
        // Добавляем пользователя к группе чата
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
    }

    public async Task LeaveChat(string chatId)
    {
        // Удаляем пользователя из группы чата
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
    }

    public Task SendMessageToUser(string senderId, string recieverId, string message, string selectedChatId)//отправка конкретному юзеру
    {
        var recieverConnection = _userConnectionRepository.GetConnectionByUserId(recieverId);
        var senderConnectionId = Context.ConnectionId;

        var sender = _userRepository.GetUserById(senderId);
        var reciever = _userRepository.GetUserById(recieverId);

        //var chat = _chatRepository.GetChatByBothUsersId(senderId, recieverId);
        //bool chatIsInitiallyNull = false;// чат первоначально существовал
        //if (chat == null)// если такого чата еще нет, то добавим его
        //{
        //    chatIsInitiallyNull = true;// если чат == null, то чат не существовал изначально, а был только что создан
        //    chat = new Chat
        //    {
        //        FirstUserId = senderId,
        //        SecondUserId = recieverId,
        //    };
        //    _chatRepository.Add(chat);
        //}

        var chatId = int.Parse(selectedChatId);
        if (_messageRepository.GetAllMessagesByChatId(chatId).Count == 0)// появление чата в списке чатов
        {
            //Clients.Group(selectedChatId).SendAsync("NewChat", sender.Name, sender.Surname, sender.Id);
            if (recieverConnection != null)
            {
                Clients.Client(recieverConnection.ConntectionId).SendAsync("NewChat", selectedChatId, message, sender.Name, sender.Surname, sender.Id);
            }
            Clients.Client(senderConnectionId).SendAsync("NewChat", selectedChatId, message, reciever.Name, reciever.Surname, reciever.Id);

        }

        var newMessage = new Message();
        newMessage.ChatId = chatId;
        newMessage.AuthorId = senderId;
        newMessage.Content = message;
        newMessage.Created = DateTime.Now;
        _messageRepository.Add(newMessage);

        string timeCreated = newMessage.Created.Value.ToString("t");// время отправки сообщения
        string dateCreated = newMessage.Created.Value.ToString("D");// дата отправки сообщения
        string postTitle = " ";
        string postDescription = " ";

        Clients.Client(senderConnectionId).SendAsync("ReceiveMessageInChatsList", message, selectedChatId);//отображение сообщания в списке чатов у отправителя
        if (recieverConnection != null)
        {
            Clients.Client(recieverConnection.ConntectionId).SendAsync("ReceiveMessageInChatsList", message, selectedChatId);//отображение сообщания в списке чатов у получателя
        }

        return Clients.Group(selectedChatId).SendAsync("ReceiveMessage", senderId, message, timeCreated, dateCreated, postTitle, postDescription);
    }// добавить отображение нового чата

    public Task SendMessageByModal(string senderId, string recieverId, string message, string chatId, string postId) // переделать под отправку через группу -------------------------------------
    {
        var recieverConnection = _userConnectionRepository.GetConnectionByUserId(recieverId);

        var sender = _userRepository.GetUserById(senderId);

        var chat = _chatRepository.GetChatByBothUsersId(senderId, recieverId);
        if (chat == null)// если такого чата еще нет, то добавим его
        {
            chat = new Chat
            {
                FirstUserId = senderId,
                SecondUserId = recieverId,
            };
            _chatRepository.Add(chat);
        }

        int postIdInt = int.Parse(postId);
        var post = _postsRepository.GetPostById(postIdInt);

        var newMessage = new Message
        {
            ChatId = chat.Id,
            AuthorId = senderId,
            Content = message,
            Created = DateTime.Now,
            PostId = postIdInt,
        };

        var postTitle = post.Title;
        var postDescription = post.Description;

        _messageRepository.Add(newMessage);
        var timeCreated = newMessage.Created.ToString();

        if (recieverConnection == null)//если получатель не подключен к хабу, сообщение не отправляется в signalR         
            return Task.CompletedTask;
        
        var recieverConnectionId = recieverConnection.ConntectionId;
        return Clients.Client(recieverConnectionId).SendAsync("ReceiveMessage", sender.Name, sender.Surname, message, timeCreated, postTitle, postDescription);//чтобы сообщение отобразилось у получателя   
    }

    public Task GetChatId (string senderId, string recieverId)
    {
        var chatId = _chatRepository.GetChatByBothUsersId(senderId, recieverId);
        return Clients.Client(Context.ConnectionId).SendAsync("TakeChatId", chatId);
    }
}
