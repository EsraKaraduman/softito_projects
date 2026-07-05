using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HotelProject.MVC.Models;

namespace HotelProject.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public AccountController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Rooms");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var client = _clientFactory.CreateClient("HotelApi");
            var response = await client.PostAsJsonAsync("auth/login", new { Email = email, Password = password });

            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadFromJsonAsync<User>();
                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, user.Role)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties { IsPersistent = true };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    return RedirectToAction("Index", "Rooms");
                }
            }

            ViewBag.Error = "Geçersiz e-posta veya şifre.";
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Rooms");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string email, string password, string role)
        {
            var client = _clientFactory.CreateClient("HotelApi");
            var newUser = new User
            {
                Username = username,
                Email = email,
                PasswordHash = password,
                Role = string.IsNullOrEmpty(role) ? "Guest" : role
            };

            var response = await client.PostAsJsonAsync("auth/register", newUser);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            ViewBag.Error = "Kayıt işlemi başarısız: " + errorContent;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
