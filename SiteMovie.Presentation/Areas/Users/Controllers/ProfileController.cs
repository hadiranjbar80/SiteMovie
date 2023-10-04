using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SiteMovie.Domain.Models;
using SiteMovie.Domain.ViewModels;
using SiteMovie.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SiteMovie.Presentation.Areas.Users.Controllers
{
    [Area("Users")]
    [Authorize]
    public class ProfileController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,IConfiguration configuration)
            :base(configuration)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            var user = await _userManager.FindByIdAsync(userId);
           // ViewBag.imageName = user.ImageName.ToString() ?? "";
            EditProfileViewModel editProfile = new()
            {
                Email = user.Email,
                UserId = userId,
                UserName = user.UserName
            };

            return View(editProfile);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProfileViewModel editProfile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Save image
                    string imageFileName = null;

                    if (editProfile.ImageName?.Length > 0)
                    {
                        imageFileName = Guid.NewGuid().ToString().Substring(0, 22)
                            .Replace("-", "") + Path.GetExtension(editProfile.ImageName.FileName);

                        string filePath = Path.Combine(Directory.GetCurrentDirectory()
                            , "wwwroot", "Images", "Profile", imageFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            editProfile.ImageName.CopyTo(stream);
                        }
                    }

                    var user = await _userManager.FindByIdAsync(editProfile.UserId);

                  //  user.ImageName = imageFileName;
                    user.UserName = editProfile.UserName;
                    user.Email = editProfile.Email;
                    // user.EmailConfirmed = false;

                    // Todo sending email verification

                    await _userManager.UpdateAsync(user);

                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                Notify("پروفایل با موفقیت ویرایش شد", "ویرایش پروفایل", NotificationType.success);
                return RedirectToAction("ShowProfile", "Home", "Users");
            }

            return View(editProfile);
        }
    }
}
