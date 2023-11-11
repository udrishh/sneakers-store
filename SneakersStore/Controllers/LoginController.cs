using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SneakersStore.Models.Entities;
using SneakersStore.Models.VMs;

namespace SneakersStore.Controllers
{
    public class LoginController : Controller
    {
        private readonly StoreDbContext _storeDbContext;

        public LoginController(StoreDbContext _storeDbContext)
        {
            this._storeDbContext = _storeDbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new LoginVM());
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "There were some errors in your form");
                return View(loginVM);
            }

            var user = GetAllUsers().FirstOrDefault(u => u.UserName == loginVM.UserName);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Credentials");
                return View(loginVM);
            }

            if (user.Password != loginVM.Password) 
            {
                ModelState.AddModelError(string.Empty, "Invalid Credentials");
                return View(loginVM);
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, loginVM.UserName),
            };

            ClaimsIdentity claimsIdentity= new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = false,
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

            HttpContext.Session.Set(SessionHelper.LoggedUser, user);
            
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Session.Set(SessionHelper.LoggedUser, new User());
            
            return RedirectToAction("Index", "Home");
        }

        public List<User> GetAllUsers()
        {
            return _storeDbContext.Users.Select(user => user).ToList();
        }

    }
}
