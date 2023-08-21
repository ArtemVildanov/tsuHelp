using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using tsuHelp.Data;
using tsuHelp.Interfaces;
using tsuHelp.Models;

namespace tsuHelp.Repository
{
    public class UserRepository : IUserRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public bool Add(User user)
        {
            throw new NotImplementedException();
        }

        public bool Delete(User user)
        {
            _context.Remove(user);
            return Save();
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserById(string id)
        {
            return _context.Users.Where(m => m.Id == id).SingleOrDefault();
        }

        public User GetCurrentUser()//получить объект User текущего авторизированного пользователя
        {
            var currentId = GetCurrentUserId();
            var currentUser = GetUserById(currentId);
            return currentUser;
        }

        public string GetCurrentUserId()//получить айди текущего авторизированного пользователя
        {
            return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public string GetUserIdByEmail(string email)
        {
            var userId = _context.Users
            .Where(m => m.Email == email)
            .Select(m => m.Id)
            .SingleOrDefault();

            return userId;
        }

        public User GetUserByEmail(string email)
        {
            var user = _context.Users.Where(m => m.Email == email).SingleOrDefault();
            return user;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(User user)
        {
            _context.Update(user);
            return Save();
        }
    }
}
