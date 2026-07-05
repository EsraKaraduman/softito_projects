using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MarketMvcProject.Data;
using System.Linq;

namespace MarketMvcProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string kullaniciAdi, string sifre)
        {
            var user = _context.Kullanicilar
                .FirstOrDefault(x => x.KullaniciAdi == kullaniciAdi && x.Sifre == sifre);

            if (user == null)
            {
                ViewBag.Hata = "Kullanıcı adı veya şifre yanlış!";
                return View();
            }

            HttpContext.Session.SetString("Role", user.Rol);
            HttpContext.Session.SetString("UserName", user.KullaniciAdi);

            if (user.Rol == "Admin")
                return RedirectToAction("Index", "Admin");

            return RedirectToAction("Index", "Kullanici");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
