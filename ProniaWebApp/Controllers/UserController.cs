using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using ProniaWebApp.Helpers;
using ProniaWebApp.Helpers.Enums;
using ProniaWebApp.Models;
using ProniaWebApp.ViewModel;

namespace ProniaWebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;


        public UserController(UserManager<AppUser> userManager, IConfiguration configuration, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
        }

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
                IsActive = true,


            };
            IdentityResult identityResult = await _userManager.CreateAsync(appUser, registerViewModel.Password);
            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {

                    ModelState.AddModelError("", error.Description);
                }

                return View();
            }

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);

            string link = Url.Action("ConfirmEmail", "Auth", new { email = appUser.Email, token = token },
                HttpContext.Request.Scheme, HttpContext.Request.Host.Value);
            string body = $"<a href=\"{link}\">lalaland</a>";

            EmailHelper emailHelper = new(_configuration);
            await emailHelper.SendEmailAsync(new MailRequest { ToEmail=appUser.Email ,Subject="Confirm Email" ,Body = body});

            await _userManager.AddToRoleAsync(appUser, Roles.User.ToString());
            
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            TempData["Tab"] = "account-dashboard"; 
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            UserProfileViewmodel userProfileViewModel = new UserProfileViewmodel()
            {
                userUpdateViewModel = new UserUpdateViewModel()
                {
                    Fullname = user.Fullname,
                    UserName = user.UserName,
                    Email = user.Email
                }
            };
            return View(userProfileViewModel);
        }
        [Authorize]
        [HttpPost]

        public async Task<IActionResult> UpdateProfile(UserUpdateViewModel userUpdateViewModel)
        {
            TempData["Tab"] = "account-details";
            UserProfileViewmodel userProfileViewmodel = new()
            {
                userUpdateViewModel = userUpdateViewModel
            };
            if (!ModelState.IsValid)
            {
                return View(nameof(Profile), userProfileViewmodel);
            }
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }
            
            if(user.UserName != userUpdateViewModel.UserName   &&_userManager.Users.Any(u => u.UserName == userUpdateViewModel.UserName))
            {
                ModelState.AddModelError("UserName", "UserName Must be unique");
                return View(nameof(Profile), userProfileViewmodel);
            }
            if (user.Email != userUpdateViewModel.Email && _userManager.Users.Any(u => u.Email == userUpdateViewModel.Email))
            {
                ModelState.AddModelError("Email", "Email Must be unique");
                return View(nameof(Profile), userProfileViewmodel);
            }

            if (userUpdateViewModel.CurrentPassword != null)
            {
                if(userUpdateViewModel.NewPassword == null)
                {
                    ModelState.AddModelError("NewPassword", "new password bos ola bilmez");
                    return View(nameof(Profile), userProfileViewmodel);

                }
                IdentityResult identityResult = await _userManager.ChangePasswordAsync(user,userUpdateViewModel.CurrentPassword,userUpdateViewModel.NewPassword);
                if (!identityResult.Succeeded)
                {
                    foreach(var i in identityResult.Errors)
                    {
                        ModelState.AddModelError("",i.Description);
                    } 
                    return View(nameof(Profile), userProfileViewmodel);
                }
            }

            user.Fullname = userUpdateViewModel.Fullname;
            user.UserName = userUpdateViewModel.UserName;
            user.Email = userUpdateViewModel.Email;
            
           IdentityResult result =  await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var i in result.Errors)
                {
                    ModelState.AddModelError("",i.Description);
                }
                return View(nameof(Profile), userProfileViewmodel);

            }

            await _signInManager.RefreshSignInAsync(user);
            TempData["SuccessMessage"] = "Sizin profiliniz ugurla yenilendi";
            return View(nameof(Profile), userProfileViewmodel);


        }
    }
}
