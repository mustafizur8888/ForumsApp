using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Forums.Data.Interface;
using Forums.Data.Models;
using Forums.Data.ViewModel.Forum;
using Forums.Data.ViewModel.Post;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ForumsApp.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForum _forumService;
        private readonly IPost _postService;

        public ForumController(IForum forumService, IPost postService)
        {
            _forumService = forumService;
            _postService = postService;
        }

        public IActionResult Index()
        {
            var forums = _forumService
                 .GetAll()
                 .Select(forum => new ForumListingModel
                 {
                     Id = forum.Id,
                     Name = forum.Title,
                     Description = forum.Description
                 });

            var model = new ForumIndexModel
            {
                ForumList = forums
            };

            return View(model);
        }

        public IActionResult Topic(int id, string searchQuery)
        {
            var forum = _forumService.GetById(id);

            var posts = !string.IsNullOrWhiteSpace(searchQuery) ? _postService.GetFilteredPosts(forum, searchQuery).ToList() : forum.Posts.ToList();
            
            var postListings = posts.Select(post =>
                new PostListingModel
                {
                    Id = post.Id,
                    AuthorId = post.User.Id,
                    AuthorName = post.User.UserName,
                    AuthorRating = post.User.Rating,
                    Title = post.Title,
                    DatePosted = post.Created.ToString(),
                    RepliesCount = post.Replies.Count(),
                    Forum = BuildForumListing(post)
                });

            var model = new ForumTopicModel
            {
                Posts = postListings,
                Forum = BuildForumListing(forum)
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Search(int id, string searchQuery)
        {
            return RedirectToAction("Topic", new { id, searchQuery });
        }

        public IActionResult Create()
        {
            var model = new AddForumModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddForum(AddForumModel model)
        {
            var imageUri = "/images/forum/default.png";
            if (model.ImageUpload!=null)
            {
                string uploadImage = SaveUploadImage(model.ImageUpload);
                imageUri = uploadImage;
            }

            var forum = new Forum
            {
                Title = model.Title,
                Description = model.Description,
                Created = DateTime.Now,
                ImageUrl = imageUri
            };
            await _forumService.Create(forum);
            return RedirectToAction("Index", "Forum");

        }

        private string SaveUploadImage(IFormFile file)
        {
            string imageName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/users", imageName);

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            savePath = savePath.Substring(savePath.IndexOf("/images"),
                savePath.Length - (savePath.IndexOf("/images"))).Replace(@"\", @"/");

            return savePath;
        }


        private ForumListingModel BuildForumListing(Post post)
        {
            var forum = post.Forum;
            return BuildForumListing(forum);
        }

        private ForumListingModel BuildForumListing(Forum forum)
        {

            return new ForumListingModel
            {
                Id = forum.Id,
                Name = forum.Title,
                Description = forum.Description,
                ImageUrl = forum.ImageUrl
            };
        }
    }
}
