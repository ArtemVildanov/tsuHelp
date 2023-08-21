using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using tsuHelp.Data;
using tsuHelp.Interfaces;
using tsuHelp.Models;
using tsuHelp.Repository;
using tsuHelp.ViewModels;

namespace tsuHelp.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserSubjectsRepository _userSubjectsRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPostsRepository _postsRepository;
        private readonly ITagsInPostRepository _tagsInPostRepository;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, 
            IUserSubjectsRepository userSubjectsRepository, IUserRepository userRepository, 
            IPostsRepository postsRepository, ITagsInPostRepository tagsInPostRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userSubjectsRepository = userSubjectsRepository;
            _userRepository = userRepository;
            _postsRepository = postsRepository;
            _tagsInPostRepository = tagsInPostRepository;
        }

        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            response.Subjects = Repository.Subjects.GetSubjects();

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid) return View(registerViewModel);

            /// <summary> проверка на уникальность почты </summary> 

            var user = await _userManager.FindByEmailAsync(registerViewModel.Email);
            if (user != null)
            {
                TempData["Error"] = "This email is already in use";
                return View(registerViewModel);
            }

            /// <summary> создание пользователя </summary> 

            var newUser = new User()
            {
                Name = registerViewModel.Name,
                Surname = registerViewModel.Surname,
                Email = registerViewModel.Email,
                UserName = registerViewModel.Email,
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);

            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            }

            /// <summary> работа с чекбоксами </summary> 

            var selectedSubjects = registerViewModel.Subjects.Where(x => x.IsChecked).ToList();//список выбранных предметов
            string userId = _userRepository.GetUserIdByEmail(registerViewModel.Email);//найти айди пользователя

            List<UserSubjects> userSubjects = new List<UserSubjects>();
            foreach (var subject in selectedSubjects)
            {
                var userSubject = new UserSubjects
                {
                    UserId = userId,
                    Subject = subject.Subject
                };
                userSubjects.Add(userSubject);
            }

            _userSubjectsRepository.AddMultipleEntities(userSubjects);

            

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);

            //var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
            var user = _userRepository.GetUserByEmail(loginViewModel.Email);

            if (user != null)
            {
                //user is found, check password
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordCheck)
                {
                    //password is correct, sign in
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                //password is incorrect
                TempData["Error"] = "Wrong password. Try again";
                return View(loginViewModel);
            }
            //user is not found
            TempData["Error"] = "Такого пользователя не существует";
            return View(loginViewModel);
        }//Password12345_

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Delete(string id)
        {
            var currentUserId = _userRepository.GetCurrentUserId();
            if (currentUserId == id)
                return RedirectToAction("Index", "Home");

            if (User.IsInRole("admin")) 
            {
                var deleteUser = _userRepository.GetUserById(id);
                return View(deleteUser);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult DeleteAccount(string id)
        {
            /// <summary>
            /// Определение объектов, которые надо удалить
            /// </summary>

            var deleteUser = _userRepository.GetUserById(id);//пользователь, которого надо удалить
            var deleteUserSubjects = _userSubjectsRepository.GetSubjectsByUserId(id);//Выбранные пользователем предметы
            var deleteUserPosts = _postsRepository.GetPostsByUserId(id);//Написанные пользователем посты
            
            foreach(var post in deleteUserPosts)
            {                                                                           /// 
                var deletePostTags = _tagsInPostRepository.GetTagsByPostId(post.Id);    /// Удаление тегов постов                                                                  
                _tagsInPostRepository.DeleteMultipleEntities(deletePostTags);           /// 
            }

            _postsRepository.DeleteMultipleEntities(deleteUserPosts);   //удаление постов (в постах уже нет тегов)

            _userSubjectsRepository.DeleteMultipleEntities(deleteUserSubjects); //удаление предметов пользователя

            _userRepository.Delete(deleteUser); // удаление пользователя

            return RedirectToAction("Index", "User");
        }
    }
}
