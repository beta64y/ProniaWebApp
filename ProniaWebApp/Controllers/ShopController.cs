using Microsoft.AspNetCore.Mvc;

namespace ProniaWebApp.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
