﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forums.Data;
using Forums.Data.Interface;
using Forums.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Forums.Service
{
    public class PostService : IPost
    {
        private readonly ApplicationDbContext _context;

        public PostService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Post GetById(int id)
        {
            return _context.Posts
                .Where(x => x.Id == id)
                .Include(post => post.User)
                .Include(post => post.Replies)
                    .ThenInclude(reply => reply.User)
                .Include(post => post.Forum)
                .First();
        }

        public IEnumerable<Post> GetAll()
        {
            return _context.Posts
                .Include(post => post.User)
                .Include(post => post.Replies)
                .ThenInclude(reply => reply.User)
                .Include(post => post.Forum);
        }

        public IEnumerable<Post> GetFilteredPosts(Forum forum, string searchQuery)
        {
            return string.IsNullOrWhiteSpace(searchQuery)
                ? forum.Posts
                : forum.Posts.Where(post
                    => post.Title.ToLower().Contains(searchQuery.ToLower())
                       || post.Content.ToLower().Contains(searchQuery.ToLower()));
        }
        public IEnumerable<Post> GetFilteredPosts(string searchQuery)
        {
            return GetAll().Where(post =>
                 post.Title.ToLower().Contains(searchQuery.ToLower())
                 || post.Content.ToLower().Contains(searchQuery.ToLower())
               );
        }

        public IEnumerable<Post> GetPostsByForum(int id)
        {
            return _context.Forums
                .First(forum => forum.Id == id)
                .Posts;
        }

        public IEnumerable<Post> GetLatestPosts(int n)
        {
            return GetAll().OrderByDescending(post => post.Created).Take(n);
        }

        public async Task Add(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }

        public Task Delete(Post post)
        {
            throw new NotImplementedException();
        }

        public Task EditPostContent(int id, string newContent)
        {
            throw new NotImplementedException();
        }

        public async Task AddReply(PostReply reply)
        {
            _context.PostReplies.Add(reply);
            await _context.SaveChangesAsync();
        }
    }
}
