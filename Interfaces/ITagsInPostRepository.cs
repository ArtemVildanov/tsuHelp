using tsuHelp.Models;

namespace tsuHelp.Interfaces
{
    public interface ITagsInPostRepository
    {
        bool Add(TagsInPost tagsInPost);

        public void AddMultipleEntities(List<TagsInPost> tagsInPost);

        List<TagsInPost> GetTagsByPostId(int postId);
        
        void DeleteMultipleEntities(List<TagsInPost> tagsInPost);

        bool Update(TagsInPost tagsInPost);

        bool Delete(TagsInPost tagsInPost);

        bool Save();
    }
}
