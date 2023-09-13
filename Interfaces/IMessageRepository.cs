using tsuHelp.Models;

namespace tsuHelp.Interfaces
{
    public interface IMessageRepository
    {
        List<Message> GetAllMessagesByChatId(int chatId);

        List<Message> GetAllMessagesByUserId(string userId);

        Message GetLatestMessageByChatId(int chatId);

        bool Add(Message message);

        bool Update(Message message);

        bool Delete(Message message);

        void DeleteMultipleEntities(List<Message> messages);

        bool Save();
    }
}
