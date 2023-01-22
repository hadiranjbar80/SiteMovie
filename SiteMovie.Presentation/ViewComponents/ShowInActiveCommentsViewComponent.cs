using Microsoft.AspNetCore.Mvc;
using SiteMovie.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteMovie.Presentation.ViewComponents
{
    public class ShowInActiveCommentsViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public ShowInActiveCommentsViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var comments = _context.MovieComments.Where(c=>c.IsActive==false).ToList();
            return View("/Views/ViewComponents/ShowInActiveComments.cshtml", comments);
        }
    }
}
