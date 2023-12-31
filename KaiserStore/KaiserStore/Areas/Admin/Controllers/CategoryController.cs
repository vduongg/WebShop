﻿using KaiserStore.Data;
using KaiserStore.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<IActionResult> Category()
        {
            var category = await _context.categorys.ToListAsync();
            return View(category);

        }
        [Area("Admin")]
        [Route("/Admin/Category/Create")]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

   
        [HttpPost]
        [Area("Admin")]
        [Route("/Admin/Category/Create")]
        [Authorize]
        public async Task<IActionResult> Create(CategoryVM category)
        {
 
            var size = await _context.categorys.ToListAsync();
            var categoryVM =  _context.categorys.Where(m => m.id == category.id).FirstOrDefault();
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
        [Route("/Admin/Category/Edit/{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
        
            if (id != null || _context.categorys != null)
            {
                var categoryVM = await _context.categorys.FindAsync(id);
                return View(categoryVM);
            }
            return View();
        }
        [HttpPost]
        [Area("Admin")]
        [Route("/Admin/Category/Edit/{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(string id, CategoryVM categoryVM)
        {
        
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
        [Route("/Admin/Category/Delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
      
            if (id == null || _context.categorys == null)
            {
                return NotFound();
            }

            var categoryVM = await _context.categorys
                .FirstOrDefaultAsync(m => m.id == id);
            if (categoryVM == null)
            {
                return NotFound();
            }

            return View(categoryVM);
        }
        [HttpPost, ActionName("Delete")]
        [Area("Admin")]
        [Route("/Admin/Category/Delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(string id, CategoryVM categoryVM)
        {
            //if (HttpContext.Session.GetString("AdminSession") == null)
            //{
            //    return RedirectToAction("AdminLogin","Admin");
            //}
            if (ModelState.IsValid)
            {
                
                _context.Update(categoryVM);
                await _context.SaveChangesAsync();

                var type = await _context.productTypes.Where(p => p.categoryId == categoryVM.id).ToListAsync();
                foreach (var item in type)
                {
                    item.status = "disable";
                    _context.SaveChanges();
                    
                }



                return RedirectToAction("Category");
            }
            return View();
        }

        [Area("Admin")]
        [Route("/Admin/Category/Restore/{id}")]
        [Authorize]
        public IActionResult Restore(string id)
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        [Area("Admin")]
        [Route("/Admin/Category/Restore/{id}")]

        public async Task<IActionResult> Restore(GetStatus get, string id)
        {
            var category = _context.categorys.Find(id);
            category.status = get.status;
            _context.SaveChanges();
            var type = await _context.productTypes.Where(p => p.categoryId == id).ToListAsync();
            foreach (var item in type)
            {
                item.status = "active";
                _context.SaveChanges();

            }
            return RedirectToAction("Category");
        }

        [Area("Admin")]
        public async Task<IActionResult> AdminLogOutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("AdminLogin", "Admin");

        }


        private bool CategoryVMExists(string id)
        {
            return (_context.categorys?.Any(e => e.id == id)).GetValueOrDefault();
        }



    }
}
