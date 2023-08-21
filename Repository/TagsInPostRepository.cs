using Microsoft.EntityFrameworkCore;
using tsuHelp.Data;
using tsuHelp.Interfaces;
using tsuHelp.Models;

namespace tsuHelp.Repository
{
    public class TagsInPostRepository : ITagsInPostRepository
    {
        private readonly ApplicationDbContext _context;

        public TagsInPostRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(TagsInPost tagsInPost)
        {
            throw new NotImplementedException();
        }

        public List<TagsInPost> GetTagsByPostId(int postId)
        {
            var postTags = _context.TagsInPosts.Where(r => r.PostId == postId);
            return postTags.ToList();
        }

        public void AddMultipleEntities(List<TagsInPost> tagsInPost)
        {
            _context.AddRange(tagsInPost);
            _context.SaveChanges();
        }

        public void DeleteMultipleEntities(List<TagsInPost> tagsInPost)
        {
            _context.RemoveRange(tagsInPost);
            _context.SaveChanges();
        }

        public bool Delete(TagsInPost tagsInPost)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public bool Update(TagsInPost tagsInPost)
        {
            throw new NotImplementedException();
        }
    }
}
