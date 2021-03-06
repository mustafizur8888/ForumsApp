﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Forums.Data.Models;

namespace Forums.Data.Interface
{
    public interface IForum
    {
        Forum GetById(int id);
        IEnumerable<Forum> GetAll();
        IEnumerable<ApplicationUser> GetAllActiveUsers(int id);

        Task Create(Forum forum);
        Task Delete(int id);
        Task UpdateForumTitle(int forumId, string newTitle);
        Task UpdateForumDescription(int forumId, string newDescription);
        bool HasRecentPost(int forumId);
    }
}