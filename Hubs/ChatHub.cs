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

        await base.OnDisconnectedAsync(exception);
    }

    public Task SendMessageToUser(string senderId, string recieverId, string message)//отправка конкретному юзеру
    {        
        var recieverConnection = _userConnectionRepository.GetConnectionByUserId(recieverId);

        var sender = _userRepository.GetUserById(senderId);
        var senderConnectionId = Context.ConnectionId;

        var chatId = _chatRepository.GetChatByBothUsersId(senderId, recieverId).Id;

        var newMessage = new Message();
        newMessage.ChatId = chatId;
        newMessage.AuthorId = senderId;
        newMessage.Content = message;
        newMessage.Created = DateTime.Now;
        
        _messageRepository.Add(newMessage);


        var timeCreated = newMessage.Created.ToString();
        string postTitle = " ";
        string postDescription = " ";

        if (recieverConnection == null)//если получатель не подключен к хабу, то сообщение загружается в бд и отображается только у отправителя
        {
            return Clients.Client(senderConnectionId).SendAsync("ReceiveMessage", sender.Name, sender.Surname, message, timeCreated, postTitle, postDescription);
        }
        var recieverConnectionId = recieverConnection.ConntectionId;

        Clients.Client(senderConnectionId).SendAsync("ReceiveMessage", sender.Name, sender.Surname, message, timeCreated, postTitle, postDescription);//чтобы сообщение отобразилось и у отправителя
        return Clients.Client(recieverConnectionId).SendAsync("ReceiveMessage", sender.Name, sender.Surname, message, timeCreated, postTitle, postDescription);//чтобы сообщение отобразилось у получателя
    }

    public Task SendMessageByModal(string senderId, string recieverId, string message, string postId)
    {
        var recieverConnection = _userConnectionRepository.GetConnectionByUserId(recieverId);

        var sender = _userRepository.GetUserById(senderId);

        var chatId = _chatRepository.GetChatByBothUsersId(senderId, recieverId).Id;

        int postIdInt = int.Parse(postId);
        var post = _postsRepository.GetPostById(postIdInt);

        var newMessage = new Message
        {
            ChatId = chatId,
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
}
