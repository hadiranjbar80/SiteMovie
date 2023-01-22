using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteMovie.Domain.Models;
using SiteMovie.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteMovie.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Categories.ToList());
        }

        #region Insert Category

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                Category ca = new Category()
                {
                    GroupTitle = category.GroupTitle
                };
                _context.Add(ca);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return PartialView(category);
        }

        #endregion 

        #region Edit Category

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
                return NotFound();

            var getCategory = await _context.Categories.FindAsync(id);

            if (getCategory == null)
                return NotFound();

            return PartialView(getCategory);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id == null)
                return NotFound();
            if (ModelState.IsValid)
            {
                var getCategory = await _context.Categories.FindAsync(id);

                if (getCategory == null)
                    return NotFound();

                getCategory.GroupTitle = category.GroupTitle;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return PartialView(category);
        }

        #endregion

        #region Delete Category

        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
                return NotFound();

            var getCategory = await _context.Categories.FindAsync(id);

            if (getCategory == null)
                return NotFound();

            return PartialView(getCategory);
        }

        [HttpPost, ActionName("Delete")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var getCategory= await _context.Categories.FindAsync(id);
            _context.Remove(getCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion
    }
}
