using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dondurmacf.Data.Data;
using Dondurmacf.Model;

namespace DondurmacfUI.Controllers
{
    [Authorize]
    public class UrunController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UrunController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Uruns.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Urun urun)
        {
            if (ModelState.IsValid)
            {
                _context.Add(urun);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(urun);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var urun = await _context.Uruns.FindAsync(id);
            if (urun == null) return NotFound();
            return View(urun);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Urun urun)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(urun);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Uruns.Any(e => e.UrunNo == urun.UrunNo)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(urun);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var urun = await _context.Uruns.FirstOrDefaultAsync(m => m.UrunNo == id);
            if (urun == null) return NotFound();
            return View(urun);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var urun = await _context.Uruns.FindAsync(id);
            if (urun != null)
            {
                _context.Uruns.Remove(urun);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
