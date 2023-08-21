using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Security.Claims;
using tsuHelp.Interfaces;
using tsuHelp.Models;
using tsuHelp.Repository;
using tsuHelp.ViewModels;

namespace tsuHelp.Controllers
{
    public class PostsController : Controller
    {

        private readonly IPostsRepository _postsRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITagsInPostRepository _tagsInPostRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostsController(IPostsRepository postsRepository, IUserRepository userRepository, ITagsInPostRepository tagsInPostRepository, IHttpContextAccessor httpContextAccessor)
        {
            _postsRepository = postsRepository;
            _userRepository = userRepository;
            _tagsInPostRepository = tagsInPostRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            string currentUserId = _userRepository.GetCurrentUserId();
            IEnumerable<Post> posts = await _postsRepository.GetAll();
            List<PostViewModel> postsViewModel = new List<PostViewModel>();
            foreach(var post in posts)
            {
                var userID = post.UserId;
                var user = _userRepository.GetUserById(userID);
                var tags = _tagsInPostRepository.GetTagsByPostId(post.Id);

                var newPost = new PostViewModel
                {
                    Id = post.Id,
                    Title = post.Title,
                    Description = post.Description,
                    UserId = userID,
                    CurrentUserId = currentUserId,
                    User = user,
                    Tags = tags,
                };

                postsViewModel.Add(newPost);
            }

            
            return View(postsViewModel);
        }

        public IActionResult Create() 
        {
            var currentUser = _userRepository.GetCurrentUser();
            var currentUserId = currentUser.Id;
            
            var newPost = new CreatePostViewModel
            {
                UserId = currentUserId,
                User =  currentUser,
                Tags = Repository.Subjects.GetSubjects()
            };
            return View(newPost);
        }

        [HttpPost]
        public IActionResult Create (CreatePostViewModel createPostViewModel)
        {
            if (ModelState.IsValid)
            {
                var post = new Post
                {
                    Title = createPostViewModel.Title,
                    Description = createPostViewModel.Description,
                    UserId = createPostViewModel.UserId,
                    User = createPostViewModel.User,
                };

                _postsRepository.Add(post);

                var selectedSubjects = createPostViewModel.Tags.Where(x => x.IsChecked).ToList();//список выбранных предметов

                List<TagsInPost> tagsInPosts = new List<TagsInPost>();
                foreach (var subject in selectedSubjects)
                {
                    var tagInPost = new TagsInPost
                    {
                        PostId = post.Id,
                        Tag = subject.Subject
                    };
                    tagsInPosts.Add(tagInPost);
                }
                _tagsInPostRepository.AddMultipleEntities(tagsInPosts);
                

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Post creating error");
            }

            return View(createPostViewModel);
        }

        public IActionResult EditPost(int id)
        {
            var post = _postsRepository.GetPostById(id);
            var tagsList = _tagsInPostRepository.GetTagsByPostId(id);
            List<SubjectViewModel> subjectsViewModel = Subjects.GetSubjects();
            foreach (var tagsFromList in tagsList)
            {
                foreach (var subject in subjectsViewModel)
                {
                    if (tagsFromList.Tag == subject.Subject)
                    {
                        subject.IsChecked = true;
                    }
                }
            }

            var editPostViewModel = new EditPostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Description = post.Description,
                UserId= post.UserId,
                User = _userRepository.GetUserById(post.UserId),
                Tags = subjectsViewModel
            };
            return View(editPostViewModel);
        }

        [HttpPost]
        public IActionResult EditPost(EditPostViewModel editPostViewModel) 
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View(editPostViewModel);
            }

            var post = _postsRepository.GetPostById(editPostViewModel.Id);
            var currentTags = _tagsInPostRepository.GetTagsByPostId(post.Id);
            var selectedTags = editPostViewModel.Tags.Where(x => x.IsChecked).ToList();//список выбранных предметов

            _tagsInPostRepository.DeleteMultipleEntities(currentTags);//удалить старые предметы

            /// <summary> добавить новые предметы </summary>
            List<TagsInPost> postTags = new List<TagsInPost>();
            foreach (var tag in selectedTags)
            {
                var postTag = new TagsInPost
                {
                    PostId = post.Id,
                    Tag = tag.Subject
                };
                postTags.Add(postTag);
            }
            _tagsInPostRepository.AddMultipleEntities(postTags);

            /// <summary> изменить остальные поля </summary>
            post.Id = editPostViewModel.Id;
            post.Title = editPostViewModel.Title;
            post.Description = editPostViewModel.Description;
            post.UserId= editPostViewModel.UserId;

            _postsRepository.Update(post);

            return RedirectToAction("Index");

        }

        public IActionResult Delete (int id)
        {
            var deletePost = _postsRepository.GetPostById(id);
            if (deletePost == null) { return View("Error"); }

            var tagsInPost = _tagsInPostRepository.GetTagsByPostId(deletePost.Id);
            deletePost.Tags = tagsInPost;

            return View(deletePost);
        }

        [HttpPost]
        public IActionResult DeletePost(int id)
        {
            var deletePost = _postsRepository.GetPostById(id);
            if (deletePost == null) { return View("Error"); }

            var deleteTags = _tagsInPostRepository.GetTagsByPostId(id);
            _tagsInPostRepository.DeleteMultipleEntities(deleteTags);
            _postsRepository.Delete(deletePost);

            return RedirectToAction("Index", "Posts");
        }
    }
}
