using Microsoft.AspNetCore.Mvc;
using ProniaWebApp.Contexts;
using ProniaWebApp.ViewModel;
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
            List<Models.Shipping> shippings = _context.Shippers.ToList();

            HomeViewModel homeViewModel = new HomeViewModel()
            {
                Sliders = slides,
                Shippings = shippings
            };
            return View(homeViewModel);
        }
    }
}
