using KaiserStore.Data;
using KaiserStore.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KaiserStore.Areas.Admin.Controllers
{
    public class ProTypeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProTypeController(ApplicationDbContext context) {
            _context = context;
        }
        [Area("Admin")]
        [Route("/Admin/ProductType")]
        public async Task<IActionResult> ProductType()
        {
            var productType = await _context.productTypes.ToListAsync();
            return View(productType);
        }
        [Area("Admin")]
        [Route("/Admin/ProductType/Create")]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewData["category"] = await _context.categorys.ToListAsync();
            return View();
        }


        [HttpPost]
        [Area("Admin")]
        [Route("/Admin/ProductType/Create")]
        [Authorize]
        public async Task<IActionResult> Create(ProductType productType)
        {
            ViewData["category"] = await _context.categorys.ToListAsync();
            var categoryVM = _context.productTypes.Where(m => m.Id == productType.Id).FirstOrDefault();
            if (categoryVM == null)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(productType);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ProductType");

                }
            }
            else
            {
                ViewData["Status"] = "Loại sản phẩm đã có sẵn";
            }
            return View();
        }
        [Area("Admin")]
        [Route("/Admin/ProductType/Edit/{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["category"] = await _context.categorys.ToListAsync();
            if (id != 0 || _context.productTypes != null)
            {
             
                var productType = await _context.productTypes.FindAsync(id);
                return View(productType);
            }
            return View();
        }
        [HttpPost]
        [Area("Admin")]
        [Route("/Admin/ProductType/Edit/{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(string id, ProductType productType)
        {
            ViewData["category"] = await _context.categorys.ToListAsync();
            if (ModelState.IsValid)
            {
                {
                    _context.Update(productType);
                    await _context.SaveChangesAsync();

                }
                return RedirectToAction("ProductType");
            }
            return View();
        }
        [Area("Admin")]
        [Route("/Admin/ProductType/Delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
         
            return View();
        }
        [HttpPost, ActionName("Delete")]
        [Area("Admin")]
        [Route("/Admin/ProductType/Delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id, GetStatus get)
        {
            //if (HttpContext.Session.GetString("AdminSession") == null)
            //{
            //    return RedirectToAction("AdminLogin","Admin");
            //}
            if (ModelState.IsValid)
            {

                var productType = _context.productTypes.Find(id);
                productType.status = get.status;
                _context.SaveChanges();



                return RedirectToAction("ProductType");
            }
            return View();
        }

        [Area("Admin")]
        [Route("/Admin/ProductType/Restore/{id}")]
        [Authorize]
        public IActionResult Restore(int id)
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        [Area("Admin")]
        [Route("/Admin/ProductType/Restore/{id}")]

        public async Task<IActionResult> Restore(GetStatus get, int id)
        {
            var productType = _context.productTypes.Find(id);
            productType.status = get.status;
            _context.SaveChanges();
           
            return RedirectToAction("ProductType");
        }
        [Area("Admin")]
        public async Task<IActionResult> AdminLogOutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("AdminLogin", "Admin");

        }
    }
}
