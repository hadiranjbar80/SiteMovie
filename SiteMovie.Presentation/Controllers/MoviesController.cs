using Microsoft.AspNetCore.Mvc;
using SiteMovie.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SiteMovie.Domain.ViewModels;
using System.IO;
using System.Security.Claims;
using Microsoft.Extensions.FileProviders;
using SiteMovie.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace SiteMovie.Presentation.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public bool isCounted;
        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("ShowMovie/{id}")]
        public async Task<IActionResult> ShowMovie(int id, bool err = false)
        {

            ViewBag.err = err;

            var getMovie = await _context.Movies.FindAsync(id);

            if (getMovie == null)
                return NotFound();

            // Get Rate of each movie in a list 
            var getRating = _context.MovieComments.Where(c => c.MovieId == id)
                .Select(c => c.Rating).ToList();

            // Get the average rate to show 
            var averageRating = ((getRating.Count != 0 ? getRating.Average() : 0));

            ShowMovieViewModel showMovie = new ShowMovieViewModel
            {
                MovieTitle = getMovie.MovieTitle,
                Description = getMovie.Description,
                Demo = getMovie.Demo,
                MovieId = id,
                Actors = getMovie.Actors,
                Director = getMovie.Director,
                imdbRating = Math.Round(getMovie.imdbRating, 1),
                Rating = (decimal)averageRating
            };

            /* Finding groups that related to this movie */
            var categoryToMovies = _context.CategoryToMovies.Include(c => c.Category).Where(c => c.MovieId == id).ToList();

            var listCategory = new List<Category>();

            foreach (var item in categoryToMovies)
            {
                listCategory.Add(new Category
                {
                    GroupTitle = item.Category.GroupTitle
                });
            }

            ViewBag.Groups = listCategory;

            ViewBag.Links = _context.MovieLinks.Where(cm => cm.MovieId == id).ToList();

            // Get userId from cliam
            var userId = "";
            if (User.Identity.IsAuthenticated)
            {
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            }
            // Checking if user has subscription or not
            var subscription = _context.Subscriptions.Where(s => s.ApplicationUserId == userId).SingleOrDefault();
            var userDownloadCount = _context.UserDownloadCounts.Where(u => u.ApplicationUserId == userId).SingleOrDefault();
            if (subscription != null)
            {
                if (DateTime.Now <=
                subscription.EndDate &&
                subscription.IsFinally == true)
                {
                    ViewBag.HasSubscription = true;
                }
            }
            if (userDownloadCount == null || userDownloadCount.Count < 10) ViewBag.DownloadCount = true;

            // counting movie visit, each time that is seen
            VisitMovie(getMovie);

            return View(showMovie);

            // Method to count visit movie
            void VisitMovie(Movies movie)
            {
                movie.Visit += 1;
                _context.Update(movie);
                _context.SaveChanges();
            }

        }

        [HttpPost]
        public IActionResult DownloadMovie(int? id)
        {
            var link = _context.MovieLinks.FirstOrDefault(l => l.Id == id);

            if (link == null)
                return NotFound();

            if (link.Attachment == null)
                return View();
            else
            {

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
                var userDownloadCount = _context.UserDownloadCounts.Where(u => u.ApplicationUserId == userId).SingleOrDefault();
                var subscription = _context.Subscriptions.Where(s => s.ApplicationUserId == userId).SingleOrDefault();
                if ((userDownloadCount == null) || (userDownloadCount != null && userDownloadCount.Count <= 10) ||
                    (subscription != null && DateTime.Now <= subscription.EndDate))
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                          "wwwroot",
                          "Links");
                    string fileName = link.Attachment;
                    var fileExtention = Path.GetExtension(fileName);
                    string fileDownloadName = link.LinkTitle + fileExtention;
                    IFileProvider provider = new PhysicalFileProvider(filePath);
                    IFileInfo fileInfo = provider.GetFileInfo(fileName);
                    var readStream = fileInfo.CreateReadStream();
                    var mineType = "application/media";
                    return File(readStream, mineType, fileDownloadName);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        // Count user's download
        public async Task<IActionResult> DownloadCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            var userDownloadCount = _context.UserDownloadCounts.Where(u => u.ApplicationUserId == userId).SingleOrDefault();
            var subscription = _context.Subscriptions.Where(s => s.ApplicationUserId == userId).SingleOrDefault();
            if (_context.UserDownloadCounts.Any(c => c.ApplicationUserId == userId))
            {
                userDownloadCount.Count++;
                _context.Update(userDownloadCount);
                await _context.SaveChangesAsync();
            }
            else
            {
                UserDownloadCount dounloadCount = new UserDownloadCount
                {
                    Count = 1,
                    ApplicationUserId = userId
                };
                _context.Add(dounloadCount);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        public IActionResult SearchMovie(string q)
        {
            List<Movies> listMovies = new List<Movies>();
            listMovies.AddRange(_context.Movies.Where(m => m.MovieTitle.Contains(q) || m.Description.Contains(q)).ToList());

            ViewBag.search = q;
            return View(listMovies.Distinct());
        }

        [Route("Archive")]
        public IActionResult Archive(int pageId = 1, string title = null, List<int> selectedGroups = null)
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.movieTitle = title;
            ViewBag.selectedGroups = selectedGroups;
            ViewBag.pageId = pageId;

            List<ShowListMoviesViewModel> listMovies = new List<ShowListMoviesViewModel>();

            if (selectedGroups != null && selectedGroups.Any())
            {
                foreach (var group in selectedGroups)
                {
                    var moviesId = _context.CategoryToMovies.Where(c => c.CategoryId == group).Select(m => m.MovieId).ToList();
                    foreach (var item in moviesId)
                    {
                        listMovies.AddRange(_context.Movies.Select(m => new ShowListMoviesViewModel
                        {
                            Id = m.Id,
                            CreateDate = m.CreateDate,
                            MovieImage = m.MovieImage,
                            MovieTitle = m.MovieTitle
                        }).Where(c => c.Id == item));
                    }
                    listMovies = listMovies.Distinct().ToList();
                }
            }
            else
            {
                foreach (var item in _context.Movies)
                {
                    listMovies.Add(new ShowListMoviesViewModel
                    {
                        Id = item.Id,
                        CreateDate = item.CreateDate,
                        MovieImage = item.MovieImage,
                        MovieTitle = item.MovieTitle
                    });
                }
            }

            if (title != null)
            {
                listMovies = listMovies.Where(m => m.MovieTitle.ToLower().Contains(title.ToLower())).ToList();
            }

            // Get the average of rate to assign in filtering
            foreach (var item in listMovies)
            {
                var getRateComments = _context.MovieComments.Where(c => c.MovieId == item.Id).Select(c => c.Rating).ToList();
                var getAverage = getRateComments.Count != 0 ? getRateComments.Average() : 0;
                item.Rate = (decimal)getAverage;
            }

            const int pageSize = 5;
            if (pageId < 1)
                pageId = 1;

            var movieCount = listMovies.Count();
            var pager = new Pager(movieCount, pageId, pageSize);
            var skip = (pageId - 1) * pageSize;

            var data = listMovies.Skip(skip).Take(pager.PageSize).ToList();

            ViewBag.Pager = pager;

            return View(data);
        }
    }
}
