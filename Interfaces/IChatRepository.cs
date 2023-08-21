using tsuHelp.Models;

namespace tsuHelp.Interfaces
{
    public interface IChatRepository
    {
        Chat GetChatById(int id);

        Chat GetChatByBothUsersId(string firstUserId, string secondUserId);

        List<Chat> GetAllChatsByUserId(string userId);

        bool Add(Chat chat);

        bool Update(Chat chat);

        bool Delete(Chat chat);

        bool Save();
    }
}
