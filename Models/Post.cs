using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tsuHelp.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }


        //[DataType(DataType.Time)]
        //public TimeSpan PostTime { get; set; }

        //[DataType(DataType.Date)]
        //[Column(TypeName = "Date")]
        //public DateTime PostDate { get; set; }


        [ForeignKey("User")]
        public string UserId { get; set; }//автор поста
        public User? User { get; set; }
        public List<TagsInPost>? Tags { get; set; } //список привязанных тегов
        public DateTime? Created { get; set; }
    }
}
