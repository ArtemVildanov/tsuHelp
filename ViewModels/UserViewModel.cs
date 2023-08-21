using tsuHelp.Models;

namespace tsuHelp.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string CurrentUserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string? Description { get; set; }
        public List<UserSubjects>? Subjects { get; set; }
        public List<Post>? Posts { get; set; }
        public string? University { get; set; }
        public string? Faculty { get; set; }
        public int? CourseNum { get; set; }
        public IFormFile? ProfileAvatar { get; set; }

    }
}
