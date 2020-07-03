using System.Collections.Generic;
using Forums.Data.ViewModel.Post;

namespace Forums.Data.ViewModel.Forum
{
   public class ForumTopicModel
    {
        public ForumListingModel Forum { get; set; }
        public IEnumerable<PostListingModel> Posts { get; set; }

    }
}
