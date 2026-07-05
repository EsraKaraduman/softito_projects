using CikolataciMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CikolataciMVC.Controllers
{
    public class UrunlerController : Controller
    {
        private readonly AppDbContext _context;

        public UrunlerController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? kategoriId)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Admin")
                return RedirectToAction("Login", "Account");

            ViewBag.Kategoriler = await _context.Kategoriler.ToListAsync();

            var urunler = _context.Urunler.AsQueryable();

            if (kategoriId != null)
            {
                urunler = urunler.Where(x => x.KategoriId == kategoriId);
            }

            urunler = urunler.Include(x => x.Kategori);

            return View(await urunler.ToListAsync());
        }

        public IActionResult Create()
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Admin")
                return RedirectToAction("Login", "Account");

            ViewBag.Kategoriler = _context.Kategoriler.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Urunler urun)
        {
            if (ModelState.IsValid)
            {
                _context.Add(urun);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Kategoriler = _context.Kategoriler.ToList();
            return View(urun);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Admin")
                return RedirectToAction("Login", "Account");

            var urun = await _context.Urunler.FindAsync(id);
            if (urun == null)
                return NotFound();

            ViewBag.Kategoriler = _context.Kategoriler.ToList();
            return View(urun);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Urunler urun)
        {
            if (id != urun.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(urun);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Kategoriler = _context.Kategoriler.ToList();
            return View(urun);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Admin")
                return RedirectToAction("Login", "Account");

            var urun = await _context.Urunler
                .Include(x => x.Kategori)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (urun == null)
                return NotFound();

            return View(urun);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var urun = await _context.Urunler.FindAsync(id);
            if (urun != null)
            {
                _context.Urunler.Remove(urun);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

