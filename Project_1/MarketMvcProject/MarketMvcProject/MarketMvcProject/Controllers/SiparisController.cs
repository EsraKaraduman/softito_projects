using MarketMvcProject.Data;
using MarketMvcProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketMvcProject.Controllers
{
    public class SiparisController : Controller
    {
        private readonly AppDbContext _db;

        public SiparisController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(DateTime? tarih)
        {
            var siparisler = _db.Siparisler
                .Include(x => x.Musteri)
                .Include(x => x.Urun)
                .AsQueryable();

            if (tarih != null)
            {
                siparisler = siparisler.Where(x => x.Tarih.Date == tarih.Value.Date);
            }

            return View(siparisler.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Musteriler = _db.Musteriler.ToList();
            ViewBag.Urunler = _db.Urunler.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Siparis siparis)
        {
            siparis.Tarih = DateTime.Now;
            _db.Siparisler.Add(siparis);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var siparis = _db.Siparisler
                .Include(x => x.Musteri)
                .Include(x => x.Urun)
                .FirstOrDefault(x => x.SiparisId == id);

            ViewBag.Musteriler = _db.Musteriler.ToList();
            ViewBag.Urunler = _db.Urunler.ToList();

            return View(siparis);
        }

        [HttpPost]
        public IActionResult Edit(Siparis siparis)
        {
            var mevcut = _db.Siparisler.Find(siparis.SiparisId);

            mevcut.MusteriId = siparis.MusteriId;
            mevcut.UrunId = siparis.UrunId;
            mevcut.Tarih = DateTime.Now;

            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var siparis = _db.Siparisler
                .Include(x => x.Musteri)
                .Include(x => x.Urun)
                .FirstOrDefault(x => x.SiparisId == id);

            return View(siparis);
        }

        [HttpPost]
        public IActionResult Delete(Siparis siparis)
        {
            _db.Siparisler.Remove(siparis);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
