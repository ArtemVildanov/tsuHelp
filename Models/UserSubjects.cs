using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tsuHelp.Models
{
    public class UserSubjects
    {
        public int Id { get; set; }
        public string Subject { get; set; } = null!;

        [ForeignKey("User")]
        public string UserId { get; set; }
        public User? User { get; set; }


    }
}
