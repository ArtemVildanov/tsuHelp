using tsuHelp.Models;

namespace tsuHelp.Interfaces
{
    public interface IUserRepository
    {
        List<User> GetAllUsers();
        User GetUserById(string id);
        User GetCurrentUser();//получить объект User текущего авторизированного пользователя
        string GetCurrentUserId();//получить айди текущего авторизированного пользователя
        string GetUserIdByEmail(string email);
        User GetUserByEmail(string email);
        bool Add(User user);
        bool Update(User user);
        bool Delete(User user);
        bool Save();
    }
}
