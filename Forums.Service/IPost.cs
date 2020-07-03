using System.Collections.Generic;
using System.Threading.Tasks;
using Forums.Data.Models;

namespace Forums.Service
{
   public interface IPost
   {
       Post GetById(int id);
       IEnumerable<Post> GetAll();
       IEnumerable<Post> GetFilteredPosts(string searchQuery);

       Task Add(Post post);
       Task Delete(Post post);
       Task EditPostContent(int id,string newContent);

       Task AddReply(PostReply reply);
   }
}
