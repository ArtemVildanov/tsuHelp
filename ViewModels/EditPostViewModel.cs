using tsuHelp.Models;

namespace tsuHelp.ViewModels
{
    public class EditPostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }//автор поста
        public User? User { get; set; }
        public List<SubjectViewModel>? Tags { get; set; }
    }
}
