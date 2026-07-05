using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project_7.Models;
using Project_7.Repositories;

namespace Project_7.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>();
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
        public async Task<IActionResult> Register(string fullName, string email, string password)
        {
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError(string.Empty, "Bütün alanlar zorunludur.");
                return View();
            }

            var existingUser = await _userRepository.GetByEmailAsync(email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Bu e-posta adresi zaten kullanımda.");
                return View();
            }

            var newUser = new User
            {
                FullName = fullName,
                Email = email,
                Role = "Student"
            };

            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, password);

            var userId = await _userRepository.RegisterAsync(newUser);
            if (userId > 0)
            {
                return RedirectToAction("Login");
            }

            ModelState.AddModelError(string.Empty, "Kayıt işlemi sırasında bir hata oluştu.");
            return View();
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
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError(string.Empty, "E-posta ve şifre zorunludur.");
                return View();
            }

            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Geçersiz e-posta veya şifre.");
                return View();
            }

            PasswordVerificationResult verificationResult;
            try
            {
                verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            }
            catch (System.FormatException)
            {
                verificationResult = user.PasswordHash == password
                    ? PasswordVerificationResult.Success
                    : PasswordVerificationResult.Failed;
            }

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Geçersiz e-posta veya şifre.");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
