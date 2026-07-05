using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SimakYolSupurge.Models;
using SimakYolSupurge.Services;

namespace SimakYolSupurge.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class MaintenancesController : Controller
    {
        private readonly ApiService _apiService;

        public MaintenancesController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index(string? search)
        {
            ViewBag.Search = search;
            var endpoint = string.IsNullOrEmpty(search) ? "maintenances" : $"maintenances?search={Uri.EscapeDataString(search)}";
            var maintenances = await _apiService.GetAsync<List<Maintenance>>(endpoint);
            return View(maintenances ?? new List<Maintenance>());
        }

        public async Task<IActionResult> Details(int id)
        {
            var maintenance = await _apiService.GetAsync<Maintenance>($"maintenances/{id}");
            if (maintenance == null)
            {
                return NotFound();
            }
            return View(maintenance);
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Maintenance maintenance)
        {
            if (ModelState.IsValid)
            {
                var response = await _apiService.PostAsync("maintenances", maintenance);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Bakım kaydı başarıyla eklendi.";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Bakım kaydı eklenirken hata oluştu.");
            }
            return View(maintenance);
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Edit(int id)
        {
            var maintenance = await _apiService.GetAsync<Maintenance>($"maintenances/{id}");
            if (maintenance == null)
            {
                return NotFound();
            }
            return View(maintenance);
        }

        [HttpPost]
        [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Maintenance maintenance)
        {
            if (id != maintenance.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var response = await _apiService.PutAsync($"maintenances/{id}", maintenance);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Bakım kaydı başarıyla güncellendi.";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Bakım kaydı güncellenirken hata oluştu.");
            }
            return View(maintenance);
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(int id)
        {
            var maintenance = await _apiService.GetAsync<Maintenance>($"maintenances/{id}");
            if (maintenance == null)
            {
                return NotFound();
            }
            return View(maintenance);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _apiService.DeleteAsync($"maintenances/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Bakım kaydı başarıyla silindi.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Bakım kaydı silinirken hata oluştu.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ExportExcel()
        {
            var fileBytes = await _apiService.GetFileAsync("maintenances/export/excel");
            if (fileBytes == null)
            {
                return BadRequest("Dosya indirilemedi.");
            }
            return File(fileBytes, "text/csv", "Bakimlar.csv");
        }
    }
}
