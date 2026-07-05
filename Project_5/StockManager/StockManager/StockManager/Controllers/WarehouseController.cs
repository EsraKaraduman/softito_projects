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
    public class WarehouseController : Controller
    {
        private readonly StockDbContext _context;

        public WarehouseController(StockDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["ActivePage"] = "Warehouses";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetList(string? search)
        {
            IQueryable<Warehouse> query = _context.Warehouses;

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                query = query.Where(w => 
                    w.Name.ToLower().Contains(term) || 
                    w.Location.ToLower().Contains(term) || 
                    w.ManagerName.ToLower().Contains(term));
            }

            var list = await query.OrderBy(w => w.Name).ToListAsync();
            return Json(list);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse == null) return NotFound(new { message = "Depo bulunamadı!" });
            return Json(warehouse);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] Warehouse model)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return Json(new { success = false, message = errors });
            }

            if (model.Capacity <= 0)
            {
                return Json(new { success = false, message = "Kapasite sıfırdan büyük olmalıdır!" });
            }

            if (model.Id == 0)
            {
                await _context.Warehouses.AddAsync(model);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Depo başarıyla eklendi." });
            }
            else
            {
                var existing = await _context.Warehouses.FindAsync(model.Id);
                if (existing == null) return Json(new { success = false, message = "Güncellenecek depo bulunamadı!" });

                existing.Name = model.Name;
                existing.Location = model.Location;
                existing.Capacity = model.Capacity;
                existing.ManagerName = model.ManagerName;

                _context.Warehouses.Update(existing);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Depo başarıyla güncellendi." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _context.Warehouses.FindAsync(id);
            if (existing == null) return Json(new { success = false, message = "Silinecek depo bulunamadı!" });

            _context.Warehouses.Remove(existing);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Depo başarıyla silindi." });
        }
    }
}
