using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiteMovie.Domain.Models;
using SiteMovie.Domain.ViewModels;
using SiteMovie.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ZarinpalSandbox;

namespace SiteMovie.Presentation.Areas.Users.Controllers
{
    [Area("Users")]
    [Authorize(Roles = "Admin,User")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> ShowProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            var user = await _userManager.FindByIdAsync(userId);
            var subscription = _context.Subscriptions.Where(s => s.ApplicationUserId == userId).SingleOrDefault();
            ViewBag.ShowPaymentSection = (subscription != null && subscription.IsFinally == false ? true : false);
            ShowUserProfileViewModel showUserProfile = new ShowUserProfileViewModel
            {
                Email = user.Email,
                ExpireDateSubscription = (subscription != null ? subscription.EndDate.ToShamsi() : ""),
                UserName = user.UserName,
                SubscriptionTitle = (subscription != null ? subscription.Title : ""),
                IsFinally = (subscription != null ? subscription.IsFinally : false),
                ImageName = user.ImageName
            };
            return View(showUserProfile);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ChangeSubscription(string selectedOption)
        {
            if (selectedOption != null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
                var getSubscription = _context.Subscriptions.Where(s => s.ApplicationUserId == userId).SingleOrDefault();
                if (getSubscription != null)
                {
                    if (getSubscription.EndDate == DateTime.Now || getSubscription.EndDate < DateTime.Now)
                    {
                        _context.Remove(getSubscription);
                        SetSubscription(userId);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        // Get rest of the time between EndDate and StartDate.
                        TimeSpan duration = DateTime.Parse(getSubscription.EndDate.ToString()).Subtract(DateTime.Parse(getSubscription.StartDate.ToString()));
                        _context.Remove(getSubscription);
                        SetSubscription(userId, duration);
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    SetSubscription(userId);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("ShowProfile");

            // Function for change user's subscription 
            void SetSubscription(string userId, dynamic restTime = null)
            {
                if (selectedOption == "tree-months")
                {
                    var getDate = DateTime.Now;
                    Subscription subscription = new Subscription
                    {
                        ApplicationUserId = userId,
                        StartDate = getDate,
                        EndDate = ((restTime != null) ? getDate.AddDays(90) + restTime : getDate.AddDays(90)),
                        Title = "ویژه سه ماه",
                        Price = 150000
                    };
                    _context.Add(subscription);
                }
                else if (selectedOption == "one-month")
                {
                    var getDate = DateTime.Now;
                    Subscription subscription = new Subscription
                    {
                        ApplicationUserId = userId,
                        StartDate = getDate,
                        EndDate = ((restTime != null) ? getDate.AddDays(30) + restTime : getDate.AddDays(30)),
                        Title = "معمولی یک ماه",
                        Price = 80000
                    };
                    _context.Add(subscription);
                }
            }
        }

        #region Payment gateway

        [Authorize]
        public IActionResult Payment()
        {
            return null;
        }

        [Authorize]
        public IActionResult OnlinePayment(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            var subscription = _context.Subscriptions.FirstOrDefault(s => s.ApplicationUserId == userId && s.IsFinally == false);
            subscription.IsFinally = true;
            _context.Update(subscription);
            _context.SaveChanges();
            return View();
        }

        #endregion

    }
}
