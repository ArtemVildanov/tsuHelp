using tsuHelp.Models;

namespace tsuHelp.ViewModels
{
    public class EditProfileViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string? Description { get; set; }
        public List<SubjectViewModel>? Subjects { get; set; }
        public string? University { get; set; }
        public string? Faculty { get; set; }
        public int? CourseNum { get; set; }
        public IFormFile? ProfileAvatar { get; set; }
    }
}
