using Microsoft.AspNetCore.Mvc;
using ProniaWebApp.Contexts;
using System;

namespace ProniaWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProniaDbContext _context;
        public HomeController(ProniaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()

        {
            List<Models.Slider> slides = _context.Sliders.ToList();
            return View(slides);
        }
    }
}
