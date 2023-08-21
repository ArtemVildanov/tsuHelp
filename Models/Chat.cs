using System.ComponentModel.DataAnnotations.Schema;

namespace tsuHelp.Models
{
    public class Chat
    {
        public int Id { get; set; }

        public string FirstUserId { get; set; }

        public string SecondUserId { get; set; }

        [NotMapped]
        public User? FirstUser { get; set; }

        [NotMapped]
        public User? SecondUser { get; set; }


    }
}
