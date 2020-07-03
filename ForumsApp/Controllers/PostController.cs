using System.Collections.Generic;
using System.Linq;
using Forums.Data.Interface;
using Forums.Data.Models;
using Forums.Data.ViewModel.Post;
using Forums.Data.ViewModel.Reply;
using Microsoft.AspNetCore.Mvc;

namespace ForumsApp.Controllers
{
    public class PostController : Controller
    {
        private readonly IPost _postService;

        public PostController(IPost postService)
        {
            _postService = postService;
        }
        public IActionResult Index(int id)
        {
            var post = _postService.GetById(id);

            var replies = GetPostReplies(post).OrderBy(reply => reply.Date);

            var model = new PostIndexModel
            {
                Id = post.Id,
                Title = post.Title,
                AuthorId = post.User.Id,
                AuthorName = post.User.UserName,
                AuthorImageUrl = post.User.ProfileImageUrl,
                AuthorRating = post.User.Rating,
                IsAuthorAdmin = IsAuthorAdmin(post.User),
                Date = post.Created,
                PostContent = _postFormatter.Prettify(post.Content),
                Replies = replies,
                ForumId = post.Forum.Id,
                ForumName = post.Forum.Title
            };

            return View();
        }

         private IEnumerable<PostReplyModel> GetPostReplies(Post post)
        {
            return post.Replies.Select(reply => new PostReplyModel
            {
                Id = reply.Id,
                AuthorName = reply.User.UserName,
                AuthorId = reply.User.Id,
                AuthorImageUrl = reply.User.ProfileImageUrl,
                AuthorRating = reply.User.Rating,
                Date = reply.Created,
                ReplyContent = _postFormatter.Prettify(reply.Content),
                IsAuthorAdmin = IsAuthorAdmin(reply.User)
            });
        }
    }
}
