using KaiserStore.Data;
using KaiserStore.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KaiserStore.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Area("Admin")]
        [Route("/Admin/ListOrder")]
        [Authorize]
        public async Task<IActionResult> ListOrder()
        {
            var payment = await _context.payments.ToListAsync();
            ViewData["payment"] = payment;
            return View();
        }
        [Area("Admin")]
        [Route("/Admin/Details")]
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var payment = await _context.payments.Where(p => p.PaymentId == id).FirstOrDefaultAsync();
            var product = await _context.orders.Where(p => p.PaymentId == id).Include("Product").ToListAsync();
            ViewData["payment"] = payment;
            ViewData["product"] = product;

            return View();
        }
        [HttpPost]
        [Area("Admin")]
        [Route("/Admin/Details")]
        [Authorize]
        public async Task<IActionResult> Details( GetStatus get ,int id)
        {
            var payment = await _context.payments.Where(p => p.PaymentId == id).FirstOrDefaultAsync();
            payment.status = get.status;
            _context.SaveChanges();
            var product = await _context.orders.Where(p => p.PaymentId == id).Include("Product").ToListAsync();
            ViewData["payment"] = payment;
            ViewData["product"] = product;
            
            return RedirectToAction("ListOrder");
        }
        [Area("Admin")]
        public async Task<IActionResult> AdminLogOutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("AdminLogin", "Admin");

        }
    }
}
