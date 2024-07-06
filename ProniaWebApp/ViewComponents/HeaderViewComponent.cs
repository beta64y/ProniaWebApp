using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using ProniaWebApp.Contexts;
using ProniaWebApp.Models;
using ProniaWebApp.ViewModel;

namespace ProniaWebApp.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly ProniaDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HeaderViewComponent(ProniaDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            HeaderViewModel headerViewModel= new();
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var basketItems = await _context.BasketItems.Include(r => r.Product.Category).Where(b => b.AppUserId == user.Id).ToListAsync();
                headerViewModel.BasketItems = basketItems;
                headerViewModel.TotalPrice = basketItems.Sum(b => b.Count * b.Product.Price);
            };
            var settings = await _context.Settings.ToDictionaryAsync(r => r.Key, r => r.Value);
            headerViewModel.Settings = settings;

            
            

            return View(headerViewModel);
        }
    }
}
