using tsuHelp.Data;
using tsuHelp.Interfaces;
using tsuHelp.Models;

namespace tsuHelp.Repository
{
    public class UserSubjectsRepository : IUserSubjectsRepository
    {
        private readonly ApplicationDbContext _context;

        public UserSubjectsRepository(ApplicationDbContext context) 
        {
            _context = context;
        }

        public bool Add(UserSubjects userSubjects)
        {
            _context.Add(userSubjects);
            return Save();
        }

        public List<UserSubjects> GetSubjectsByUserId (string userId)
        {
            var userSubjects = _context.UserSubjects.Where(r => r.UserId == userId).ToList();
            return userSubjects;
        }

        public void AddMultipleEntities(List<UserSubjects> subjects)
        {
            _context.AddRange(subjects);
            _context.SaveChanges();
        }

        public bool Delete(UserSubjects userSubjects)
        {
            throw new NotImplementedException();
        }

        public void DeleteMultipleEntities(List<UserSubjects> userSubjects)
        {
            _context.RemoveRange(userSubjects);
            _context.SaveChanges();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(UserSubjects userSubjects)
        {
            throw new NotImplementedException();
        }
    }
}
