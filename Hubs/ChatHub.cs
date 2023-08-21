using Microsoft.AspNetCore.SignalR;
using tsuHelp.Data;
using tsuHelp.Interfaces;
using tsuHelp.Models;

//namespace BlazorServerSignalRApp.Server.Hubs;

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

    public Task SendMessageToUser(string senderId, string recieverId, string message, string postId)//отправка конкретному юзеру
    {        
        var recieverConnection = _userConnectionRepository.GetConnectionByUserId(recieverId);

        var sender = _userRepository.GetUserById(senderId);
        var senderConnectionId = Context.ConnectionId;

        var chatId = _chatRepository.GetChatByBothUsersId(senderId, recieverId).Id;

        var post = new Post();
        int postIdInt = int.Parse(postId);
        string postTitle = " ";
        string postDescription = " ";

        var newMessage = new Message();
        
        if (postIdInt != -1)//если есть прикрепленный пост 
        {
            newMessage.ChatId = chatId;
            newMessage.AuthorId = senderId;
            newMessage.Content = message;
            newMessage.Created = DateTime.Now;
            newMessage.PostId = postIdInt;

            post = _postsRepository.GetPostById(postIdInt);
            postTitle = post.Title;
            postDescription = post.Description;
        }
        else
        {
            newMessage.ChatId = chatId;
            newMessage.AuthorId = senderId;
            newMessage.Content = message;
            newMessage.Created = DateTime.Now;
        }

        
        _messageRepository.Add(newMessage);
        var timeCreated = newMessage.Created.ToString();

        if (recieverConnection == null)//если получатель не подключен к хабу, то сообщение загружается в бд и отображается только у отправителя
        {
            return Clients.Client(senderConnectionId).SendAsync("ReceiveMessage", sender.Name, sender.Surname, message, timeCreated, postTitle, postDescription);
        }
        var recieverConnectionId = recieverConnection.ConntectionId;

        Clients.Client(senderConnectionId).SendAsync("ReceiveMessage", sender.Name, sender.Surname, message, timeCreated, postTitle, postDescription);//чтобы сообщение отобразилось и у отправителя
        return Clients.Client(recieverConnectionId).SendAsync("ReceiveMessage", sender.Name, sender.Surname, message, timeCreated, postTitle, postDescription);//чтобы сообщение отобразилось у получателя
    }
}
