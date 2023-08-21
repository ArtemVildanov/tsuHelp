using tsuHelp.Models;

namespace tsuHelp.Interfaces
{
    public interface IUserSubjectsRepository
    {
        bool Add(UserSubjects userSubjects);

        List<UserSubjects> GetSubjectsByUserId(string userId);

        public void AddMultipleEntities(List<UserSubjects> subjects);

        bool Update(UserSubjects userSubjects);

        bool Delete(UserSubjects userSubjects);

        void DeleteMultipleEntities(List<UserSubjects> userSubjects);

        bool Save();
    }
}
