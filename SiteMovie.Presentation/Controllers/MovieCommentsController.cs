using Microsoft.AspNetCore.Mvc;
using SiteMovie.Domain.ViewModels;
using SiteMovie.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SiteMovie.Domain.Models;
using System.Security.Claims;

namespace SiteMovie.Presentation.Controllers
{
    public class MovieCommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public MovieCommentsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateComment(CreateCommentViewModel createComment)
        {
            if (ModelState.IsValid)
            {
                var userName = User.FindFirstValue(ClaimTypes.Name).ToString();
                MovieComments comment = new MovieComments
                {
                    Comment = createComment.Comment,
                    CreateDate = DateTime.Now,
                    IsActive = false,
                    MovieId = createComment.MovieId,
                    Rating = createComment.Rating,
                    ApplicationUserName = userName
                };
                _context.Add(comment);
                await _context.SaveChangesAsync();

                return RedirectToAction("ShowMovie", "Movies", new { id = createComment.MovieId });
            }
            return RedirectToAction("ShowMovie", "Movies", new { id = createComment.MovieId, err = true });
        }
    }
}
