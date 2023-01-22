using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteMovie.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteMovie.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() 
        { 
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ActivateCommentByAdmin(int id)
        {
            var comment = await _context.MovieComments.FindAsync(id);
            comment.IsActive = true;
            _context.Update(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteCommentByAdmin(int id)
        {
            var comment = await _context.MovieComments.FindAsync(id);
            if (comment == null)
                return NotFound();
            _context.Remove(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ActiveAllComments()
        {
            var comments = _context.MovieComments.Where(c => c.IsActive == false).ToList();
            foreach (var comment in comments)
            {
                comment.IsActive = true;
                _context.Update(comment);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteAllComments()
        {
            var comments = _context.MovieComments.Where(c => c.IsActive == false).ToList();
            foreach (var comment in comments)
            {
                _context.Remove(comment);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
