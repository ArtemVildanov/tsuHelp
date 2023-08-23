using tsuHelp.Models;

namespace tsuHelp.ViewModels
{
    public class ChatViewModel
    {
        public List<SelectedChatViewModel> Chats { get; set; }
        public SelectedChatViewModel SelectedChat { get; set; }
    }
}
