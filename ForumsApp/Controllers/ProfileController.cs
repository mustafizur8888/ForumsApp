using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Forums.Data.Interface;
using Forums.Data.Models;
using Forums.Data.ViewModel.ApplicationUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ForumsApp.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationUser _userService;
        private readonly IUpload _uploadService;

        public ProfileController(UserManager<ApplicationUser> userManager,
            IApplicationUser userService,
            IUpload uploadService)
        {
            _userManager = userManager;
            _userService = userService;
            _uploadService = uploadService;
        }

        public IActionResult Detail(string id)
        {
            var user = _userService.GetById(id);
            var userRoles = _userManager.GetRolesAsync(user).Result;
            var model = new ProfileModel()
            {
                UserId = user.Id,
                Username = user.UserName,
                UserRating = user.Rating.ToString(),
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
                MemberSince = user.MemberSince,
                IsActive = user.IsActive,
                IsAdmin = userRoles.Contains("Admin")
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {
            var userId = _userManager.GetUserId(User);

            string imageName = userId + Path.GetExtension(file.FileName);
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/users", imageName);

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            savePath = savePath.Substring(savePath.IndexOf("/images"),
                savePath.Length - (savePath.IndexOf("/images"))).Replace(@"\", @"/");
            await _userService.SetProfileImage(userId, savePath);

            return RedirectToAction("Detail", "Profile",
                new { id = userId });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var profiles = _userService
                .GetAll().OrderByDescending(user => user.Rating)
                .Select(u => new ProfileModel
                {
                    Email = u.Email,
                    ProfileImageUrl = u.ProfileImageUrl,
                    Username = u.UserName,
                    UserRating = u.Rating.ToString(),
                    MemberSince = u.MemberSince,
                    UserId = u.Id
                });
            var model = new ProfileListModel
            {
                Profiles = profiles
            };

            return View(model);
        }

    }
}
