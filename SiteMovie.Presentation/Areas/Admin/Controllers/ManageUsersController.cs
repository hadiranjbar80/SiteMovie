using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiteMovie.Domain.Models;
using SiteMovie.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteMovie.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ManageUsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManage;
        private readonly RoleManager<IdentityRole> _roleManage;

        public ManageUsersController(UserManager<ApplicationUser> userManage, RoleManager<IdentityRole> roleManage)
        {
            _userManage = userManage;
            _roleManage = roleManage;
        }

        public IActionResult Index()
        {
            var model = _userManage.Users
                 .Select(u => new UsersIndexViewModel()
                 {
                     UserId = u.Id,
                     UserName = u.UserName,
                     Email = u.Email
                 }).ToList();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddUserToRole(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var user = await _userManage.FindByIdAsync(id);
            if (user == null) return NotFound();
            var roles = _roleManage.Roles.ToList();
            var model = new AddUserToRoleViewModel() { UserId = id };

            foreach (var role in roles)
            {
                if (!await _userManage.IsInRoleAsync(user, role.Name))
                {
                    model.UserRoles.Add(new UserRolesViewModel()
                    {
                        RoleName = role.Name
                    });
                }
            }

            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddUserToRole(AddUserToRoleViewModel addUser)
        {
            if (addUser == null) return NotFound();
            var user = await _userManage.FindByIdAsync(addUser.UserId);
            if (user == null) return NotFound();
            var requestRoles = addUser.UserRoles.Where(r => r.IsSelected)
                .Select(u => u.RoleName)
                .ToList();
            var result = await _userManage.AddToRolesAsync(user, requestRoles);

            if (result.Succeeded) return RedirectToAction("Index");

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(addUser);
        }

        [HttpGet]
        public async Task<IActionResult> RemoveUserFromRole(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var user = await _userManage.FindByIdAsync(id);
            if (user == null) return NotFound();

            var userRoles = await _userManage.GetRolesAsync(user);
            var validRoles = userRoles.Select(r => new UserRolesViewModel(r)).ToList();
            var model = new AddUserToRoleViewModel(id, validRoles);

            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> RemoveUserFromRole(AddUserToRoleViewModel model)
        {
            if (model == null) return NotFound();
            var user = await _userManage.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();
            var requestRoles = model.UserRoles.Where(r => r.IsSelected)
                .Select(u => u.RoleName)
                .ToList();
            var result = await _userManage.RemoveFromRolesAsync(user, requestRoles);

            if (result.Succeeded) return RedirectToAction("Index");

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var user = await _userManage.FindByIdAsync(id);
            if (user == null) return NotFound();
            await _userManage.DeleteAsync(user);

            return RedirectToAction("Index");
        }
    }
}
