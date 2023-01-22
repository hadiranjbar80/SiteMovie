using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
    public class MovieLinksController : Controller
    {

        private readonly ApplicationDbContext _context;
        public MovieLinksController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var links = _context.MovieLinks;

            List<ListMovieLinksViewModel> listMovieLinks = new List<ListMovieLinksViewModel>();

            foreach (var item in links)
            {
                listMovieLinks.Add(new ListMovieLinksViewModel
                {
                    Id = item.Id,
                    Attachment = item.Attachment,
                    LinkTitle = item.LinkTitle
                });
            }

            return View(listMovieLinks);
        }

        #region AddLink

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [RequestFormLimits(MultipartBodyLengthLimit = 1073741824)]
        public async Task<IActionResult> Create(AddLinkViewModel addLink)
        {
            if (ModelState.IsValid)
            {
                string fileName = "";

                if (addLink.Attachment?.Length > 0)
                {
                    fileName = Guid.NewGuid().ToString().Substring(0, 22)
                        .Replace("-", " ") + Path.GetExtension(addLink.Attachment.FileName);

                    string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot", "Links", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        addLink.Attachment.CopyTo(stream);
                    }
                }

                MovieLinks link = new MovieLinks
                {
                    LinkTitle = addLink.LinkTitle,
                    Attachment = fileName
                };

                _context.MovieLinks.Add(link);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(addLink);
        }

        #endregion

        #region EditLink

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
                return NotFound();

            var link = await _context.MovieLinks.FindAsync(id);

            if (link == null)
                return NotFound();

            EditLinkViewModel editLink = new EditLinkViewModel
            {
                LinkTitle = link.LinkTitle,
                MovieId = link.MovieId

            };

            ViewBag.id = id;

            return View(editLink);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int id, EditLinkViewModel editLink)
        {
            if (id == null)
                return NotFound();

            var link = await _context.MovieLinks.FindAsync(id);

            if (link == null)
                return NotFound();

            string fileName = "";
            if (editLink.Attachment?.Length > 0)
            {
                fileName = Guid.NewGuid().ToString().Substring(0, 22)
                        .Replace("-", " ") + Path.GetExtension(editLink.Attachment.FileName);

                string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot", "Links", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    editLink.Attachment.CopyTo(stream);
                }
            }

            link.LinkTitle = editLink.LinkTitle;
            link.Attachment = ((fileName != null) ? fileName : link.Attachment);

            _context.MovieLinks.Update(link);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion

        #region DeleteLink

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var link = await _context.MovieLinks.FindAsync(id);
            return PartialView(link);
        }

        [HttpPost, ActionName("Delete")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var link = await _context.MovieLinks.FindAsync(id);

            if (link == null)
                return NotFound();

            _context.Remove(link);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion 
    }
}
