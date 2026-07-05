using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockManager.Data;
using StockManager.Models;
using System.Linq;
using System.Threading.Tasks;

namespace StockManager.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SupplierController : Controller
    {
        private readonly StockDbContext _context;

        public SupplierController(StockDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["ActivePage"] = "Suppliers";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetList(string? search)
        {
            IQueryable<Supplier> query = _context.Suppliers;

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                query = query.Where(s => 
                    s.Name.ToLower().Contains(term) || 
                    s.ContactPerson.ToLower().Contains(term) || 
                    s.Email.ToLower().Contains(term) || 
                    s.Phone.ToLower().Contains(term) || 
                    s.Address.ToLower().Contains(term));
            }

            var list = await query.OrderBy(s => s.Name).ToListAsync();
            return Json(list);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null) return NotFound(new { message = "Tedarikçi bulunamadı!" });
            return Json(supplier);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] Supplier model)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return Json(new { success = false, message = errors });
            }

            if (model.Id == 0)
            {
                await _context.Suppliers.AddAsync(model);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Tedarikçi başarıyla eklendi." });
            }
            else
            {
                var existing = await _context.Suppliers.FindAsync(model.Id);
                if (existing == null) return Json(new { success = false, message = "Güncellenecek tedarikçi bulunamadı!" });

                existing.Name = model.Name;
                existing.ContactPerson = model.ContactPerson;
                existing.Email = model.Email;
                existing.Phone = model.Phone;
                existing.Address = model.Address;

                _context.Suppliers.Update(existing);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Tedarikçi başarıyla güncellendi." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _context.Suppliers.FindAsync(id);
            if (existing == null) return Json(new { success = false, message = "Silinecek tedarikçi bulunamadı!" });

            _context.Suppliers.Remove(existing);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Tedarikçi başarıyla silindi." });
        }
    }
}
