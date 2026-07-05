using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockManager.Data;
using System.Linq;
using System.Threading.Tasks;

namespace StockManager.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly StockDbContext _context;

        public ReportController(StockDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActivePage"] = "Reports";

            var products = await _context.Products.ToListAsync();
            var suppliers = await _context.Suppliers.ToListAsync();
            var warehouses = await _context.Warehouses.ToListAsync();

            ViewBag.TotalProducts = products.Count;
            ViewBag.TotalStockValue = products.Sum(p => p.Quantity * p.UnitPrice);
            ViewBag.AveragePrice = products.Any() ? products.Average(p => p.UnitPrice) : 0m;
            
            ViewBag.CategorySummary = products
                .GroupBy(p => p.Category)
                .Select(g => new {
                    Category = g.Key,
                    UniqueProducts = g.Count(),
                    TotalQuantity = g.Sum(p => p.Quantity),
                    TotalValue = g.Sum(p => p.Quantity * p.UnitPrice)
                }).ToList();

            ViewBag.ProductsList = products;
            ViewBag.SuppliersList = suppliers;
            ViewBag.WarehousesList = warehouses;

            return View();
        }
    }
}
