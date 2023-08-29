using KaiserStore.Data;
using KaiserStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult AdminHome()
        {
            if(HttpContext.Session.GetString("AdminSession") == null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {


            }
            return View();
        }
        [Area("Admin")]
        public IActionResult AdminLogin()
        {
            return View();
        }
        [Area("Admin")]
        [HttpPost]
        public IActionResult AdminLogin(LoginModel accounts)
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

                HttpContext.Session.SetString("AdminSession", name);
                return RedirectToAction("AdminHome");
            }
            else
            {
                ViewData["Error"] = "Tài khoản không hợp lệ!!";
            }
              return View();
        }
        [Area("Admin")]
        [HttpPost]
        public IActionResult AdminLogOut()
        {
            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                HttpContext.Session.Remove("AdminSession");
                return RedirectToAction("AdminLogin");
            }
            return View();
        }
    }
}
