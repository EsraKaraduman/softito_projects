using CikolataciMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CikolataciMVC.Controllers
{
    public class KullanicilarController : Controller
    {
        private readonly AppDbContext _context;

        public KullanicilarController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Admin")
                return RedirectToAction("Login", "Account");

            return View(await _context.Kullanicilar.ToListAsync());
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
        public async Task<IActionResult> Create(Kullanicilar k)
        {
            if (ModelState.IsValid)
            {
                _context.Add(k);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(k);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Admin")
                return RedirectToAction("Login", "Account");

            var user = await _context.Kullanicilar.FindAsync(id);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Kullanicilar k)
        {
            if (id != k.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(k);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(k);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Admin")
                return RedirectToAction("Login", "Account");

            var user = await _context.Kullanicilar.FindAsync(id);
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Kullanicilar.FindAsync(id);
            if (user != null)
            {
                _context.Kullanicilar.Remove(user);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
