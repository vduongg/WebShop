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
            var category = await _context.categorys.Where(a => a.status == "active").ToListAsync();
            ViewData["category"] = category;
            var product = _context.products.ToList();
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
            var category = await _context.categorys.Where(a => a.status == "active").ToListAsync();
            ViewData["category"] = category;
            var nameCategory = _context.categorys.Find(id);
            var product = _context.products.Where(a => a.categoryId == nameCategory.id).ToList();
            return View(product);
        }
        [Route("/Product/{id}")]
        public async Task<IActionResult> Product(string id)
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewData["Data"] = HttpContext.Session.GetString("UserSession");

            }
            var category = await _context.categorys.Where(a => a.status == "active").ToListAsync();
            ViewData["category"] = category;
            var nameProduct = _context.products.Find(int.Parse(id));
            
            return View(nameProduct);
        }
        [Route("/Order")]
        public async Task<IActionResult> Order()
        {
            var category = await _context.categorys.Where(a => a.status == "active").ToListAsync();
            ViewData["category"] = category;
            return View();

        }
        [Route("/Payments")]
        public async Task<IActionResult> Payments()
        {
            var category = await _context.categorys.Where(a => a.status == "active").ToListAsync();
            ViewData["category"] = category;
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}