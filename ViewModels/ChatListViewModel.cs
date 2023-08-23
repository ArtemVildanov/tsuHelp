using tsuHelp.Models;

namespace tsuHelp.ViewModels
{
    public class ChatListViewModel
    {
        public List<Chat> chats { get; set; }
		public string currentUserId { get; set; }
	}
}
