using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockManager.Data;
using StockManager.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StockManager.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly StockDbContext _context;

        public ProductController(StockDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["ActivePage"] = "Products";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetList(string? search)
        {
            IQueryable<Product> query = _context.Products;

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                query = query.Where(p => 
                    p.Name.ToLower().Contains(term) || 
                    p.Sku.ToLower().Contains(term) || 
                    p.Category.ToLower().Contains(term));
            }

            var list = await query.OrderBy(p => p.Name).ToListAsync();
            return Json(list);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound(new { message = "Ürün bulunamadı!" });
            return Json(product);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] Product model)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return Json(new { success = false, message = errors });
            }

            if (model.Quantity < 0 || model.UnitPrice < 0)
            {
                return Json(new { success = false, message = "Miktar veya fiyat sıfırdan küçük olamaz!" });
            }

            if (model.Id == 0)
            {
                var skuExists = await _context.Products.AnyAsync(p => p.Sku.ToLower() == model.Sku.ToLower());
                if (skuExists)
                {
                    return Json(new { success = false, message = "Bu barkod / SKU kodu zaten sistemde tanımlı!" });
                }

                model.LastUpdated = DateTime.Now;
                await _context.Products.AddAsync(model);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Ürün başarıyla eklendi." });
            }
            else
            {
                var existing = await _context.Products.FindAsync(model.Id);
                if (existing == null) return Json(new { success = false, message = "Güncellenecek ürün bulunamadı!" });

                existing.Name = model.Name;
                existing.Sku = model.Sku;
                existing.Category = model.Category;
                existing.Quantity = model.Quantity;
                existing.UnitPrice = model.UnitPrice;
                existing.LastUpdated = DateTime.Now;

                _context.Products.Update(existing);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Ürün başarıyla güncellendi." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _context.Products.FindAsync(id);
            if (existing == null) return Json(new { success = false, message = "Silinecek ürün bulunamadı!" });

            _context.Products.Remove(existing);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Ürün başarıyla silindi." });
        }
    }
}
