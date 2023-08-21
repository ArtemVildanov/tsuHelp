using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using tsuHelp.Data;
using tsuHelp.Models;

namespace tsuHelp.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Необходимо ввести имя")]
        public string Name { get; set; }

        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Необходимо ввести фамилию")]
        public string Surname { get; set; }

        [Display(Name = "Email адрес")]
        [Required(ErrorMessage = "Необходимо ввести вашу почту")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Подтвердите пароль")]
        [Required(ErrorMessage = "Необходимо подтвердить пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        public string? Description { get; set; }
        public List<SubjectViewModel>? Subjects { get; set; } 
        public string? University { get; set; }
        public string? Faculty { get; set; }
        public int? CourseNum { get; set; }
        public IFormFile? ProfileAvatar { get; set; }
    }
}
