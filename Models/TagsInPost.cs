using System.ComponentModel.DataAnnotations.Schema;

namespace tsuHelp.Models
{
    public class TagsInPost
    {
        public int Id { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }
        public Post? Post { get; set; }
        public string Tag { get; set; } = null!;
    }
}
