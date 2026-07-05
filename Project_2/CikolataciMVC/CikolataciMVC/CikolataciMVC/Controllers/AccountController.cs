using CikolataciMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CikolataciMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
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
                ViewBag.Hata = "Kullanıcı adı veya şifre yanlış";
                return View();
            }

            HttpContext.Session.SetString("KullaniciAdi", user.KullaniciAdi);
            HttpContext.Session.SetString("Rol", user.Rol);

            if (user.Rol == "Admin")
                return RedirectToAction("Index", "Admin");

            return RedirectToAction("Index", "User");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}

