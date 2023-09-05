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
            var product = _context.product.ToList();
            return View(product);
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
                return RedirectToAction("Product");

            }
            return View();
        }

        [Area("Admin")]
        [Route("/Admin/Product/Edit")]
        public  IActionResult Edit(int id)
        {
            var category = _context.category.ToList();
            ViewData["category"] = category;
            var productsVM =  _context.product.Find(id);
            return View(productsVM);
            
        }
        [HttpPost]
        [Area("Admin")]
        [Route("/Admin/Product/Edit")]
        public async Task<IActionResult> Edit(ProductsVM productVM)
        {
          
            var file = productVM.file;
            if (file != null)
            {
                var p = productVM;
                using (var target = new MemoryStream())
                {
                    file.CopyTo(target);
                    p.dataimage = target.ToArray();
                }
                _context.product.Update(p);
                _context.SaveChanges();
                return RedirectToAction("Product");

            }
            else
            {
                _context.product.Update(productVM);
                _context.SaveChanges();
                return RedirectToAction("Product");
            }   
        }


        [Area("Admin")]
        [Route("/Admin/Product/Delete")]
        public IActionResult Delete()
        {
            return View();
        }
    }
}
