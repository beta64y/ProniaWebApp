using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaWebApp.Contexts;
using ProniaWebApp.Models;

namespace ProniaWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly ProniaDbContext _context;
        public SliderController(ProniaDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Models.Slider> sliders = await _context.Sliders.AsNoTracking().ToListAsync();

            return View(sliders);
        }
        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Create(Slider slider)
        {
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Detail(int id)
        {
            var slider = await _context.Sliders.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            return View(slider);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var slider = await _context.Sliders.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (slider == null)
            {
                return NotFound();
            };

            return View(slider);

        }
        [HttpPost]
        [ActionName(nameof(Delete))]
        public async Task<IActionResult> DeleteSlider(int id)
        {
            var slider = await _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (slider == null)
            {
                return NotFound();
            };
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Update(int id)
        {
            var slider = await _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (slider == null)
            {
                return NotFound();
            };

            return View(slider);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id ,Slider slider)
        {
            var dbSlider = await _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (dbSlider == null)
            {
                return NotFound();
            };

            dbSlider.Title = slider.Title;
            dbSlider.Description = slider.Description;
            dbSlider.Offer = slider.Offer;
            dbSlider.Image = slider.Image;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }

}
