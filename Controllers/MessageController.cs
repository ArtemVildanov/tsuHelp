using Microsoft.AspNetCore.Mvc;
using tsuHelp.Data;
using tsuHelp.Interfaces;
using tsuHelp.Models;
using tsuHelp.ViewModels;

namespace tsuHelp.Controllers
{
    public class MessageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IPostsRepository _postsRepository;

        public MessageController(ApplicationDbContext context, IUserRepository userRepository, IChatRepository chatRepository, 
            IMessageRepository messageRepository, IPostsRepository postsRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _chatRepository = chatRepository;
            _messageRepository = messageRepository;
            _postsRepository = postsRepository;
        }
        public IActionResult Index(string recieverId, int postId = -1)
        {
            var reciever = _userRepository.GetUserById(recieverId);
            var sender = _userRepository.GetCurrentUser();
            var chat = _chatRepository.GetChatByBothUsersId(reciever.Id, sender.Id);
            if(chat == null)
            {
                chat = new Chat
                {
                    FirstUserId = sender.Id,
                    SecondUserId = reciever.Id,
                    FirstUser = sender,
                    SecondUser = reciever
                };

                _chatRepository.Add(chat);
            }

            var chatMessages = _messageRepository.GetAllMessagesByChatId(chat.Id);

            foreach (var message in chatMessages)
            {
                if (message.PostId != null)
                {
                    message.Post = _postsRepository.GetPostById(message.PostId.Value);
                }
            }

            var chatViewModel = new ChatViewModel//текущий авторизированный юзер - sender, получатель всегда reciever
            {
                Messages = chatMessages,
                Sender = sender,
                Reciever = reciever,
                RecieverId = reciever.Id,
                SenderId = sender.Id,
            };

            if (postId != -1)
            {
                chatViewModel.Post = _postsRepository.GetPostById(postId);    
            }

            return View(chatViewModel);
        }
    }
}
