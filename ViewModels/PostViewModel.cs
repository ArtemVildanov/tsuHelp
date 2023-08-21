using System.ComponentModel.DataAnnotations.Schema;
using tsuHelp.Models;

namespace tsuHelp.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }//автор поста
        public string CurrentUserId { get; set; }//айди текущего пользователя
        public User? User { get; set; }
        public List<TagsInPost>? Tags { get; set; } //список привязанных тегов
    }
}
