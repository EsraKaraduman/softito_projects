using MarketMvcProject.Data;
using MarketMvcProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace MarketMvcProject.Controllers
{
    public class MusteriController : Controller
    {
        private readonly AppDbContext _db;

        public MusteriController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(string searchString)
        {
            var result = _db.Musteriler.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                result = result.Where(x => x.AdSoyad.Contains(searchString));
            }

            return View(result.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Musteri musteri)
        {
            _db.Musteriler.Add(musteri);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var musteri = _db.Musteriler.Find(id);
            return View(musteri);
        }

        [HttpPost]
        public IActionResult Edit(Musteri musteri)
        {
            var mevcut = _db.Musteriler.Find(musteri.MusteriId);
            mevcut.AdSoyad = musteri.AdSoyad;
            mevcut.Email = musteri.Email;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var musteri = _db.Musteriler.Find(id);
            return View(musteri);
        }

        [HttpPost]
        public IActionResult Delete(Musteri musteri)
        {
            _db.Musteriler.Remove(musteri);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

