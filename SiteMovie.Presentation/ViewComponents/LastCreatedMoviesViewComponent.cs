using Microsoft.AspNetCore.Mvc;
using SiteMovie.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteMovie.Presentation.ViewComponents
{
    public class LastCreatedMoviesViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public LastCreatedMoviesViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var lastMovies = _context.Movies.OrderByDescending(m => m.CreateDate).Take(5).ToList();
            return View("/Views/ViewComponents/LastCreatedMovies.cshtml", lastMovies);
        }
    }
}
