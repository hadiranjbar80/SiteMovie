using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SiteMovie.Domain.ViewModels;
using SiteMovie.Repository;
using Microsoft.AspNetCore.Identity;
using SiteMovie.Domain.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SiteMovie.Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }


        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            if (_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            if (ModelState.IsValid)
            {
                if (_signInManager.IsSignedIn(User))
                    return RedirectToAction("Index", "Home");

                var newUser = new ApplicationUser()
                {
                    UserName = register.UserName,
                    Email = register.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(newUser, register.Password);

                if (result.Succeeded)
                {
                    // Set subscription
                    if (register.selectedOption == "tree-months")
                    {
                        var getDate = DateTime.Now;
                        Subscription subscription = new Subscription
                        {
                            ApplicationUserId = newUser.Id,
                            StartDate = getDate,
                            EndDate = getDate.AddDays(90),
                            Title = "ویژه سه ماه",
                            IsFinally = false
                        };
                        _context.Add(subscription);
                        await _context.SaveChangesAsync();
                    }
                    else if (register.selectedOption == "one-month")
                    {
                        var getDate = DateTime.Now;
                        Subscription subscription = new Subscription
                        {
                            ApplicationUserId = newUser.Id,
                            StartDate = getDate,
                            EndDate = getDate.AddDays(30),
                            Title = "معمولی یک ماه",
                            IsFinally = false
                        };
                        _context.Add(subscription);
                        await _context.SaveChangesAsync();
                    }

                    // Set Role
                    await _userManager.AddToRoleAsync(newUser, "User");
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(register);
        }

        [HttpGet]
        [Route("Login")]
        public IActionResult Login(string returnUrl = null)
        {
            if (_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");

            ViewData["returnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel login, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                if (_signInManager.IsSignedIn(User))
                    return RedirectToAction("Index", "Home");

                ViewData["returnUrl"] = returnUrl;

                var result = await _signInManager.PasswordSignInAsync(login.UserName, login.Password, login.RememberMe, true);

                var user = await _userManager.FindByNameAsync(login.UserName);
                if (result.Succeeded)
                {
                    if (returnUrl != null && Url.IsLocalUrl(returnUrl))
                    {
                        var claim = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
                            ,new Claim(ClaimTypes.Name,user.UserName)
                        };
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");

                }

                if (result.IsLockedOut)
                {
                    ViewData["Message"] = "اکانت شما به دلیل پنج بار ورود نا موفق به مدت پنج دقیقه قفل شده است.";
                }

                ModelState.AddModelError("", "نام کاربری یا کلمه عبور اشتباه است.");
            }

            return View(login);
        }

        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
