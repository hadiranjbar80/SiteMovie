using Microsoft.AspNetCore.Mvc;
using SiteMovie.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteMovie.Domain.ViewComponents
{
    public class ShowLastMoviesViewComponents : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ShowLastMoviesViewComponents(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var movies = _context.Movies.OrderByDescending(c=>c.CreateDate).Take(3).ToList();
            return View("/Views/ViewComponents/ShowLastMovies.cshtml", movies);
        }
    }
}
