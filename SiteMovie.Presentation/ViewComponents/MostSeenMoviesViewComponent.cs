using Microsoft.AspNetCore.Mvc;
using SiteMovie.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteMovie.Presentation.ViewComponents
{
    public class MostSeenMoviesViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public MostSeenMoviesViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var mostVisitedMovies = _context.Movies.OrderByDescending(m => m.Visit).Take(5).ToList();
            return View("/Views/ViewComponents/MostSeenMovies.cshtml", mostVisitedMovies);
        }
    }
}
