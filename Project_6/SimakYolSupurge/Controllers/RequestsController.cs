using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SimakYolSupurge.Models;
using SimakYolSupurge.Services;

namespace SimakYolSupurge.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class RequestsController : Controller
    {
        private readonly ApiService _apiService;

        public RequestsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Index(string? search)
        {
            ViewBag.Search = search;
            var endpoint = string.IsNullOrEmpty(search) ? "requests" : $"requests?search={Uri.EscapeDataString(search)}";
            var requests = await _apiService.GetAsync<List<Request>>(endpoint);
            return View(requests ?? new List<Request>());
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Details(int id)
        {
            var request = await _apiService.GetAsync<Request>($"requests/{id}");
            if (request == null)
            {
                return NotFound();
            }
            return View(request);
        }

        public IActionResult Create(string? vehicleModel)
        {
            var request = new Request
            {
                VehicleModel = vehicleModel ?? string.Empty
            };
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Request request)
        {
            if (ModelState.IsValid)
            {
                var response = await _apiService.PostAsync("requests", request);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Talebiniz başarıyla alınmıştır. En kısa sürede sizinle iletişime geçilecektir.";
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Talep iletilirken hata oluştu.");
            }
            return View(request);
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Edit(int id)
        {
            var request = await _apiService.GetAsync<Request>($"requests/{id}");
            if (request == null)
            {
                return NotFound();
            }
            return View(request);
        }

        [HttpPost]
        [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Request request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var response = await _apiService.PutAsync($"requests/{id}", request);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Talep başarıyla güncellendi.";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Talep güncellenirken hata oluştu.");
            }
            return View(request);
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(int id)
        {
            var request = await _apiService.GetAsync<Request>($"requests/{id}");
            if (request == null)
            {
                return NotFound();
            }
            return View(request);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _apiService.DeleteAsync($"requests/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Talep başarıyla silindi.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Talep silinirken hata oluştu.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ExportExcel()
        {
            var fileBytes = await _apiService.GetFileAsync("requests/export/excel");
            if (fileBytes == null)
            {
                return BadRequest("Dosya indirilemedi.");
            }
            return File(fileBytes, "text/csv", "Talepler.csv");
        }
    }
}
