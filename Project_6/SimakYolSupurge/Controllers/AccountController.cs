using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SimakYolSupurge.Models;
using SimakYolSupurge.Services;

namespace SimakYolSupurge.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiService _apiService;

        public AccountController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var response = await _apiService.PostAsync("auth/login", new
            {
                model.Username,
                model.Password
            });

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var authResult = System.Text.Json.JsonSerializer.Deserialize<ApiAuthResponse>(content, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (authResult != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, authResult.Username),
                        new Claim(ClaimTypes.Email, authResult.Email),
                        new Claim(ClaimTypes.Role, authResult.Role),
                        new Claim("JwtToken", authResult.Token)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var response = await _apiService.PostAsync("auth/register", new
            {
                model.Username,
                model.Email,
                model.Password,
                model.Role
            });

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Kayıt başarıyla tamamlandı! Giriş yapabilirsiniz.";
                return RedirectToAction("Login");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", errorContent ?? "Kayıt sırasında bir hata oluştu.");
            return View(model);
        }

        [HttpPost]
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
