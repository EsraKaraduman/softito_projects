using MarketMvcProject.Data;
using MarketMvcProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketMvcProject.Controllers
{
    public class UrunController : Controller
    {
        private readonly AppDbContext _db;

        public UrunController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(int kategoriId)
        {
            var kategoriler = _db.Kategoriler.ToList();
            ViewBag.Kategoriler = kategoriler;

            var urunler = _db.Urunler.Include(x => x.Kategori).AsQueryable();

            if (kategoriId != 0)
            {
                urunler = urunler.Where(x => x.KategoriId == kategoriId);
            }

            return View(urunler.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Kategoriler = _db.Kategoriler.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Urun urun)
        {
            _db.Urunler.Add(urun);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var urun = _db.Urunler.Find(id);
            ViewBag.Kategoriler = _db.Kategoriler.ToList();
            return View(urun);
        }

        [HttpPost]
        public IActionResult Edit(Urun urun)
        {
            var mevcut = _db.Urunler.Find(urun.UrunId);
            mevcut.UrunAdi = urun.UrunAdi;
            mevcut.Fiyat = urun.Fiyat;
            mevcut.KategoriId = urun.KategoriId;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var urun = _db.Urunler.Include(x => x.Kategori).FirstOrDefault(x => x.UrunId == id);
            return View(urun);
        }

        [HttpPost]
        public IActionResult Delete(Urun urun)
        {
            _db.Urunler.Remove(urun);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
