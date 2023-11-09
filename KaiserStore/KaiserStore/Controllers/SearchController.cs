using KaiserStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KaiserStore.Controllers
{
    public class SearchController : Controller
    {
        public readonly ApplicationDbContext _context;
        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Route("/Search")]
        public async Task<IActionResult> Search(string search)
        {
            var cart = await _context.carts.Where(c => c.UserId == HttpContext.Session.GetString("UserID")).Include("Product").ToListAsync();
            var total = 0;
            foreach (var cartItem in cart)
            {
                total += (cartItem.quantity * cartItem.Product.producdPrice);
            }
            ViewData["cartTotal"] = total;
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewData["Data"] = HttpContext.Session.GetString("UserSession");


            }
            var category = await _context.categorys.Where(a => a.status == "active").ToListAsync();
            ViewData["category"] = category;
           
            var product = _context.products.Where(a => a.producdName.Contains(search)).Where(p => p.status == "active").ToList();
            ViewData["product"] = product;
            return View();
        }
        
    }
}
