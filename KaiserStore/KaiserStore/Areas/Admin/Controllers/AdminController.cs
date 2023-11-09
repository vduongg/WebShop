using KaiserStore.Data;
using KaiserStore.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KaiserStore.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Area("Admin")]
        [Route("Admin/Home")]
        [Authorize]
        public async Task<IActionResult> AdminHome()
        {
            List<HomeItem> ListHomeItems = new List<HomeItem>();
            HomeItem homeItem = new HomeItem();
            HomeItem homeItem1 = new HomeItem();
            HomeItem homeItem2 = new HomeItem();

            var size = await _context.sizes.ToListAsync();
            var spCon = 0;
            foreach (var item in size)
            {
                spCon += item.Quantity;
            }
            homeItem.name = "Sản phẩm còn lại";
            homeItem.icon = "fa-solid fa-warehouse";
            homeItem.value = spCon;
            homeItem.color = "#cd3333";
            ListHomeItems.Add(homeItem);
        

            var order = await _context.orders.ToListAsync();
            var spDB = 0;
            foreach (var item in order)
            {
                spDB += item.quantity;
            }
            homeItem1.name = "Sản phẩm đã bán";
            homeItem1.icon = "fa-solid fa-truck-fast";
            homeItem1.value = spDB;
            homeItem1.color = "#262687";
            ListHomeItems.Add(homeItem1);
          

            var sales = await _context.orders.Include("Product").ToListAsync();
            var dt = 0;
            foreach (var item in sales)
            {
                dt += item.quantity * item.Product.producdPrice;
            }
            homeItem2.name = "Doanh thu";
            homeItem2.icon = "fa-solid fa-money-check-dollar";
            homeItem2.value = dt;
            homeItem2.color = "green";
            ListHomeItems.Add(homeItem2);

            ViewData["HomeItem"] = ListHomeItems;
            return View();
          
        }
        [Area("Admin")]
        [Route("/Admin/Login")]
        public IActionResult AdminLogin()
        {
           
        
            return View();
            
        }
        [Area("Admin")]
        [Route("/Admin/Login")]
        [HttpPost]
        public async Task<IActionResult> AdminLoginAsync(LoginModel accounts)
        {
            var loginUser = _context.accounts.Where(a => a.username == accounts.user && a.password == accounts.pass).FirstOrDefault();
            var loginemail = _context.accounts.Where(a => a.email == accounts.user && a.password == accounts.pass).FirstOrDefault();
            if ( (loginUser != null && loginUser.role == "admin" )|| (loginemail != null && loginemail.role == "admin" ))
            {
                var name = "";
                if (loginUser != null)
                {
                    name = loginUser.name;
                }
                if (loginemail != null)
                {
                    name = loginemail.name;
                }
                List<Claim> claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, accounts.user),
                    new Claim(ClaimTypes.Name,name),

                };
                ClaimsIdentity claimsidentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,


                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsidentity), properties);
                return RedirectToAction("AdminHome");

            }
            else
            {
                ViewData["Error"] = "Tài khoản không hợp lệ!!";
            }
              return View();
        }
        [Area("Admin")]
        public async Task<IActionResult> AdminLogOutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("AdminLogin", "Admin");

        }


    }
}
