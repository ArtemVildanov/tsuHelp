using tsuHelp.ViewModels;

namespace tsuHelp.Repository
{
    public class Subjects
    {
        public static List<SubjectViewModel> GetSubjects()
        {
            return new List<SubjectViewModel>
            {
                new SubjectViewModel
                {
                    Id = 1,
                    Subject = "Базы данных",
                    IsChecked = false
                },

                new SubjectViewModel
                {
                    Id = 2,
                    Subject = "Структурное проектирование",
                    IsChecked = false
                },

                new SubjectViewModel
                {
                    Id = 3,
                    Subject = "Теория вероятностей",
                    IsChecked = false
                },

                new SubjectViewModel
                {
                    Id = 4,
                    Subject = "Математический анализ",
                    IsChecked = false
                },

                new SubjectViewModel
                {
                    Id = 5,
                    Subject = "Алгоритмы и структуры данных",
                    IsChecked = false
                },

                new SubjectViewModel
                {
                    Id = 6,
                    Subject = "Основы программирования",
                    IsChecked = false
                },
            };
        }
    }
}
