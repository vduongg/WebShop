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
        public IActionResult AdminHome()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                return View();
            }
            
            return RedirectToAction("AdminLogin");
        }
        [Area("Admin")]
        [Route("/Admin/Login")]
        public IActionResult AdminLogin()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("AdminHome");
            }
        
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
