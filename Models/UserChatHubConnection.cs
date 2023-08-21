using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tsuHelp.Models
{
    public class UserChatHubConnection
    {
        public int Id { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public User? User { get; set; }

        public string ConntectionId { get; set; }
    }
}
