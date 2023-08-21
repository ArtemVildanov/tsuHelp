using Microsoft.Extensions.Hosting;
using tsuHelp.Data;
using tsuHelp.Interfaces;
using tsuHelp.Models;

namespace tsuHelp.Repository
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _context;

        public ChatRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Chat chat)
        {
            _context.Add(chat);
            return Save();
        }

        public bool Delete(Chat chat)
        {
            _context.Remove(chat);
            return Save();
        }

        public List<Chat> GetAllChatsByUserId(string userId)
        {
            var userChatsAsFirstUser = _context.Chats.Where(c => c.FirstUserId == userId).ToList();//если юзер в первой группе
            var userChatsAsSecondUser = _context.Chats.Where(c => c.SecondUserId == userId).ToList();//если юзер во второй группе
            userChatsAsFirstUser.AddRange(userChatsAsSecondUser);
            return userChatsAsFirstUser;
        }

        public Chat GetChatByBothUsersId(string firstUserId, string secondUserId)
        {
            var chatsFirstUserId = _context.Chats.Where(c => c.FirstUserId == firstUserId).ToList();
            var chatsBothUsersId = chatsFirstUserId.Where(c => c.SecondUserId == secondUserId).ToList();
            if(chatsBothUsersId.Count() == 0)
            {
                var chatsByFirstUserId = _context.Chats.Where(c => c.FirstUserId == secondUserId).ToList();
                var chatsByBothUsersId = chatsByFirstUserId.Where(c => c.SecondUserId == firstUserId).ToList();

                if (chatsByBothUsersId.Count() == 0)
                {
                    return null;
                }
                else
                {
                    return chatsByBothUsersId.First();
                }
            }
            else
            {
                return chatsBothUsersId.First();
            }

        }

        public Chat GetChatById(int id)
        {
            var chat = _context.Chats.FirstOrDefault(c => c.Id == id);
            return chat;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Chat chat)
        {
            _context.Update(chat);
            return Save();
        }
    }
}
