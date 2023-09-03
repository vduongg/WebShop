using KaiserStore.Data;
using KaiserStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KaiserStore.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Area("Admin")]
        [Route("/Admin/Category")]
        public async Task<IActionResult> Category()
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
            var category =  await _context.category.Where(a => a.active == "true").ToListAsync();
            return View(category);

        }
        [Area("Admin")]
        [Route("/Admin/Category/Create")]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
            return View();
        }

   
        [HttpPost]
        [Area("Admin")]
        [Route("/Admin/Category/Create")]

        public async Task<IActionResult> Create(CategoryVM category)
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
            var size = await _context.category.ToListAsync();
            var categoryVM =  _context.category.Where(m => m.id == category.id).FirstOrDefault();
            if (categoryVM == null)
            {
                if (size.Count() <= 6) {
                    if (ModelState.IsValid)
                    {
                        _context.Add(category);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Category");
                    }
                }
                else
                {
                    ViewData["Status"] = "Đã đạt số danh mục tối đa";
                }
                
            }
            
            else
            {
                ViewData["Status"] = "Danh mục đã có sẵn";
            }
            return View();
        }


        [Area("Admin")]
        [Route("/Admin/Category/Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
            if (id != null || _context.category != null)
            {
                var categoryVM = await _context.category.FindAsync(id);
                return View(categoryVM);
            }
            return View();
        }
        [HttpPost]
        [Area("Admin")]
        [Route("/Admin/Category/Edit")]
        public async Task<IActionResult> Edit(string id, CategoryVM categoryVM)
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
            {
                return RedirectToAction("AdminLogin", "Admin");
            }

            if (ModelState.IsValid)
            {
                {
                    _context.Update(categoryVM);
                    await _context.SaveChangesAsync();
              
                }
                return RedirectToAction("Category");
            }
            return View();
        }
        [Area("Admin")]
        [Route("/Admin/Category/Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
            if (id == null || _context.category == null)
            {
                return NotFound();
            }

            var categoryVM = await _context.category
                .FirstOrDefaultAsync(m => m.id == id);
            if (categoryVM == null)
            {
                return NotFound();
            }

            return View(categoryVM);
        }
        [HttpPost, ActionName("Delete")]
        [Area("Admin")]
        [Route("/Admin/Category/Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id, CategoryVM categoryVM)
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
            {
                return RedirectToAction("AdminLogin","Admin");
            }
            if (ModelState.IsValid)
            {
                {
                    _context.Update(categoryVM);
                    await _context.SaveChangesAsync();

                }
                return RedirectToAction("Category");
            }
            return View();
        }

        [Area("Admin")]
        public IActionResult AdminLogOut()
        {
            HttpContext.Session.Remove("AdminSession");
            return RedirectToAction("AdminLogin", "Admin");

        }


        private bool CategoryVMExists(string id)
        {
            return (_context.category?.Any(e => e.id == id)).GetValueOrDefault();
        }



    }
}
