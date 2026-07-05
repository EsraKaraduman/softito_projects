using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SimakYolSupurge.Models;
using SimakYolSupurge.Services;

namespace SimakYolSupurge.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class VehiclesController : Controller
    {
        private readonly ApiService _apiService;

        public VehiclesController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string? search)
        {
            ViewBag.Search = search;
            var endpoint = string.IsNullOrEmpty(search) ? "vehicles" : $"vehicles?search={Uri.EscapeDataString(search)}";
            var vehicles = await _apiService.GetAsync<List<Vehicle>>(endpoint);
            return View(vehicles ?? new List<Vehicle>());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var vehicle = await _apiService.GetAsync<Vehicle>($"vehicles/{id}");
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                var response = await _apiService.PostAsync("vehicles", vehicle);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Araç başarıyla eklendi.";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Araç eklenirken bir hata oluştu.");
            }
            return View(vehicle);
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Edit(int id)
        {
            var vehicle = await _apiService.GetAsync<Vehicle>($"vehicles/{id}");
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        [HttpPost]
        [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var response = await _apiService.PutAsync($"vehicles/{id}", vehicle);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Araç başarıyla güncellendi.";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Araç güncellenirken bir hata oluştu.");
            }
            return View(vehicle);
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(int id)
        {
            var vehicle = await _apiService.GetAsync<Vehicle>($"vehicles/{id}");
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _apiService.DeleteAsync($"vehicles/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Araç başarıyla silindi.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Araç silinirken hata oluştu.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ExportExcel()
        {
            var fileBytes = await _apiService.GetFileAsync("vehicles/export/excel");
            if (fileBytes == null)
            {
                return BadRequest("Dosya indirilemedi.");
            }
            return File(fileBytes, "text/csv", "Araclar.csv");
        }
    }
}
