using CikolataciMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CikolataciMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Admin")
                return RedirectToAction("Login", "Account");

            var satislar = await _context.Satislar.Include(s => s.Urun).ToListAsync();
            var toplamGelir = satislar.Sum(s => s.Adet * (s.Urun?.Fiyat ?? 0));
            var toplamSatisAdet = satislar.Sum(s => s.Adet);
            var toplamKullanici = await _context.Kullanicilar.CountAsync();
            var toplamUrun = await _context.Urunler.CountAsync();

            ViewBag.ToplamGelir = toplamGelir;
            ViewBag.ToplamSatisAdet = toplamSatisAdet;
            ViewBag.ToplamKullanici = toplamKullanici;
            ViewBag.ToplamUrun = toplamUrun;

            return View();
        }
    }
}

