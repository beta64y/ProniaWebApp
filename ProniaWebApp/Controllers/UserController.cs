using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProniaWebApp.Models;
using ProniaWebApp.ViewModel;

namespace ProniaWebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser appUser = new AppUser()
            {
                Fullname = registerViewModel.Fullname,
                UserName = registerViewModel.Username,
                Email = registerViewModel.Email,
                

            };
            await _userManager.CreateAsync(appUser,registerViewModel.Password);
            return RedirectToAction("Index", "Home");
        }
    }
}
