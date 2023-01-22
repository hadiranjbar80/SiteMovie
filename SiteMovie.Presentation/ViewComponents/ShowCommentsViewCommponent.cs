using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiteMovie.Domain.Models;
using SiteMovie.Domain.ViewModels;
using SiteMovie.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SiteMovie.Presentation.ViewComponents
{
    [ViewComponent(Name = "ShowComments")]
    public class ShowCommentsViewCommponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public ShowCommentsViewCommponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            List<ShowCommentsViewModel> showcomments = new List<ShowCommentsViewModel>();

            var comments = _context.MovieComments.Where(c => c.IsActive == true && c.MovieId == id).Select(c => new ShowCommentsViewModel()
            {
                Comment = c.Comment,
                CreateDate = c.CreateDate,
                Rating = c.Rating,
                UserName = c.ApplicationUserName
            }).ToList();

            return View("/Views/ViewComponents/ShowComments.cshtml", comments);
        }
    }
}
