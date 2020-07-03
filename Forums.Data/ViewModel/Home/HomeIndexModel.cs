using System.Collections.Generic;
using Forums.Data.ViewModel.Post;

namespace Forums.Data.ViewModel.Home
{
    public class HomeIndexModel
    {
        public string SearchQuery { get; set; }
        public IEnumerable<PostListingModel> LatestPosts { get; set; }

    }
}
