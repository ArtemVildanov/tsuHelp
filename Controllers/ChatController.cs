using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using tsuHelp.Data;
using tsuHelp.Interfaces;
using tsuHelp.Models;
using tsuHelp.ViewModels;

namespace tsuHelp.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IPostsRepository _postsRepository;

        public ChatController(ApplicationDbContext context, IUserRepository userRepository, IChatRepository chatRepository, 
            IMessageRepository messageRepository, IPostsRepository postsRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _chatRepository = chatRepository;
            _messageRepository = messageRepository;
            _postsRepository = postsRepository;
        }

        public IActionResult Index(string recieverId = " ", int postId = -1)
        {
            var senderId = _userRepository.GetCurrentUserId();
            var chats = _chatRepository.GetAllChatsByUserId(senderId);

            var chatList = new List<SelectedChatViewModel>();
            foreach (var chat in chats)//заполняем список чатов, 
            {                          //определяем отправителя как авторизированного пользователя,
                                       //получатель - второй пользователь чата
                if (_messageRepository.GetAllMessagesByChatId(chat.Id).Count() != 0)// нужны только чаты в которых есть сообщения
                {
                    User _reciever = null;
                    User _sender = null;

                    if (chat.FirstUserId == senderId)
                    {
                        _sender = _userRepository.GetUserById(chat.FirstUserId);
                        _reciever = _userRepository.GetUserById(chat.SecondUserId);
                    }

                    if (chat.SecondUserId == senderId)
                    {
                        _sender = _userRepository.GetUserById(chat.SecondUserId);
                        _reciever = _userRepository.GetUserById(chat.FirstUserId);
                    }

                    var _chat = new SelectedChatViewModel
                    {
                        SenderId = _sender.Id,
                        Sender = _sender,
                        RecieverId = _reciever.Id,
                        Reciever = _reciever,
                    };
                    chatList.Add(_chat);
                }
            }

            var chatViewModel = new ChatViewModel
            {
                RecieverId = recieverId,
                PostId = postId,
                Chats = chatList,
            };

            return View(chatViewModel);
        }


        public IActionResult Detail(string recieverId, int postId = -1)
        {

            //если пост не был прикреплен к сообщению, то айди поста -1 
            //пост прикреплен к сообщению если был переход по "откликнуться"

            var reciever = _userRepository.GetUserById(recieverId);
            var sender = _userRepository.GetCurrentUser();// отправитель это текущий авторизированный пользователь
            var selectedChat = _chatRepository.GetChatByBothUsersId(reciever.Id, sender.Id);
            

            if(selectedChat == null)
            {
                selectedChat = new Chat
                {
                    FirstUserId = sender.Id,
                    SecondUserId = reciever.Id,
                    FirstUser = sender,
                    SecondUser = reciever
                };

                _chatRepository.Add(selectedChat);
            }

            var chatMessages = _messageRepository.GetAllMessagesByChatId(selectedChat.Id);

            foreach (var message in chatMessages)
            {
                if (message.PostId != null)
                {
                    message.Post = _postsRepository.GetPostById(message.PostId.Value);
                }
            }

            var selectedChatViewModel = new SelectedChatViewModel//текущий авторизированный юзер - sender, получатель всегда reciever
            {
                ChatId = selectedChat.Id,
                Messages = chatMessages,
                Sender = sender,
                Reciever = reciever,
                RecieverId = reciever.Id,
                SenderId = sender.Id,
            };

           

            if (postId != -1)
            {
                selectedChatViewModel.Post = _postsRepository.GetPostById(postId);
            }

            return View(selectedChatViewModel);
        }
    }
}
