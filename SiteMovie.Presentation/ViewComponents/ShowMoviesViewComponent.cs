using Microsoft.AspNetCore.Mvc;
using SiteMovie.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteMovie.Presentation.ViewComponents
{
    public class ShowMoviesViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ShowMoviesViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var movies = _context.Movies.ToList();
            return View("/Views/ViewComponents/ShowMovies.cshtml",movies);
        }
    }
}
