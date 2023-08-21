using Microsoft.Extensions.Hosting;
using System;
using tsuHelp.Data;
using tsuHelp.Interfaces;
using tsuHelp.Models;

namespace tsuHelp.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Message message)
        {
            _context.Add(message);
            return Save();
        }

        public bool Delete(Message message)
        {
            _context.Remove(message);
            return Save();
        }

        public void DeleteMultipleEntities(List<Message> messages)
        {
            _context.RemoveRange(messages);
            _context.SaveChanges();
        }

        public List<Message> GetAllMessagesByChatId(int chatId)
        {
            var messages = _context.Messages.Where(c => c.ChatId == chatId).ToList();
            return messages;
        }

        public List<Message> GetAllMessagesByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Message message)
        {
            _context.Update(message);
            return Save();
        }
    }
}
