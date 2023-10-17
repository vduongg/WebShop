using KaiserStore.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using KaiserStore.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace KaiserStore.Controllers
{
    public class AccountsController : Controller
    {

        private readonly ApplicationDbContext _context;
        public AccountsController(ApplicationDbContext context)
        {
            _context = context;
        }
       
        public async Task<IActionResult> Register()
        {
            var category = await _context.category.Where(a => a.active == "true").ToListAsync();
            ViewData["category"] = category;
            ClaimsPrincipal claimUser = HttpContext.User;
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                return RedirectToAction("Home", "Home");

            }
            //else
            //{
            //    ViewData["Role"] = "guest";
            //}
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Register(AccountsVM accounts )
        {
            var category = await _context.category.Where(a => a.active == "true").ToListAsync();
            ViewData["category"] = category;
            var loginUser = _context.accounts.Where(a => a.username == accounts.username).FirstOrDefault();
            var loginemail = _context.accounts.Where(a => a.email == accounts.email).FirstOrDefault();
            if (loginUser != null)
            {
                if (loginUser.username == accounts.username)
                {
                    ViewData["UserError"] = "Tài khoản  đã được sử dụng!";
                }
            }
            if( loginemail != null)
            {
                if (loginemail.email == accounts.email)
                {
                    ViewData["MailError"] = "Email đã được sử dụng!";
                }
            }
            if(loginUser == null && loginemail == null && ModelState.IsValid)
            {
               await _context.accounts.AddAsync(accounts);
               await _context.SaveChangesAsync();
               return RedirectToAction("login", "accounts");

            }
          
            return View();
        }
        public async Task<IActionResult> Login()
        {
            var category = await _context.category.Where(a => a.active == "true").ToListAsync();
            ViewData["category"] = category;
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                return RedirectToAction("Home", "Home");
                
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel accounts)
        {

            var category = await _context.category.Where(a => a.active == "true").ToListAsync();
            ViewData["category"] = category;
            var loginUser = _context.accounts.Where(a => a.username == accounts.user && a.password == accounts.pass).FirstOrDefault();
            var loginemail = _context.accounts.Where(a => a.email == accounts.user && a.password == accounts.pass).FirstOrDefault();
            if (loginUser != null || loginemail != null)
            {
                var name = "";
                if(loginUser != null) {
                    name = loginUser.name;
                }
                if (loginemail != null)
                {
                    name = loginemail.name;
                }

                HttpContext.Session.SetString("UserSession", name);
                return RedirectToAction("Home", "Home");
              
            }

            ViewData["Validate"] = "Tài khoản hoặc mật khẩu không hợp lệ!";
            return View();
        }
       

    }
}
