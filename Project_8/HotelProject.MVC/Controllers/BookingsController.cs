using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using HotelProject.MVC.Models;

namespace HotelProject.MVC.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public BookingsController(IHttpClientFactory clientFactory)
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
                response = await client.GetAsync("bookings");
            }
            else
            {
                response = await client.GetAsync($"bookings/search?query={Uri.EscapeDataString(query)}");
            }

            var bookings = new List<Booking>();
            if (response.IsSuccessStatusCode)
            {
                bookings = await response.Content.ReadFromJsonAsync<List<Booking>>() ?? new List<Booking>();
            }

            ViewBag.Query = query;
            return View(bookings);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var client = _clientFactory.CreateClient("HotelApi");
            var response = await client.GetAsync("rooms");
            var rooms = new List<Room>();

            if (response.IsSuccessStatusCode)
            {
                var allRooms = await response.Content.ReadFromJsonAsync<List<Room>>() ?? new List<Room>();
                rooms = allRooms.Where(r => r.IsAvailable).ToList();
            }

            ViewBag.Rooms = rooms;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Booking booking)
        {
            var client = _clientFactory.CreateClient("HotelApi");
            var roomResponse = await client.GetAsync($"rooms/{booking.RoomId}");

            if (roomResponse.IsSuccessStatusCode)
            {
                var room = await roomResponse.Content.ReadFromJsonAsync<Room>();
                if (room != null)
                {
                    int days = (booking.CheckOutDate - booking.CheckInDate).Days;
                    if (days <= 0) days = 1;
                    booking.TotalPrice = room.PricePerNight * days;
                }
            }

            var response = await client.PostAsJsonAsync("bookings", booking);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            var roomsResponse = await client.GetAsync("rooms");
            var rooms = new List<Room>();
            if (roomsResponse.IsSuccessStatusCode)
            {
                var allRooms = await roomsResponse.Content.ReadFromJsonAsync<List<Room>>() ?? new List<Room>();
                rooms = allRooms.Where(r => r.IsAvailable).ToList();
            }
            ViewBag.Rooms = rooms;
            ViewBag.Error = "Rezervasyon oluşturulamadı.";
            return View(booking);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var client = _clientFactory.CreateClient("HotelApi");
            var response = await client.GetAsync($"bookings/{id}");

            if (response.IsSuccessStatusCode)
            {
                var booking = await response.Content.ReadFromJsonAsync<Booking>();
                if (booking != null)
                {
                    var roomsResponse = await client.GetAsync("rooms");
                    var rooms = new List<Room>();
                    if (roomsResponse.IsSuccessStatusCode)
                    {
                        var allRooms = await roomsResponse.Content.ReadFromJsonAsync<List<Room>>() ?? new List<Room>();
                        rooms = allRooms.Where(r => r.IsAvailable || r.Id == booking.RoomId).ToList();
                    }
                    ViewBag.Rooms = rooms;
                    return View(booking);
                }
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            var client = _clientFactory.CreateClient("HotelApi");
            var roomResponse = await client.GetAsync($"rooms/{booking.RoomId}");

            if (roomResponse.IsSuccessStatusCode)
            {
                var room = await roomResponse.Content.ReadFromJsonAsync<Room>();
                if (room != null)
                {
                    int days = (booking.CheckOutDate - booking.CheckInDate).Days;
                    if (days <= 0) days = 1;
                    booking.TotalPrice = room.PricePerNight * days;
                }
            }

            var response = await client.PutAsJsonAsync($"bookings/{id}", booking);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            var roomsResponse = await client.GetAsync("rooms");
            var rooms = new List<Room>();
            if (roomsResponse.IsSuccessStatusCode)
            {
                var allRooms = await roomsResponse.Content.ReadFromJsonAsync<List<Room>>() ?? new List<Room>();
                rooms = allRooms.Where(r => r.IsAvailable || r.Id == booking.RoomId).ToList();
            }
            ViewBag.Rooms = rooms;
            ViewBag.Error = "Rezervasyon güncellenemedi.";
            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _clientFactory.CreateClient("HotelApi");
            await client.DeleteAsync($"bookings/{id}");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ExportExcel()
        {
            var client = _clientFactory.CreateClient("HotelApi");
            var response = await client.GetAsync("bookings");
            var bookings = new List<Booking>();

            if (response.IsSuccessStatusCode)
            {
                bookings = await response.Content.ReadFromJsonAsync<List<Booking>>() ?? new List<Booking>();
            }

            var builder = new StringBuilder();
            builder.AppendLine("Id,Guest Name,Email,Phone,Room,Type,CheckIn,CheckOut,Total,Status");

            foreach (var b in bookings)
            {
                builder.AppendLine($"{b.Id},{b.GuestName},{b.GuestEmail},{b.GuestPhone},{b.RoomNumber},{b.RoomType},{b.CheckInDate:yyyy-MM-dd},{b.CheckOutDate:yyyy-MM-dd},{b.TotalPrice},{b.BookingStatus}");
            }

            var data = Encoding.UTF8.GetBytes(builder.ToString());
            var result = Encoding.UTF8.GetPreamble().Concat(data).ToArray();

            return File(result, "text/csv; charset=utf-8", "bookings_export.csv");
        }

        [HttpGet]
        public async Task<IActionResult> PrintReport()
        {
            var client = _clientFactory.CreateClient("HotelApi");
            var response = await client.GetAsync("bookings");
            var bookings = new List<Booking>();

            if (response.IsSuccessStatusCode)
            {
                bookings = await response.Content.ReadFromJsonAsync<List<Booking>>() ?? new List<Booking>();
            }

            return View(bookings);
        }
    }
}
