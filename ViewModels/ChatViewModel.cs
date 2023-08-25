using tsuHelp.Models;

namespace tsuHelp.ViewModels
{
    public class ChatViewModel
    {
        public List<SelectedChatViewModel> Chats { get; set; }
        public string RecieverId { get; set; }
        public int PostId { get; set; }
    }
}
