﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Forums.Data.Models;

namespace Forums.Data.Interface
{
    public interface IPost
    {
        Post GetById(int id);
        IEnumerable<Post> GetAll();
        IEnumerable<Post> GetFilteredPosts(string searchQuery);
        IEnumerable<Post> GetPostsByForum(int id);

        Task Add(Post post);
        Task Delete(Post post);
        Task EditPostContent(int id, string newContent);

       // Task AddReply(PostReply reply);

    }
}
