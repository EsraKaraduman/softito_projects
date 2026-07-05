using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketBooking.Core.Entities;
using TicketBooking.Service.Services;
using TicketBooking.Web.Models;

namespace TicketBooking.Web.Controllers;

[Authorize]
public class TicketController : Controller
{
    private readonly ITicketService _ticketService;
    private readonly IEventService _eventService;

    public TicketController(ITicketService ticketService, IEventService eventService)
    {
        _ticketService = ticketService;
        _eventService = eventService;
    }

    [HttpGet]
    public async Task<IActionResult> Book(int id)
    {
        if (User.IsInRole("Admin"))
        {
            return RedirectToAction("AccessDenied", "Account");
        }

        var @event = await _eventService.GetByIdAsync(id);
        if (@event == null)
        {
            return NotFound();
        }

        var model = new BookTicketViewModel
        {
            EventId = @event.Id,
            EventTitle = @event.Title,
            EventLocation = @event.Location,
            EventDate = @event.Date,
            Price = @event.Price,
            AvailableSeats = @event.AvailableSeats
        };

        var rand = new Random();
        model.SeatNumber = $"{(char)rand.Next(65, 75)}-{rand.Next(1, 100)}";

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Book(BookTicketViewModel model)
    {
        if (User.IsInRole("Admin"))
        {
            return RedirectToAction("AccessDenied", "Account");
        }

        var @event = await _eventService.GetByIdAsync(model.EventId);
        if (@event == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            model.EventTitle = @event.Title;
            model.EventLocation = @event.Location;
            model.EventDate = @event.Date;
            model.Price = @event.Price;
            model.AvailableSeats = @event.AvailableSeats;
            return View(model);
        }

        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
        {
            return Challenge();
        }

        if (@event.AvailableSeats < model.Quantity)
        {
            ModelState.AddModelError(string.Empty, "Seçilen adet kadar boş koltuk bulunmamaktadır.");
            model.EventTitle = @event.Title;
            model.EventLocation = @event.Location;
            model.EventDate = @event.Date;
            model.Price = @event.Price;
            model.AvailableSeats = @event.AvailableSeats;
            return View(model);
        }

        var ticket = await _ticketService.BookTicketAsync(userId, model.EventId, model.Quantity, model.SeatNumber);
        if (ticket == null)
        {
            ModelState.AddModelError(string.Empty, "Bilet alımı sırasında bir hata oluştu.");
            model.EventTitle = @event.Title;
            model.EventLocation = @event.Location;
            model.EventDate = @event.Date;
            model.Price = @event.Price;
            model.AvailableSeats = @event.AvailableSeats;
            return View(model);
        }

        TempData["SuccessMessage"] = "Biletiniz başarıyla alındı!";
        return RedirectToAction(nameof(MyTickets));
    }

    [HttpGet]
    public async Task<IActionResult> MyTickets()
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
        {
            return Challenge();
        }

        var tickets = await _ticketService.GetUserTicketsAsync(userId);
        return View(tickets);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id)
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
        {
            return Challenge();
        }

        var ticket = await _ticketService.GetByIdAsync(id);
        if (ticket == null || ticket.UserId != userId)
        {
            return Unauthorized();
        }

        var @event = await _eventService.GetByIdAsync(ticket.EventId);
        if (@event != null && @event.Date < DateTime.Now.AddDays(1))
        {
            TempData["ErrorMessage"] = "Etkinliğe 24 saatten az süre kaldığı için bilet iptal edilemez.";
            return RedirectToAction(nameof(MyTickets));
        }

        var success = await _ticketService.CancelTicketAsync(id);
        if (success)
        {
            TempData["SuccessMessage"] = "Biletiniz başarıyla iptal edildi ve ücret iadesi tanımlandı.";
        }
        else
        {
            TempData["ErrorMessage"] = "Bilet iptal edilirken bir hata oluştu.";
        }

        return RedirectToAction(nameof(MyTickets));
    }

    [HttpGet]
    public async Task<IActionResult> Invoice(int id)
    {
        var ticket = await _ticketService.GetByIdAsync(id);
        if (ticket == null)
        {
            return NotFound();
        }

        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
        {
            return Challenge();
        }

        if (ticket.UserId != userId && role != "Admin")
        {
            return Unauthorized();
        }

        var ticketsWithDetails = await _ticketService.GetUserTicketsAsync(ticket.UserId);
        foreach (var t in ticketsWithDetails)
        {
            if (t.Id == ticket.Id)
            {
                return View(t);
            }
        }

        return View(ticket);
    }

    [HttpGet]
    public async Task<IActionResult> ExportToExcel()
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
        {
            return Challenge();
        }

        var tickets = await _ticketService.GetUserTicketsAsync(userId);
        
        var sb = new StringBuilder();
        sb.Append('\uFEFF');
        sb.AppendLine("Bilet ID;Etkinlik Adı;Tarih;Mekan;Koltuk No;Adet;Bilet Fiyatı;Toplam Ödenen");

        foreach (var t in tickets)
        {
            sb.AppendLine($"{t.Id};{t.Event?.Title};{t.Event?.Date:dd.MM.yyyy HH:mm};{t.Event?.Location};{t.SeatNumber};{t.Quantity};{t.Event?.Price:F2};{t.TotalPrice:F2}");
        }

        var fileBytes = Encoding.UTF8.GetBytes(sb.ToString());
        return File(fileBytes, "text/csv; charset=utf-8", $"Biletlerim_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ExportToExcelAll()
    {
        var tickets = await _ticketService.GetAllTicketsWithDetailsAsync();

        var sb = new StringBuilder();
        sb.Append('\uFEFF');
        sb.AppendLine("Bilet ID;Müşteri;E-posta;Etkinlik Adı;Tarih;Mekan;Koltuk No;Adet;Toplam Tutar;Alım Tarihi");

        foreach (var t in tickets)
        {
            sb.AppendLine($"{t.Id};{t.User?.Username};{t.User?.Email};{t.Event?.Title};{t.Event?.Date:dd.MM.yyyy HH:mm};{t.Event?.Location};{t.SeatNumber};{t.Quantity};{t.TotalPrice:F2};{t.BookingDate:dd.MM.yyyy HH:mm}");
        }

        var fileBytes = Encoding.UTF8.GetBytes(sb.ToString());
        return File(fileBytes, "text/csv; charset=utf-8", $"TumSatislar_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
    }
}
