using System.Linq;
using Forums.Data.Interface;
using Forums.Data.Models;
using Forums.Data.ViewModel.Forum;
using Forums.Data.ViewModel.Post;
using Forums.Data.ViewModel.Search;
using Microsoft.AspNetCore.Mvc;

namespace ForumsApp.Controllers
{
    public class SearchController : Controller
    {
        private readonly IPost _postSearch;

        public SearchController(IPost postSearch)
        {
            _postSearch = postSearch;
        }

        public IActionResult Results(string searchQuery)
        {
            var posts = _postSearch.GetFilteredPosts(searchQuery).ToList();
            var areNoResults = (!string.IsNullOrEmpty(searchQuery) && posts.Any());

            var postListings = posts
                .Select(p => new PostListingModel
                {
                    Id = p.Id,
                    AuthorId = p.User.Id,
                    AuthorName = p.User.UserName,
                    AuthorRating = p.User.Rating,
                    Title = p.Title,
                    DatePosted = p.Created.ToString(),
                    RepliesCount = p.Replies.Count(),
                    Forum = BuildForumListing(p)
                });

            var model = new SearchResultModel
            {
                Posts = postListings,
                SearchQuery = searchQuery,
                EmptySearchResults = areNoResults
            };

            return View(model);
        }
        
        [HttpPost]
        public IActionResult Search(string searchQuery)
        {
            return RedirectToAction("Results", new { searchQuery });
        }
        private ForumListingModel BuildForumListing(Post post)
        {
            var forum = post.Forum;
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
