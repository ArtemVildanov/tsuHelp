using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using tsuHelp.Interfaces;
using tsuHelp.Models;
using tsuHelp.ViewModels;

namespace tsuHelp.Controllers
{
    public class UserController : Controller
    {
        private readonly IPostsRepository _postsRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITagsInPostRepository _tagsInPostRepository;
        private readonly IUserSubjectsRepository _userSubjectsRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IPostsRepository postsRepository, IUserRepository userRepository, ITagsInPostRepository tagsInPostRepository,
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
            var currentUserId = _userRepository.GetCurrentUserId();
            var users = _userRepository.GetAllUsers();
            var result = new List<User>();
            foreach(var user in users) 
            {
                //var currentUserId = _userRepository.GetCurrentUserId();
                //if (user.Id != currentUserId)//не показывать в списке пользователей текущего авторизированного пользователя
                //{
                //    user.Subjects = _userSubjectsRepository.GetSubjectsByUserId(user.Id);

                //    var userViewModel = new UserViewModel
                //    {
                //        Id = user.Id,
                //        Name = user.Name,
                //        Surname = user.Surname,
                //        Description = user.Description,
                //        Subjects = user.Subjects,
                //    };
                //    result.Add(userViewModel);
                //}

                
                if (user.Id != currentUserId)//не показывать в списке пользователей текущего авторизированного пользователя
                {

                    user.Subjects = _userSubjectsRepository.GetSubjectsByUserId(user.Id);

                    var userView = new User
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Surname = user.Surname,
                        Description = user.Description,
                        Subjects = user.Subjects,
                        ProfileAvatar = user.ProfileAvatar,
                    };
                    result.Add(userView);
                }
            }

            return View(result);
        }

        public IActionResult Detail(string id)
        {
            //var currentUserId = _userRepository.GetCurrentUserId();
            //if (id == currentUserId)
            //{
            //    return RedirectToAction("Index", "Dashboard");
            //}

            var user = _userRepository.GetUserById(id);
            var userSubjects = _userSubjectsRepository.GetSubjectsByUserId(id);
            var userPosts = _postsRepository.GetPostsByUserId(id);
            foreach(var post in userPosts)
            {
                post.Tags = _tagsInPostRepository.GetTagsByPostId(post.Id);
            }

            var detailUser = new User
            {
                Id = id,
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
            return View(detailUser);            
        } 
    }
}
