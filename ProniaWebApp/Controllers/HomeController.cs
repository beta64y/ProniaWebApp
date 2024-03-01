using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Index()

        {
            List<Models.Slider> slides = await _context.Sliders.ToListAsync();
            List<Models.Shipping> shippings = await  _context.Shippers.ToListAsync();

            HomeViewModel homeViewModel = new HomeViewModel()
            {
                Sliders = slides,
                Shippings = shippings
            };
            return View(homeViewModel);
        }
    }
}
