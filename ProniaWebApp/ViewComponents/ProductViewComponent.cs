using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaWebApp.Contexts;

namespace ProniaWebApp.ViewComponents
{
    public class ProductViewComponent : ViewComponent
    {
        private readonly ProniaDbContext _context;

        public ProductViewComponent(ProniaDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await _context.Products.Where(r => r.IsDeleted == false).Take(8).ToListAsync();
            return View(products);
        }
    }
}
