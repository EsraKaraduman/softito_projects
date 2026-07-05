using CikolataciMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CikolataciMVC.Controllers
{
    public class KategorilerController : Controller
    {
        private readonly AppDbContext _context;

        public KategorilerController(AppDbContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Admin")
                return RedirectToAction("Login", "Account");

            var list = await _context.Kategoriler.ToListAsync();
            return View(list);
        }

        
        public IActionResult Create()
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Admin")
                return RedirectToAction("Login", "Account");

            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Kategoriler kategori)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kategori);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kategori);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Admin")
                return RedirectToAction("Login", "Account");

            if (id == null)
                return NotFound();

            var kategori = await _context.Kategoriler.FindAsync(id);
            if (kategori == null)
                return NotFound();

            return View(kategori);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Kategoriler kategori)
        {
            if (id != kategori.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(kategori);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kategori);
        }

        
        public async Task<IActionResult> Delete(int? id)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Admin")
                return RedirectToAction("Login", "Account");

            if (id == null)
                return NotFound();

            var kategori = await _context.Kategoriler
                .FirstOrDefaultAsync(m => m.Id == id);

            if (kategori == null)
                return NotFound();

            return View(kategori);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kategori = await _context.Kategoriler.FindAsync(id);
            if (kategori != null)
            {
                _context.Kategoriler.Remove(kategori);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        

    }
}
