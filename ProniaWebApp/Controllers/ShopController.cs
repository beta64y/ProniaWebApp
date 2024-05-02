using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaWebApp.Contexts;

namespace ProniaWebApp.Controllers
{
    public class ShopController : Controller
    {
        private readonly ProniaDbContext _context;

		public ShopController(ProniaDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
        {
            int productCount =  await _context.Products.Where(p => !p.IsDeleted).CountAsync();
            ViewBag.ProductCount = productCount;

            return View();
        }
        public async Task<IActionResult> LoadMore(int skip)
        {
            int productCount = await _context.Products.Where(p => !p.IsDeleted).CountAsync();
            if (productCount <= skip)
            {
                return BadRequest();
            }
            var products =  await _context.Products.Where(p => !p.IsDeleted).Skip(skip).Take(8).ToListAsync();
            return View("_ProductPartial",products);
        }
    }
}
