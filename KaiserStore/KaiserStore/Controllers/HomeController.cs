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
                total += (cartItem.quantity * cartItem.Product.producdPrice);
            }
            ViewData["cartTotal"] = total;
            var category = await _context.categorys.Where(a => a.status == "active").ToListAsync();
            ViewData["category"] = category;
            var product = _context.products.Where(p => p.status == "active").Include("category").ToList();
            var bestSell = await _context.products.Where(p=> p.status == "active").OrderBy(p => p.sold).ToListAsync();
            ViewData["bestSell"] = bestSell;
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewData["Data"] = HttpContext.Session.GetString("UserSession");

            }
            var slide = await _context.slides.Where(s=> s.status == "active").ToListAsync();
            ViewData["slide"] = slide;
            return View(product);
        }
        public async Task<IActionResult> LogOut()
        {
            HttpContext.Session.Remove("UserSession");
            HttpContext.Session.Remove("UserID");
            return RedirectToAction("login", "accounts");
          
            
        }
        [Route("/Category/{id}")]
        public async Task<IActionResult> Category(string id, int sPrice, int ePrice)
        {
           if( _context.categorys.Find(id).status == "disable")
            {
                return RedirectToAction("home", "home");
            }
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
            var nameCategory = _context.categorys.Find(id);
            if(sPrice != 0 && ePrice == 0)
            {
                var product = _context.products.Where(a => a.categoryId == nameCategory.id).Where(p => p.producdPrice >= sPrice).Where(p => p.status == "active").ToList();
                ViewData["product"] = product;
            }
            else if(sPrice == 0 && ePrice != 0)
            {
                var product = _context.products.Where(a => a.categoryId == nameCategory.id).Where(p => p.producdPrice <= ePrice).Where(p => p.status == "active").ToList();
                ViewData["product"] = product;
            }
            else if (sPrice == 0 && ePrice == 0)
            {
                var product = _context.products.Where(a => a.categoryId == nameCategory.id).Where(p => p.status == "active").ToList();
                ViewData["product"] = product;
            }
            else
            {
                var product = _context.products.Where(a => a.categoryId == nameCategory.id).Where(p => p.producdPrice >= sPrice).Where(p => p.producdPrice <= ePrice).Where(p => p.status == "active").ToList();
                ViewData["product"] = product;
            }
          
            return View();
        }
       


        [Route("/Product/{id}")]
        public async Task<IActionResult> ProductAsync(int id)
        {
            if ((await _context.products.Where(p => p.Id == id).Include("category").FirstOrDefaultAsync()).category.status == "disable")
            {
                return RedirectToAction("home", "home");
            }
            var cart =  await _context.carts.Where(c => c.UserId == HttpContext.Session.GetString("UserID")).Include("Product").ToListAsync();
            var total = 0;
            foreach (var cartItem in cart)
            {
                total += (cartItem.quantity * cartItem.Product.producdPrice);
            }
            ViewData["cartTotal"] = total;
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewData["Data"] = HttpContext.Session.GetString("UserSession");
                ViewData["ID"] = HttpContext.Session.GetString("UserID");

            }
            var ListSize =  await _context.sizes.Where(s => s.ProductId == id).Where(s=> s.Quantity > 0).ToListAsync();
            ViewData["ListSize"] = ListSize;
            var category =  await _context.categorys.Where(a => a.status == "active").ToListAsync();
            ViewData["category"] = category;
            var product = await  _context.products.FindAsync(id);
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
                total += (c.quantity * c.Product.producdPrice);
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


            if (flagQ.Quantity >= cartItem.quantity && cartItem.quantity > 0)
            {

                var cartIndex = await _context.carts.Where(p => p.UserId == cartItem.UserId).Where(p => p.ProductId == cartItem.ProductId).Where(p => p.Size == cartItem.Size).FirstOrDefaultAsync();
                if (cartIndex != null)
                {
                    if ((cartItem.quantity + cartIndex.quantity) <= flagQ.Quantity)
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
            else {
                ViewData["Error"] = "Số lượng nhập nhỏ hơn 0 hoặc lớn số lượng hiện có";
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
                total += (cartItem.quantity * cartItem.Product.producdPrice);
            }
            ViewData["cartTotal"] = total;
            ViewData["cart"] = cart;
            return View();

        }
        [HttpPost]
        [Route("/Order")]
        public async Task<IActionResult> Order(int id)
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
                total += (cartItem.quantity * cartItem.Product.producdPrice);
            }
            ViewData["cartTotal"] = total;
            ViewData["cart"] = cart;

            _context.Remove(_context.carts.Find(id));
            _context.SaveChanges();

            return RedirectToAction("Order");

        }
        [Route("/Payments")]
        public async Task<IActionResult> Payments()
        {
            ViewData["success"] = "";
            var cart = await _context.carts.Where(c => c.UserId == HttpContext.Session.GetString("UserID")).Include("Product").ToListAsync();
            List<Cart> cartList = new List<Cart>();
            List<Cart> soldOut = new List<Cart>();
            foreach (var item in cart)
            {
                if (item.quantity <= (await _context.sizes.Where(s => s.ProductId == item.ProductId).Where(s => s.Name == item.Size).FirstOrDefaultAsync()).Quantity)
                {
                    cartList.Add(item);
                }
                else
                {
                    soldOut.Add(item);
                }
            }
            var total = 0;
            foreach (var cartItem in cartList)
            {
                total += (cartItem.quantity * cartItem.Product.producdPrice);
            }
            ViewData["cartTotal"] = total;
            ViewData["cart"] = cartList;
            ViewData["soldOut"] = soldOut;

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
            ViewData["success"] = "";
            Order order = new Order();
            var cart = await _context.carts.Where(c => c.UserId == HttpContext.Session.GetString("UserID")).Include("Product").ToListAsync();
            List<Cart> cartList = new List<Cart>();
            List<Cart> soldOut = new List<Cart>();
            foreach(var item in cart)
            {
                if(item.quantity <= (await _context.sizes.Where(s=> s.ProductId == item.ProductId).Where(s=> s.Name == item.Size).FirstOrDefaultAsync()).Quantity)
                {
                    cartList.Add(item);
                }
                else
                {
                    soldOut.Add(item);
                }
            }


            var total = 0;
            var count = 0;
            foreach (var cartItem in cart)
            {
                total += (cartItem.quantity * cartItem.Product.producdPrice);
                count += cartItem.quantity;
            }
            ViewData["cartTotal"] = total;
            ViewData["cart"] = cartList;
            ViewData["soldOut"] = soldOut;

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
            if(ModelState.IsValid && cartList.Count() > 0)
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

                    var sold = await _context.products.Where(p => p.Id == item.ProductId).FirstOrDefaultAsync();
                    sold.sold += item.quantity;
                    _context.SaveChanges();


                 

                }

                ViewData["success"] = "Bạn đã đặt hàng thành công";
                return View();
            }
          
          


            return View();
        }
        [Route("/paymentHistory")]
        public async Task<IActionResult> PaymentHistory()
        {
            var cart = await _context.carts.Where(c => c.UserId == HttpContext.Session.GetString("UserID")).Include("Product").ToListAsync();
            var total = 0;
            foreach (var cartItem in cart)
            {
                total += (cartItem.quantity * cartItem.Product.producdPrice);
            }
            ViewData["cartTotal"] = total;
            var category = await _context.categorys.Where(a => a.status == "active").ToListAsync();
            ViewData["category"] = category;
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewData["Data"] = HttpContext.Session.GetString("UserSession");
                ViewData["ID"] = HttpContext.Session.GetString("UserID");

            }
            else
            {
                return RedirectToAction("login", "accounts");
            }
            var payment = await _context.payments.Where(p => p.UserId == HttpContext.Session.GetString("UserID")).ToListAsync();
            ViewData["payment"] = payment;
            return View();
        }
        [Route("/ChangePass")]
        public async Task<IActionResult> ChangePassAsync() 
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
            ViewData["success"] = "";
            return View();
        }
        [HttpPost]
        [Route("/ChangePass")]
        public async Task<IActionResult> ChangePassAsync(string oldPass, string newPass, string repeatNew)
        {
            ViewData["success"] = "";
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
            var user = _context.accounts.Find(HttpContext.Session.GetString("UserID"));
            if(user.password == oldPass)
            {
                if( newPass == repeatNew)
                {
                    user.password = newPass;
                    _context.SaveChanges();
                    ViewData["success"] = "Đổi mật khẩu thành công";
                }
                else
                {
                    ViewData["error"] = "Mật khẩu mới không khớp";
                }
            }
            else
            {
                ViewData["error"] = "Mật khẩu không khớp với mật khẩu hiện tại";
            }
            return View();
        }
        [Route("/paymentHistory/ListProduct")]
        public async Task<IActionResult> ListProduct(int id)
        {
            var cart = await _context.carts.Where(c => c.UserId == HttpContext.Session.GetString("UserID")).Include("Product").ToListAsync();
            var total = 0;
            foreach (var cartItem in cart)
            {
                total += (cartItem.quantity * cartItem.Product.producdPrice);
            }
            ViewData["cartTotal"] = total;
            var category = await _context.categorys.Where(a => a.status == "active").ToListAsync();
            ViewData["category"] = category;
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewData["Data"] = HttpContext.Session.GetString("UserSession");
                ViewData["ID"] = HttpContext.Session.GetString("UserID");

            }
            else
            {
                return RedirectToAction("login", "accounts");
            }
      
            var order = await _context.orders.Where(o => o.PaymentId == id).Include("Product").ToListAsync();

            ViewData["order"] = order;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}