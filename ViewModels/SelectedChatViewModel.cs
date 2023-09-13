using tsuHelp.Models;

namespace tsuHelp.ViewModels
{
    public class SelectedChatViewModel
    {
        public int? ChatId { get; set; }
        public List<Message>? Messages { get; set; }
        public Message? LatestMessage { get; set; }
        public string SenderId { get; set; }
        public string RecieverId { get; set; }
        public User? Sender { get; set; }
        public User? Reciever { get; set; }
        public Post? Post { get; set; }
        public DateTime? LatestMessageDate { get; set; }
    }
}
