using CikolataciMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CikolataciMVC.Controllers
{
    public class SatislarController : Controller
    {
        private readonly AppDbContext _context;

        public SatislarController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Admin")
                return RedirectToAction("Login", "Account");

            var satislar = _context.Satislar
                .Include(x => x.Urun)
                .Include(x => x.Kullanici);

            return View(await satislar.ToListAsync());
        }

        public IActionResult Create()
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Admin")
                return RedirectToAction("Login", "Account");

            ViewBag.Urunler = _context.Urunler.ToList();
            ViewBag.Kullanicilar = _context.Kullanicilar.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Satislar satis)
        {
            satis.Tarih = DateTime.Now;

            _context.Add(satis);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Admin")
                return RedirectToAction("Login", "Account");

            var satis = await _context.Satislar.FindAsync(id);
            if (satis == null)
                return NotFound();

            ViewBag.Urunler = _context.Urunler.ToList();
            ViewBag.Kullanicilar = _context.Kullanicilar.ToList();

            return View(satis);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Satislar satis)
        {
            if (id != satis.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(satis);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Urunler = _context.Urunler.ToList();
            ViewBag.Kullanicilar = _context.Kullanicilar.ToList();

            return View(satis);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Admin")
                return RedirectToAction("Login", "Account");

            var satis = await _context.Satislar
                .Include(x => x.Urun)
                .Include(x => x.Kullanici)
                .FirstOrDefaultAsync(x => x.Id == id);

            return View(satis);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var satis = await _context.Satislar.FindAsync(id);
            if (satis != null)
            {
                _context.Satislar.Remove(satis);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
