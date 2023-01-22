using Microsoft.AspNetCore.Mvc;
using SiteMovie.Domain.ViewModels;
using SiteMovie.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteMovie.Presentation.ViewComponents
{
    public class SuggestedMoviesViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public SuggestedMoviesViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var movies = _context.Movies.ToList();

            List<SuggestedMoviesViewModel> suggestedMovies = new List<SuggestedMoviesViewModel>();

            // Get the average rate to assign in filtering
            foreach (var item in movies)
            {
                var getRateComments = _context.MovieComments.Where(c => c.MovieId == item.Id).Select(c => c.Rating).ToList();
                var getAverage = getRateComments.Count != 0 ? getRateComments.Average() : 0;
                suggestedMovies.Add(new SuggestedMoviesViewModel
                {
                    MovieId = item.Id,
                    MovieImage = item.MovieImage,
                    MovieTitle = item.MovieTitle,
                    Rate = (decimal)getAverage
                });
            }

            suggestedMovies = suggestedMovies.OrderByDescending(m => m.Rate).Take(5).ToList();
            
            return View("/Views/ViewComponents/SuggestedMovies.cshtml", suggestedMovies);
        }

    }
}
