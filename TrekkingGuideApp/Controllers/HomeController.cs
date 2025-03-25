using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrekkingGuideApp.Data;
using TrekkingGuideApp.Models;

namespace TrekkingGuideApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            // Get all places from the database
            var places = _context.Places.ToList();
            return View(places);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public IActionResult NgApp()
        {
            return View();
        }

        public IActionResult Landing() 
        { 
            return View();
        }
    }
}
