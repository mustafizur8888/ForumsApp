using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forums.Data.Models;

namespace Forums.Data.Interface
{
    public interface IApplicationUser
    {
        ApplicationUser GetById(string id);
        IEnumerable<ApplicationUser> GetAll();

        Task IncrementRating(string id);
        Task Add(ApplicationUser user);
        Task Deactivate(ApplicationUser user);
        Task SetProfileImage(string id, Uri uri);
        Task BumpRating(string userId, Type type);
    }
}
