using KaiserStore.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using KaiserStore.Data;

namespace KaiserStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("/")]
        public async Task<IActionResult> Home()
        {
            var category = await _context.category.Where(a => a.active == "true").ToListAsync();
            ViewData["category"] = category;
            var product = _context.product.ToList();
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewData["Data"] = HttpContext.Session.GetString("UserSession");

            }
            return View(product);
        }
        public async Task<IActionResult> LogOut()
        {
            HttpContext.Session.Remove("UserSession");
            return RedirectToAction("login", "accounts");
          
            
        }
        [Route("/Category/{id}")]
        public async Task<IActionResult> Category(string id)
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewData["Data"] = HttpContext.Session.GetString("UserSession");

            }
            var category = await _context.category.Where(a => a.active == "true").ToListAsync();
            ViewData["category"] = category;
            var nameCategory = _context.category.Find(id);
            var product = _context.product.Where(a => a.productType == nameCategory.namecategory).ToList();
            return View(product);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}