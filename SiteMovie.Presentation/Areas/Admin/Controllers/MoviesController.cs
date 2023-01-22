using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SiteMovie.Domain.Models;
using SiteMovie.Domain.ViewModels;
using SiteMovie.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SiteMovie.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult ListMovies()
        {
            return PartialView();
        }

        public IActionResult Index(int pageId = 1)
        {
            var movies = _context.Movies.ToList();
            List<ListMoviesViewModel> listMovies = new List<ListMoviesViewModel>();
            foreach (var item in movies)
            {
                listMovies.Add(new ListMoviesViewModel
                {
                    MovieTitle = item.MovieTitle,
                    Actors = item.Actors,
                    Country = item.Country,
                    PublishDate = item.PublishDate,
                    Director = item.Director,
                    Id = item.Id,
                    MovieImage = item.MovieImage
                });
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

        #region InsertMovie

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.categories = _context.Categories.ToList();

            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(AddMovieViewModels movie)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Save image
                    string imageFileName = "";

                    if (movie.MovieImage?.Length > 0)
                    {
                        imageFileName = Guid.NewGuid().ToString().Substring(0, 22)
                            .Replace("-", " ") + Path.GetExtension(movie.MovieImage.FileName);

                        string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                            "wwwroot", "Images", "Movies", imageFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            movie.MovieImage.CopyTo(stream);
                        }
                    }

                    // Save demo 
                    string demoFileName = "";

                    if (movie.Demo?.Length > 0)
                    {
                        demoFileName = Guid.NewGuid().ToString().Substring(0, 22)
                            .Replace("-", " ") + Path.GetExtension(movie.Demo.FileName);

                        string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                           "wwwroot", "Images", "Movies", demoFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            movie.Demo.CopyTo(stream);
                        }
                    }

                    Movies newMovie = new Movies
                    {
                        MovieTitle = movie.MovieTitle,
                        Actors = movie.Actors,
                        Director = movie.Director,
                        Country = movie.Country,
                        PublishDate = movie.PublishDate,
                        Description = movie.Description,
                        CreateDate = DateTime.Now,
                        MovieImage = imageFileName,
                        Demo = demoFileName,
                        imdbRating = movie.imdbRating
                    };

                    _context.Add(newMovie);
                    _context.SaveChanges();


                    // Put groups and movies together 
                    if (movie.selectedGroups.Any() && movie.selectedGroups.Count > 0)
                    {
                        foreach (var groups in movie.selectedGroups)
                        {
                            _context.CategoryToMovies.Add(new CategoryToMovies()
                            {
                                CategoryId = groups,
                                MovieId = newMovie.Id
                            });
                        }
                        _context.SaveChanges();
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                return RedirectToAction("Index");
            }

            ViewBag.categories = _context.Categories.ToList();

            return View(movie);
        }

        #endregion

        #region EditMovie

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
                return NotFound();

            var getMovie = await _context.Movies.FindAsync(id);


            if (getMovie == null)
                return NotFound();

            EditMovieViewModels editMovie = new EditMovieViewModels
            {
                Actors = getMovie.Actors,
                Country = getMovie.Country,
                Description = getMovie.Description,
                Director = getMovie.Director,
                MovieTitle = getMovie.MovieTitle,
                PublishDate = getMovie.PublishDate,
                imdbRating = getMovie.imdbRating
            };

            ViewBag.categories = _context.Categories.ToList();

            ViewBag.movieGroups = _context.CategoryToMovies.
                Where(g => g.MovieId == id).ToList();

            return View(editMovie);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int id, EditMovieViewModels editMovie)
        {
            if (ModelState.IsValid)
            {
                var movie = await _context.Movies.FindAsync(id);

                // Save image
                string imageFileName = "";

                if (editMovie.MovieImage?.Length > 0)
                {
                    System.IO.File.Delete(Directory.GetCurrentDirectory() + "/wwwroot/Images/Movies/" + movie.MovieImage);

                    imageFileName = Guid.NewGuid().ToString().Substring(0, 22)
                        .Replace("-", " ") + Path.GetExtension(editMovie.MovieImage.FileName);

                    string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot", "Images", "Movies", imageFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        editMovie.MovieImage.CopyTo(stream);
                    }
                }

                // Save demo 
                string demoFileName = "";

                if (editMovie.Demo?.Length > 0)
                {
                    System.IO.File.Delete(Directory.GetCurrentDirectory() + "/wwwroot/Images/Movies/" + movie.Demo);

                    demoFileName = Guid.NewGuid().ToString().Substring(0, 22)
                        .Replace("-", " ") + Path.GetExtension(editMovie.Demo.FileName);

                    string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                       "wwwroot", "Images", "Movies", demoFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        editMovie.Demo.CopyTo(stream);
                    }
                }

                movie.Actors = editMovie.Actors;
                movie.Country = editMovie.Country;
                movie.Description = editMovie.Description;
                movie.Director = editMovie.Director;
                movie.MovieTitle = editMovie.MovieTitle;
                movie.PublishDate = editMovie.PublishDate;
                movie.imdbRating = editMovie.imdbRating;
                movie.Demo = ((demoFileName != "") ? demoFileName : movie.Demo);
                movie.MovieImage = ((imageFileName != "") ? imageFileName : movie.MovieImage);

                _context.Update(movie);
                await _context.SaveChangesAsync();

                // Delete all groups from the current movie and set new seleted groups.
                _context.CategoryToMovies.Where(g => g.MovieId == editMovie.Id).ToList().ForEach(g => _context.CategoryToMovies.Remove(g));
                if (editMovie.selectedGroups.Any() && editMovie.selectedGroups.Count > 0)
                {
                    foreach (var groups in editMovie.selectedGroups)
                    {
                        _context.CategoryToMovies.Add(new CategoryToMovies()
                        {
                            CategoryId = groups,
                            MovieId = editMovie.Id
                        });
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        #endregion

        #region DeleteMovie

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
                return NotFound();

            var getMovie = await _context.Movies.FindAsync(id);

            if (getMovie == null)
                return NotFound();

            return PartialView(getMovie);
        }

        [HttpPost, ActionName("Delete")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            if (id == null)
                return NotFound();

            var getMovie = await _context.Movies.FindAsync(id);

            if (getMovie == null)
                return NotFound();

            _context.Remove(getMovie);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        #endregion

        public IActionResult LinkToMovies(int id)
        {
            ViewBag.movieId = id;
            return View(_context.MovieLinks.Where(l => l.MovieId == 0));
        }

        [HttpPost, ActionName("LinkToMovies")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> LinkToMoviesConfirm(int movieId, List<int> selectedLinks)
        {
            if (selectedLinks != null)
            {
                foreach (var item in selectedLinks)
                {
                    var getLink = await _context.MovieLinks.FindAsync(item);
                    getLink.MovieId = movieId;
                    _context.Update(getLink);
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteLink(int id)
        {
            var movieLinks = _context.MovieLinks.Where(l => l.MovieId == id).ToList();
            return View(movieLinks);
        }

        [HttpPost, ActionName("DeleteLink")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteLinkConfirm(List<int> selectedLinks)
        {
            if (selectedLinks != null)
            {
                foreach (var item in selectedLinks)
                {
                    var link = await _context.MovieLinks.FindAsync(item);
                    link.MovieId = 0;
                    _context.Update(link);
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

    }
}
