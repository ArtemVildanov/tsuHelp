using Microsoft.EntityFrameworkCore;
using tsuHelp.Data;
using tsuHelp.Interfaces;
using tsuHelp.Models;

namespace tsuHelp.Repository
{
    public class PostsRepository : IPostsRepository
    {
        private readonly ApplicationDbContext _context;
        public PostsRepository(ApplicationDbContext context) 
        {
            _context = context;
        }

        public bool Add(Post post)
        {
            _context.Add(post);
            return Save();
        }

        public bool Delete(Post post)
        {
            _context.Remove(post);
            return Save();
        }

        public void DeleteMultipleEntities(List<Post> posts)
        {
            _context.RemoveRange(posts);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Post>> GetAll()
        {
            return await _context.Posts.ToListAsync();
        }

        public Post GetPostById(int id)
        {
            return _context.Posts.SingleOrDefault(p => p.Id == id);
        }

        public Task<Post> GetByIdAsyncNoTracking(int id)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Post post)
        {
            _context.Update(post);
            return Save();
        }

        public List<Post> GetPostsByUserId(string userId)
        {
            var userPosts = _context.Posts.Where(r => r.UserId == userId).ToList();
            return userPosts;
        }
    }
}
