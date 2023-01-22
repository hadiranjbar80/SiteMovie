using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiteMovie.Domain.Models;
using SiteMovie.Domain.ViewModels;
using SiteMovie.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteMovie.Presentation.ViewComponents
{
    public class CreateCommentViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
       
        public CreateCommentViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            return View("/Views/ViewComponents/CreateComment.cshtml", new CreateCommentViewModel() { MovieId = id });
        }


    }
}
