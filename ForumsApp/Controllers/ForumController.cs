using System.Linq;
using Forums.Data.Interface;
using Forums.Data.Models;
using Forums.Data.ViewModel.Forum;
using Forums.Data.ViewModel.Post;
using Forums.Service;
using Microsoft.AspNetCore.Mvc;

namespace ForumsApp.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForum _forumService;
        private readonly IPost _postService;

        public ForumController(IForum forumService)
        {
            _forumService = forumService;
          //  _postService = postService;
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

        public IActionResult Topic(int id)
        {
            var forum = _forumService.GetById(id);
            var posts = forum.Posts;

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
