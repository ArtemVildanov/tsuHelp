using System.ComponentModel.DataAnnotations.Schema;

namespace tsuHelp.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string Content { get; set; }

        [ForeignKey("Chat")]
        public int ChatId { get; set; }
        public Chat? Chat { get; set; }

        [ForeignKey("User")]
        public string AuthorId { get; set; }
        public User? Author { get; set; }

        [ForeignKey("Post")]
        public int? PostId { get; set; }
        public Post? Post { get; set; }

        public DateTime? Created { get; set; }
    }
}
