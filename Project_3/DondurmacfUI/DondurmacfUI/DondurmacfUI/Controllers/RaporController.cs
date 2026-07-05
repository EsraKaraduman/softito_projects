using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Dondurmacf.Data.Data;
using DondurmacfUI.Models;

namespace DondurmacfUI.Controllers
{
    [Authorize]
    public class RaporController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RaporController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var r1 = _context.Turs
                .Join(_context.Uruns,
                      t => t.UrunNo,
                      u => u.UrunNo,
                      (t, u) => new Rapor1Model { TurAdi = t.Tur, Fiyat = t.Fiyat, UrunAdi = u.UrunAdi })
                .ToList();

            var r2 = _context.Turs
                .Join(_context.Kullanicis,
                      t => t.EkleyenKullaniciNo,
                      k => k.KullaniciNo,
                      (t, k) => new Rapor2Model { TurAdi = t.Tur, Fiyat = t.Fiyat, Olusturan = k.AdSoyad })
                .ToList();

            var r3 = _context.Turs
                .Join(_context.Uruns, t => t.UrunNo, u => u.UrunNo, (t, u) => new { t, u })
                .Join(_context.Kullanicis, tu => tu.t.EkleyenKullaniciNo, k => k.KullaniciNo, (tu, k) => new Rapor3Model
                {
                    TurAdi = tu.t.Tur,
                    Fiyat = tu.t.Fiyat,
                    UrunAdi = tu.u.UrunAdi,
                    Olusturan = k.AdSoyad
                })
                .ToList();

            var r4 = _context.Turs
                .Include(t => t.Urun)
                .GroupBy(t => t.Urun.UrunAdi)
                .Select(g => new Rapor4Model
                {
                    UrunAdi = g.Key ?? "Belirtilmemiş",
                    TurSayisi = g.Count(),
                    OrtalamaFiyat = g.Average(t => t.Fiyat)
                })
                .ToList();

            var r5 = _context.Turs
                .Include(t => t.Urun)
                .OrderByDescending(t => t.Fiyat)
                .Take(3)
                .Select(t => new Rapor5Model
                {
                    TurAdi = t.Tur,
                    Fiyat = t.Fiyat,
                    UrunAdi = t.Urun.UrunAdi
                })
                .ToList();

            var viewModel = new RaporViewModel
            {
                Rapor1 = r1,
                Rapor2 = r2,
                Rapor3 = r3,
                Rapor4 = r4,
                Rapor5 = r5
            };

            return View(viewModel);
        }
    }
}
