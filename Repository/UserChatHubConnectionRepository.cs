using System;
using tsuHelp.Data;
using tsuHelp.Interfaces;
using tsuHelp.Models;

namespace tsuHelp.Repository
{
    public class UserChatHubConnectionRepository : IUserChatHubConnectionRepository
    {
        private readonly ApplicationDbContext _context;

        public UserChatHubConnectionRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(UserChatHubConnection userConnection)
        {
            _context.Add(userConnection);
            return Save();
        }

        public bool Delete(UserChatHubConnection userConnection)
        {
            _context.Remove(userConnection);
            return Save();
        }

        public UserChatHubConnection GetConnectionByUserId(string userId)
        {
            var connection = _context.UserChatHubConnections.Where(c => c.UserId == userId).FirstOrDefault();
            return connection;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(UserChatHubConnection userConnection)
        {
            _context.Update(userConnection);
            return Save();
        }
    }
}
