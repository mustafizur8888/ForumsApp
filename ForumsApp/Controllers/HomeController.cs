﻿using System.Diagnostics;
using System.Linq;
using Forums.Data.Interface;
using Forums.Data.Models;
using Forums.Data.ViewModel.Forum;
using Forums.Data.ViewModel.Home;
using Forums.Data.ViewModel.Post;
using Microsoft.AspNetCore.Mvc;
using ForumsApp.Models;

namespace ForumsApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPost _postService;

        public HomeController(IPost postService)
        {
            _postService = postService;
        }

        public IActionResult Index()
        {
            var model = BuildHomeIndexModel();

            return View(model);
        }
        
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private HomeIndexModel BuildHomeIndexModel()
        {
            var latestPosts = _postService.GetLatestPosts(10);
            var posts = latestPosts
                .Select(post => new PostListingModel
                {
                    Id = post.Id,
                    Title = post.Title,
                    AuthorId = post.User.Id,
                    AuthorName = post.User.UserName,
                    AuthorRating = post.User.Rating,
                    DatePosted = post.Created.ToString(),
                    RepliesCount = post.Replies.Count(),
                    Forum = GetForumListingForPost(post),
                });

            return new HomeIndexModel
            {
                LatestPosts = posts,
                SearchQuery = ""
            };
        }

        private ForumListingModel GetForumListingForPost(Post post)
        {
            var forum = post.Forum;
            return new ForumListingModel
            {
                Id = forum.Id,
                Name = forum.Title,
                ImageUrl = forum.ImageUrl
            };
        }


    }
}
