using Microsoft.AspNetCore.Mvc;
using System;
using tsuHelp.Interfaces;
using tsuHelp.Models;
using tsuHelp.Repository;
using tsuHelp.ViewModels;

namespace tsuHelp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IPostsRepository _postsRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITagsInPostRepository _tagsInPostRepository;
        private readonly IUserSubjectsRepository _userSubjectsRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardController(IPostsRepository postsRepository, IUserRepository userRepository, ITagsInPostRepository tagsInPostRepository,
            IUserSubjectsRepository userSubjectsRepository, IHttpContextAccessor httpContextAccessor)
        {
            _postsRepository = postsRepository;
            _userRepository = userRepository;
            _tagsInPostRepository = tagsInPostRepository;
            _userSubjectsRepository = userSubjectsRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            var id = _userRepository.GetCurrentUserId();
            var user = _userRepository.GetCurrentUser();

            var userSubjects = _userSubjectsRepository.GetSubjectsByUserId(id);
            var userPosts = _postsRepository.GetPostsByUserId(id);
            foreach (var post in userPosts)
            {
                post.Tags = _tagsInPostRepository.GetTagsByPostId(post.Id);
            }

            var UserView = new User
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Description = user.Description,
                Subjects = userSubjects,
                Posts = userPosts,
                University = user.University,
                Faculty = user.Faculty,
                CourseNum = user.CourseNum,
                ProfileAvatar = user.ProfileAvatar,
            };

            return View(UserView);
        }

        public IActionResult EditProfile()
        {
            var user = _userRepository.GetCurrentUser();
            var subjectsList = _userSubjectsRepository.GetSubjectsByUserId(user.Id);
            List<SubjectViewModel> subjectsViewModel = Subjects.GetSubjects();
            foreach (var subjectFromList in subjectsList)
            {
                foreach(var subject in subjectsViewModel)
                {
                    if(subjectFromList.Subject == subject.Subject)
                    {
                        subject.IsChecked = true;
                    }
                }
            }
            
            var editProfileViewModel = new EditProfileViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Description = user.Description,
                Subjects = subjectsViewModel,
                University = user.University,
                Faculty = user.Faculty,
                CourseNum = user.CourseNum,
            };
            return View(editProfileViewModel);
        }

        [HttpPost]
        public IActionResult EditProfile(EditProfileViewModel editProfileViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View(editProfileViewModel);
            }
            var user = _userRepository.GetCurrentUser();
            var currentSubjects = _userSubjectsRepository.GetSubjectsByUserId(user.Id);
            var selectedSubjects = editProfileViewModel.Subjects.Where(x => x.IsChecked).ToList();//список выбранных предметов

            _userSubjectsRepository.DeleteMultipleEntities(currentSubjects);//удалить старые предметы

            /// <summary> добавить новые предметы </summary>
            List<UserSubjects> userSubjects = new List<UserSubjects>();
            foreach (var subject in selectedSubjects)
            {
                var userSubject = new UserSubjects
                {
                    UserId = user.Id,
                    Subject = subject.Subject
                };
                userSubjects.Add(userSubject);
            }
            _userSubjectsRepository.AddMultipleEntities(userSubjects);

            /// <summary> изменить остальные поля </summary>
            user.Id = editProfileViewModel.Id;
            user.Email = editProfileViewModel.Email;
            user.Description = editProfileViewModel.Description;
            user.University= editProfileViewModel.University;
            user.Faculty = editProfileViewModel.Faculty;
            user.CourseNum = editProfileViewModel.CourseNum;

            ///<summary> загрузка фото </summary>
            if(editProfileViewModel.ProfileAvatar != null)
            {
                byte[] imageData = null;

                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(editProfileViewModel.ProfileAvatar.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)editProfileViewModel.ProfileAvatar.Length);
                }
                // установка массива байтов
                user.ProfileAvatar = imageData;//добавляем фото в нужном формате 
            }

            _userRepository.Update(user);

            return RedirectToAction("Index");
        }
    }
}
