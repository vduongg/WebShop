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
            if (loginUser != null || loginemail != null)
            {
                HttpContext.Session.SetString("AdminSession", loginUser.username);
                return RedirectToAction("AdminHome");
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
