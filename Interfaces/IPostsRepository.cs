using tsuHelp.Models;

namespace tsuHelp.Interfaces
{
    public interface IPostsRepository
    {
        Task<IEnumerable<Post>> GetAll();

        Post GetPostById(int id);

        Task<Post> GetByIdAsyncNoTracking(int id);

        List<Post> GetPostsByUserId(string userId);

        bool Add(Post post);

        bool Update(Post post);

        bool Delete(Post post);

        void DeleteMultipleEntities(List<Post> posts);

        bool Save();
    }
}
