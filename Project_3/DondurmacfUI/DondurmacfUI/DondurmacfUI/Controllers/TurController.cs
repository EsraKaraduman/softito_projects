using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Dondurmacf.Data.Data;
using Dondurmacf.Model;

namespace DondurmacfUI.Controllers
{
    [Authorize]
    public class TurController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TurController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var query = _context.Turs
                .Include(t => t.Urun)
                .Include(t => t.EkleyenKullanici)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(t => t.Tur.Contains(searchString) || t.Urun.UrunAdi.Contains(searchString));
            }

            ViewBag.CurrentFilter = searchString;
            return View(await query.ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.UrunNo = new SelectList(await _context.Uruns.ToListAsync(), "UrunNo", "UrunAdi");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DondurmaTur model)
        {
            ModelState.Remove("Urun");
            ModelState.Remove("EkleyenKullanici");

            if (ModelState.IsValid)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    model.EkleyenKullaniciNo = userId;
                }

                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.UrunNo = new SelectList(await _context.Uruns.ToListAsync(), "UrunNo", "UrunAdi", model.UrunNo);
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var tur = await _context.Turs.FindAsync(id);
            if (tur == null) return NotFound();

            ViewBag.UrunNo = new SelectList(await _context.Uruns.ToListAsync(), "UrunNo", "UrunAdi", tur.UrunNo);
            return View(tur);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DondurmaTur model)
        {
            ModelState.Remove("Urun");
            ModelState.Remove("EkleyenKullanici");

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.Turs.AsNoTracking().FirstOrDefaultAsync(t => t.TurNo == model.TurNo);
                    if (existing == null) return NotFound();

                    model.EkleyenKullaniciNo = existing.EkleyenKullaniciNo;
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Turs.Any(e => e.TurNo == model.TurNo)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.UrunNo = new SelectList(await _context.Uruns.ToListAsync(), "UrunNo", "UrunAdi", model.UrunNo);
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var tur = await _context.Turs
                .Include(t => t.Urun)
                .Include(t => t.EkleyenKullanici)
                .FirstOrDefaultAsync(m => m.TurNo == id);

            if (tur == null) return NotFound();

            return View(tur);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tur = await _context.Turs.FindAsync(id);
            if (tur != null)
            {
                _context.Turs.Remove(tur);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
