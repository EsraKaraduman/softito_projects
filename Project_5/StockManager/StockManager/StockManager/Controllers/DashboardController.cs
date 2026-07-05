using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockManager.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StockManager.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly StockDbContext _context;

        public DashboardController(StockDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActivePage"] = "Dashboard";

            var products = await _context.Products.ToListAsync();
            var suppliers = await _context.Suppliers.ToListAsync();
            var warehouses = await _context.Warehouses.ToListAsync();

            ViewBag.TotalProducts = products.Count;
            ViewBag.TotalStock = products.Sum(p => p.Quantity);
            ViewBag.TotalSuppliers = suppliers.Count;
            ViewBag.TotalWarehouseCapacity = warehouses.Sum(w => w.Capacity);
            ViewBag.LowStockCount = products.Count(p => p.Quantity < 15);

            ViewBag.LowStockProducts = products.Where(p => p.Quantity < 15).OrderBy(p => p.Quantity).Take(5).ToList();

            var categoryStats = products
                .GroupBy(p => p.Category)
                .Select(g => new { Category = g.Key, Count = g.Count(), Stock = g.Sum(p => p.Quantity) })
                .ToList();

            ViewBag.CategoryLabels = categoryStats.Select(c => c.Category).ToArray();
            ViewBag.CategoryStocks = categoryStats.Select(c => c.Stock).ToArray();

            var warehouseStats = warehouses
                .Select(w => new { w.Name, w.Capacity })
                .ToList();

            ViewBag.WarehouseLabels = warehouseStats.Select(w => w.Name).ToArray();
            ViewBag.WarehouseCapacities = warehouseStats.Select(w => w.Capacity).ToArray();

            return View();
        }
    }
}
