using tsuHelp.Models;

namespace tsuHelp.Interfaces
{
    public interface IUserChatHubConnectionRepository
    {
        UserChatHubConnection GetConnectionByUserId (string userId);

        bool Add(UserChatHubConnection userConnection);

        bool Update(UserChatHubConnection userConnection);

        bool Delete(UserChatHubConnection userConnection);

        bool Save();
    }
}
