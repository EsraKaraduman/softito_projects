using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelProject.MVC.Models;

namespace HotelProject.MVC.Controllers
{
    public class RoomsController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public RoomsController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string query)
        {
            var client = _clientFactory.CreateClient("HotelApi");
            HttpResponseMessage response;

            if (string.IsNullOrWhiteSpace(query))
            {
                response = await client.GetAsync("rooms");
            }
            else
            {
                response = await client.GetAsync($"rooms/search?query={Uri.EscapeDataString(query)}");
            }

            var rooms = new List<Room>();
            if (response.IsSuccessStatusCode)
            {
                rooms = await response.Content.ReadFromJsonAsync<List<Room>>() ?? new List<Room>();
            }

            ViewBag.Query = query;
            return View(rooms);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Room room)
        {
            var client = _clientFactory.CreateClient("HotelApi");
            var response = await client.PostAsJsonAsync("rooms", room);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Error = "Oda eklenirken bir hata oluştu.";
            return View(room);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var client = _clientFactory.CreateClient("HotelApi");
            var response = await client.GetAsync($"rooms/{id}");

            if (response.IsSuccessStatusCode)
            {
                var room = await response.Content.ReadFromJsonAsync<Room>();
                if (room != null)
                {
                    return View(room);
                }
            }

            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Room room)
        {
            var client = _clientFactory.CreateClient("HotelApi");
            var response = await client.PutAsJsonAsync($"rooms/{id}", room);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Error = "Oda güncellenirken bir hata oluştu.";
            return View(room);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _clientFactory.CreateClient("HotelApi");
            await client.DeleteAsync($"rooms/{id}");
            return RedirectToAction("Index");
        }
    }
}
