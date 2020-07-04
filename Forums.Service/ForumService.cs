using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forums.Data;
using Forums.Data.Interface;
using Forums.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Forums.Service
{
    public class ForumService : IForum
    {
        private readonly ApplicationDbContext _context;
        public ForumService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Forum GetById(int id)
        {
            var forum = _context
                .Forums
                .Where(x => x.Id == id)
                .Include(f => f.Posts)
                    .ThenInclude(p => p.User)
                .Include(f => f.Posts)
                    .ThenInclude(p => p.Replies)
                        .ThenInclude(r => r.User)
                .FirstOrDefault();
            return forum;
        }

        public IEnumerable<Forum> GetAll()
        {
            return _context
                .Forums
                .Include(x => x.Posts);
        }

        public IEnumerable<ApplicationUser> GetAllActiveUsers(int id)
        {
            var posts = GetById(id).Posts;
            if (posts != null || !posts.Any())
            {
                var postUsers = posts.Select(p => p.User);
                var replyUsers = posts
                    .SelectMany(p => p.Replies)
                    .Select(r => r.User);

                var users = postUsers
                    .Union(replyUsers).Distinct();
                return users;
            }
            return new List<ApplicationUser>();
        }

        public async Task Create(Forum forum)
        {
            _context.Forums.Add(forum);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var forum = GetById(id);
            _context.Remove(forum);
            await _context.SaveChangesAsync();
        }

        public Task UpdateForumTitle(int forumId, string newTitle)
        {
            throw new NotImplementedException();
        }

        public Task UpdateForumDescription(int forumId, string newDescription)
        {
            throw new NotImplementedException();
        }

        public bool HasRecentPost(int forumId)
        {
            const int hoursAgo = 12;
            var window = DateTime.Now.AddHours(-hoursAgo);
            return GetById(forumId).Posts.Any(post => post.Created > window);
        }
    }
}
