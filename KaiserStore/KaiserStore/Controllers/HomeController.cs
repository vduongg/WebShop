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
            var cart = await _context.carts.Where(c => c.UserId == HttpContext.Session.GetString("UserID")).Include("Product").ToListAsync();
            var total = 0;
            foreach (var cartItem in cart)
            {
                total += (cartItem.quantity * int.Parse(cartItem.Product.producdPrice));
            }
            ViewData["cartTotal"] = total;
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
            HttpContext.Session.Remove("UserID");
            return RedirectToAction("login", "accounts");
          
            
        }
        [Route("/Category/{id}")]
        public async Task<IActionResult> Category(string id)
        {
            var cart = await _context.carts.Where(c => c.UserId == HttpContext.Session.GetString("UserID")).Include("Product").ToListAsync();
            var total = 0;
            foreach (var cartItem in cart)
            {
                total += (cartItem.quantity * int.Parse(cartItem.Product.producdPrice));
            }
            ViewData["cartTotal"] = total;
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
        public  IActionResult Product(int id)
        {
            var cart =  _context.carts.Where(c => c.UserId == HttpContext.Session.GetString("UserID")).Include("Product").ToList();
            var total = 0;
            foreach (var cartItem in cart)
            {
                total += (cartItem.quantity * int.Parse(cartItem.Product.producdPrice));
            }
            ViewData["cartTotal"] = total;
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewData["Data"] = HttpContext.Session.GetString("UserSession");
                ViewData["ID"] = HttpContext.Session.GetString("UserID");

            }
            var ListSize =  _context.sizes.Where(s => s.ProductId == id).Where(s=> s.Quantity > 0).ToList();
            ViewData["ListSize"] = ListSize;
            var category =  _context.categorys.Where(a => a.status == "active").ToList();
            ViewData["category"] = category;
            var product =  _context.products.Find(id);
            ViewData["productItem"] = product;
            return View();
        }
        [HttpPost]
        [Route("/Product/{id}")]
        public async Task<IActionResult> Product(Cart cartItem, int id)
        {
            var cart = await _context.carts.Where(c => c.UserId == HttpContext.Session.GetString("UserID")).Include("Product").ToListAsync();
            var total = 0;
            foreach (var c in cart)
            {
                total += (cartItem.quantity * int.Parse(c.Product.producdPrice));
            }
            ViewData["cartTotal"] = total;

            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewData["Data"] = HttpContext.Session.GetString("UserSession");
                ViewData["ID"] = HttpContext.Session.GetString("UserID");
            }
            var product = await _context.products.FindAsync(id);
            ViewData["productItem"] = product;
            var ListSize = await   _context.sizes.Where(s => s.ProductId == id).Where(s=> s.Quantity > 0).ToListAsync();
            ViewData["ListSize"] = ListSize;
            var category = await  _context.categorys.Where(a => a.status == "active").ToListAsync();
            ViewData["category"] = category;

            var flagQ = await _context.sizes.Where(s => s.ProductId == cartItem.ProductId).Where(s => s.Name == cartItem.Size).FirstOrDefaultAsync();
         
      
            if (flagQ.Quantity >= cartItem.quantity && cartItem.quantity > 0 )
            {

                var cartIndex = await _context.carts.Where(p => p.UserId == cartItem.UserId).Where(p => p.ProductId == cartItem.ProductId).FirstOrDefaultAsync();
                if (cartIndex != null)
                {
                    if ( (cartItem.quantity + cartIndex.quantity) <= flagQ.Quantity) 
                    {
                        cartIndex.quantity += cartItem.quantity;
                        _context.SaveChanges();
                        return RedirectToAction("Home");
                    }
                    

                }
                else
                {
                    cartItem.Id = 0;
                    _context.Add(cartItem);
                    _context.SaveChanges();
                    return RedirectToAction("Home");
                }

            }

            
   


            return View();
        }
        [Route("/Order")]
        public async Task<IActionResult> Order()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewData["Data"] = HttpContext.Session.GetString("UserSession");
                ViewData["ID"] = HttpContext.Session.GetString("UserID");

            }
            else
            {
                return RedirectToAction("login", "accounts");
            }
            var category = await _context.categorys.Where(a => a.status == "active").ToListAsync();
            ViewData["category"] = category;
            var cart = await _context.carts.Where(c => c.UserId == HttpContext.Session.GetString("UserID")).Include("Product").ToListAsync();
            var total = 0;
            foreach (var cartItem in cart)
            {
                total += (cartItem.quantity * int.Parse(cartItem.Product.producdPrice));
            }
            ViewData["cartTotal"] = total;
            ViewData["cart"] = cart;
            return View();

        }
        [Route("/Payments")]
        public async Task<IActionResult> Payments()
        {
            var cart = await _context.carts.Where(c => c.UserId == HttpContext.Session.GetString("UserID")).Include("Product").ToListAsync();
            var total = 0;
            foreach (var cartItem in cart)
            {
                total += (cartItem.quantity * int.Parse(cartItem.Product.producdPrice));
            }
            ViewData["cartTotal"] = total;
            ViewData["cart"] = cart;

            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewData["Data"] = HttpContext.Session.GetString("UserSession");
                ViewData["ID"] = HttpContext.Session.GetString("UserID");

            }
            else
            {
                return RedirectToAction("login", "accounts");
            }
            var category = await _context.categorys.Where(a => a.status == "active").ToListAsync();
            ViewData["category"] = category;
            return View();
        }
        [HttpPost]
        [Route("/Payments")]
        public async Task<IActionResult> Payments(Payment payment)
        {
            Order order = new Order();
            var cart = await _context.carts.Where(c => c.UserId == HttpContext.Session.GetString("UserID")).Include("Product").ToListAsync();
            var total = 0;
            var count = 0;
            foreach (var cartItem in cart)
            {
                total += (cartItem.quantity * int.Parse(cartItem.Product.producdPrice));
                count += cartItem.quantity;
            }
            ViewData["cartTotal"] = total;
            ViewData["cart"] = cart;

            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewData["Data"] = HttpContext.Session.GetString("UserSession");
                ViewData["ID"] = HttpContext.Session.GetString("UserID");

            }
            else
            {
                return RedirectToAction("login", "accounts");
            }
            var category = await _context.categorys.Where(a => a.status == "active").ToListAsync();
            ViewData["category"] = category;
            if(ModelState.IsValid)
            {
                payment.PaymentId = 0;
                payment.Total = count;
                payment.DateTime = DateTime.Now;
                _context.Add(payment);
                _context.SaveChanges();

                var p = await _context.payments.Where(p => p.UserId == HttpContext.Session.GetString("UserID")).ToListAsync();


                foreach (var item in cart)
                {
                    order.OrderId = 0;
                    order.PaymentId = p[p.Count() - 1].PaymentId;
                    order.ProductId = item.ProductId;
                    order.Size = item.Size;
                    order.quantity = item.quantity;
                    _context.Add(order);
                    _context.SaveChanges();

                    _context.Remove(_context.carts.Find(item.Id));
                    _context.SaveChanges();

                    var product = await _context.sizes.Where(p => p.ProductId == item.ProductId).Where(s=> s.Name == item.Size).FirstOrDefaultAsync();
                    product.Quantity = product.Quantity - item.quantity;
                    _context.SaveChanges();
                 

                }

                
                
                return RedirectToAction("home");
            }
          


            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}