using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaWebApp.Contexts;
using ProniaWebApp.Models;

namespace ProniaWebApp.Controllers
{
    public class ShopController : Controller
    {
        private readonly ProniaDbContext _context;
        private readonly UserManager<AppUser> _userManager;

		public ShopController(ProniaDbContext context, UserManager<AppUser> userManager)
		{
			_context = context;
            _userManager = userManager;
          
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
        public async Task<IActionResult> ProductDetail (int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
            return PartialView("_ProductModalPartial",product);
        }
        [Authorize]
        public async Task<IActionResult> AddProductToBasket(int productId)
        {
            var product =await  _context.Products.FirstOrDefaultAsync( p => p.Id == productId && !p.IsDeleted); 
            if (product == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var  basketItem = await _context.BasketItems.FirstOrDefaultAsync(p => p.ProductId == productId && p.AppUserId == user.Id );

            if (basketItem == null)
            {
                BasketItem newBasketItem = new()
                {
                    ProductId = productId,
                    AppUserId = user.Id,
                    Count = 1,
                    CreatedDate = DateTime.UtcNow,

                };
                await _context.BasketItems.AddAsync(newBasketItem);
            }
            else
            {
                basketItem.Count++;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        

    }
}
