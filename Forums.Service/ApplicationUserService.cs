﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forums.Data;
using Forums.Data.Interface;
using Forums.Data.Models;

namespace Forums.Service
{
    public class ApplicationUserService : IApplicationUser
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public ApplicationUser GetById(string id)
        {
            return GetAll().FirstOrDefault(user => user.Id == id);
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return _context.ApplicationUsers;
        }

        public Task IncrementRating(string id)
        {
            throw new NotImplementedException();
        }

        public async Task Add(ApplicationUser user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
        }

        public Task Deactivate(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public async Task SetProfileImage(string id, string path)
        {
            var user = GetById(id);
            user.ProfileImageUrl = path;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        public Task BumpRating(string userId, Type type)
        {
            throw new NotImplementedException();
        }
    }
}
