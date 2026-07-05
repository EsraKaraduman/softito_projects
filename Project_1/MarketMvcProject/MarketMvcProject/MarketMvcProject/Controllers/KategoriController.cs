using Microsoft.AspNetCore.Mvc;
using MarketMvcProject.Data;
using MarketMvcProject.Models;
using System.Linq;

namespace MarketMvcProject.Controllers
{
    public class KategoriController : Controller
    {
        private readonly AppDbContext _context;

        public KategoriController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var kategoriler = _context.Kategoriler.ToList();
            return View(kategoriler);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Kategori model)
        {
            if (ModelState.IsValid)
            {
                _context.Kategoriler.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var kategori = _context.Kategoriler.Find(id);
            return View(kategori);
        }

        [HttpPost]
        public IActionResult Edit(Kategori model)
        {
            if (ModelState.IsValid)
            {
                _context.Kategoriler.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var kategori = _context.Kategoriler.Find(id);
            return View(kategori);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var kategori = _context.Kategoriler.Find(id);
            _context.Kategoriler.Remove(kategori);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
