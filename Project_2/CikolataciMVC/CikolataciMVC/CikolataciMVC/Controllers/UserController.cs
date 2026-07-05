using CikolataciMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CikolataciMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "User")
                return RedirectToAction("Login", "Account");

            var username = HttpContext.Session.GetString("KullaniciAdi");
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Account");

            var user = await _context.Kullanicilar
                .Include(u => u.Satislar!)
                    .ThenInclude(s => s.Urun)
                .FirstOrDefaultAsync(u => u.KullaniciAdi == username);

            if (user == null)
                return RedirectToAction("Login", "Account");

            ViewBag.Urunler = await _context.Urunler.ToListAsync();

            decimal topHarcanan = 0;
            int topAdet = 0;
            string favoriUrun = "Yok";

            if (user.Satislar != null && user.Satislar.Any())
            {
                topHarcanan = user.Satislar.Sum(s => s.Adet * (s.Urun?.Fiyat ?? 0));
                topAdet = user.Satislar.Sum(s => s.Adet);
                
                var groupProduct = user.Satislar
                    .Where(s => s.Urun != null)
                    .GroupBy(s => s.Urun!.UrunAdi)
                    .OrderByDescending(g => g.Sum(s => s.Adet))
                    .FirstOrDefault();
                    
                if (groupProduct != null)
                {
                    favoriUrun = groupProduct.Key;
                }
            }

            ViewBag.TopHarcanan = topHarcanan;
            ViewBag.TopAdet = topAdet;
            ViewBag.FavoriUrun = favoriUrun;

            return View(user.Satislar ?? new List<Satislar>());
        }

        [HttpPost]
        public async Task<IActionResult> SatinAl(int urunId, int adet)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "User")
                return RedirectToAction("Login", "Account");

            var username = HttpContext.Session.GetString("KullaniciAdi");
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Account");

            var user = await _context.Kullanicilar.FirstOrDefaultAsync(u => u.KullaniciAdi == username);
            if (user == null)
                return RedirectToAction("Login", "Account");

            if (urunId > 0 && adet > 0)
            {
                var satis = new Satislar
                {
                    UrunId = urunId,
                    KullaniciId = user.Id,
                    Adet = adet,
                    Tarih = DateTime.Now
                };

                _context.Satislar.Add(satis);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
