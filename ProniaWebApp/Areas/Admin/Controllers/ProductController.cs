using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProniaWebApp.Areas.Admin.ViewModels.ProductViewModel;
using ProniaWebApp.Contexts;
using ProniaWebApp.Helpers.Extensions;
using ProniaWebApp.Models;

namespace ProniaWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ProniaDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ProniaDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products.AsNoTracking().Include(p => p.Category).Where(r => !r.IsDeleted).ToListAsync();
            return View(products);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.AsNoTracking().ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel product)
        {

            ViewBag.Categories = await _context.Categories.AsNoTracking().ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!product.Image.CheckFileSize(3000))
            {
                ModelState.AddModelError("Image", "Sekl boyukdu balacasini yukle");
                return View();
            }
            if (!product.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "sekil gonderde ne pdfni yapisdirmisan");
                return View();
            }


            string fileName = $"{Guid.NewGuid()}-{product.Image.FileName}";
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "website-images", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await product.Image.CopyToAsync(stream);
            }


            //FileStream stream = new FileStream(path, FileMode.Create);
            //await product.Image.CopyToAsync(stream);
            //stream.Dispose();

            Product newProduct = new()
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                DiscountPercent = product.DiscountPercent,
                Rating = product.Rating,
                Image = fileName,
                CategoryId = product.CategoryId,
                CreatedDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,


            };
            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }


        public async Task<IActionResult> Update(int id)
        {

            var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            ProductUpdateViewModel productUpdateViewModel = new()
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                DiscountPercent = product.DiscountPercent,
                Rating = product.Rating,
                
                CategoryId = product.CategoryId,
            };

            ViewBag.Categories = await _context.Categories.AsNoTracking().ToListAsync();

            return View(productUpdateViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName(nameof(Update))]
        public async Task<IActionResult> Update(ProductUpdateViewModel productUpdateViewModel, int id)
        { 
            if (!productUpdateViewModel.Image.CheckFileSize(3000))
            {
                ModelState.AddModelError("Image", "get ariqla");
                return View();
            }

            if (!productUpdateViewModel.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "get ariqla");
                return View();
            }
            var product = await _context.Products.FirstOrDefaultAsync(r => r.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            if (productUpdateViewModel.Image != null)
            {
                string basePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "website-images");
                string path = Path.Combine(basePath, product.Image);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                string fileName = $"{Guid.NewGuid()}-{productUpdateViewModel.Image.FileName}";
                path = Path.Combine(basePath, fileName);
                using (FileStream stream = new(path, FileMode.Create))
                {
                    await productUpdateViewModel.Image.CopyToAsync(stream);
                }
                product.Image = fileName;
            }

         
    


            product.Name = productUpdateViewModel.Name;
            product.Description = productUpdateViewModel.Description;
            product.Price = productUpdateViewModel.Price; 
            product.DiscountPercent = productUpdateViewModel.DiscountPercent;
            product.Rating = productUpdateViewModel.Rating; 
            //product.Image = productUpdateViewModel.Image.FileName;
            product.CategoryId = productUpdateViewModel.CategoryId;
            product.UpdateDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(); 
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            
            var product = await _context.Products.FirstOrDefaultAsync(r => r.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "website-images", product.Image.ToString());
            if(System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);   
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName(nameof(Delete))]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(r => r.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            product.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}
