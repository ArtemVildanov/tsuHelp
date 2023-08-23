using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tsuHelp.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string? Description { get; set; }
        public List<UserSubjects>? Subjects { get; set; } 
        public List<Post>? Posts { get; set; }

        public List<Message>? Messages { get; set; }

        public string? University { get; set; }
        public string? Faculty { get; set; }
        public int? CourseNum { get; set; }
        public byte[]? ProfileAvatar { get; set; }

        public UserChatHubConnection? userConnection { get; set; }

    }
}
