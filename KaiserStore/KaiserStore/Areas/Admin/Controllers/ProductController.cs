using KaiserStore.Data;
using KaiserStore.Models;
using Microsoft.AspNetCore.Mvc;


namespace KaiserStore.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Area("Admin")]
        [Route("/Admin/Product")]
        public IActionResult Product()
        {
            return View();
        }

        [Area("Admin")]
        [Route("/Admin/Product/Add")]
        public IActionResult Add()
        {
            var category = _context.category.ToList();
            ViewData["category"] = category;
            return View();
        }
       
        [HttpPost]
        [Area("Admin")]
        [Route("/Admin/Product/Add")]
        public IActionResult Add(ProductsVM product)
        {
            var category = _context.category.ToList();
            ViewData["category"] = category;
            var file = product.file;
            if (file != null)
            {
                var p = product;
                using (var target = new MemoryStream() )
                {
                    file.CopyTo(target);
                    p.dataimage = target.ToArray();
                }
                  _context.product.Add(p);
                 _context.SaveChanges();
                ViewData["Validate"] = "123456";
                return RedirectToAction("Product");

            }
            else {
                   ViewData["Validate"] = "Abcd";
            }
            return View();
        }

        [Area("Admin")]
        [Route("/Admin/Product/Edit")]
        public IActionResult Edit()
        {
            var product = _context.product;
            return View(product);
        }

        [Area("Admin")]
        [Route("/Admin/Product/Delete")]
        public IActionResult Delete()
        {
            return View();
        }
    }
}
