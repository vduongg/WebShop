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
       
        public IActionResult Register()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Home", "Home");
            }
            else
            {
                ViewData["Role"] = "guest";
            }
            return View();

        }
        
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if(claimUser.Identity.IsAuthenticated) {
                return RedirectToAction("Home", "Home");
            }
            else
            {
                ViewData["Role"] = "guest";
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel accounts)
        {
            var loginUser = _context.accounts.Where(a => a.username == accounts.user && a.password == accounts.pass).FirstOrDefault();
            var loginemail = _context.accounts.Where(a => a.email == accounts.user && a.password == accounts.pass).FirstOrDefault();
            if (loginUser != null || loginemail != null)
            {
               
                List<Claim> claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, accounts.user),
                    new Claim(ClaimTypes.Name,loginUser.name),

                };
                ClaimsIdentity claimsidentity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                 

                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsidentity),properties);
               
                return RedirectToAction("Home","Home");
            }
         

            return View();
        }
       

    }
}
