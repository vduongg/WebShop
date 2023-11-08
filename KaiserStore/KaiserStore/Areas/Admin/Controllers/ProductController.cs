using KaiserStore.Data;
using KaiserStore.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public IActionResult Product()
        {
            var product = _context.products.ToList();
            return View(product);
        }

        [Area("Admin")]
        [Route("/Admin/Product/Add")]
        [Authorize]
        public IActionResult Add()
        {
            var category = _context.categorys.ToList();
            ViewData["category"] = category;
            return View();
        }
       
        [HttpPost]
        [Area("Admin")]
        [Route("/Admin/Product/Add")]
        [Authorize]
        public IActionResult Add(ProductsVM product)
        {
            var category = _context.categorys.ToList();
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
                if(ModelState.IsValid)
                {
                    product.Id = 0;
                    _context.products.Add(p);
                    _context.SaveChanges();
                    return RedirectToAction("Product");
                }
                    
          
                  

            }
            return View();
        }

        [Area("Admin")]
        [Route("/Admin/Product/Edit/{id}")]
        [Authorize]
        public  IActionResult Edit(int id)
        {
            var category = _context.categorys.ToList();
            ViewData["category"] = category;
            var productsVM =  _context.products.Find(id);
            return View(productsVM);
            
        }
        [HttpPost]
        [Area("Admin")]
        [Route("/Admin/Product/Edit/{id}")]
        [Authorize]
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
                _context.products.Update(p);
                _context.SaveChanges();
                return RedirectToAction("Product");

            }
            else
            {
                _context.products.Update(productVM);
                _context.SaveChanges();
                return RedirectToAction("Product");
            }   
        }


        [Area("Admin")]
        [Route("/Admin/Product/Delete/{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        [Area("Admin")]
        [Route("/Admin/Product/Delete/{id}")]
        
        public IActionResult Delete(GetStatus get, int id)
        {
            var product = _context.products.Find(id);
            product.status = get.status;
            _context.SaveChanges();
            return RedirectToAction("Product");
        }
        [Area("Admin")]
        [Route("/Admin/Product/Restore/{id}")]
        [Authorize]
        public IActionResult Restore(int id)
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        [Area("Admin")]
        [Route("/Admin/Product/Restore/{id}")]

        public IActionResult Restore(GetStatus get, int id)
        {
            var product = _context.products.Find(id);
            product.status = get.status;
            _context.SaveChanges();
            return RedirectToAction("Product");
        }
        [Area("Admin")]
        public async Task<IActionResult> AdminLogOutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("AdminLogin", "Admin");

        }

    }
}
