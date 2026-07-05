using Microsoft.AspNetCore.Mvc;
using MarketMvcProject.Data;
using MarketMvcProject.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace MarketMvcProject.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly AppDbContext _context;

        public KullaniciController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");

            if (role == null)
                return RedirectToAction("Login", "Account");

            if (role != "User")
                return RedirectToAction("Login", "Account");

            var kullanicilar = _context.Kullanicilar.ToList();
            return View(kullanicilar);
        }
    }
}
