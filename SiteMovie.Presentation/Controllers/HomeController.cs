using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SiteMovie.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SiteMovie.Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowLastMovies()
        {
            List<Domain.Models.Movies> movies = (List<Domain.Models.Movies>)_context.Movies.OrderByDescending(c => c.CreateDate).Take(3);
            return PartialView(movies);
        }
    }
}
