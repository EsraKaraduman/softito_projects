using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketBooking.Core.Entities;
using TicketBooking.Core.Repositories;
using TicketBooking.Service.Services;

namespace TicketBooking.Web.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventService _eventService;
    private readonly ITicketService _ticketService;

    public AdminController(IUnitOfWork unitOfWork, IEventService eventService, ITicketService ticketService)
    {
        _unitOfWork = unitOfWork;
        _eventService = eventService;
        _ticketService = ticketService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var eventsCount = (await _eventService.GetAllAsync()).Count();
        var categoriesCount = (await _unitOfWork.Categories.GetAllAsync()).Count();
        var tickets = await _ticketService.GetAllTicketsWithDetailsAsync();
        
        var totalSales = tickets.Sum(t => t.TotalPrice);
        var totalTicketsSold = tickets.Sum(t => t.Quantity);

        ViewBag.EventsCount = eventsCount;
        ViewBag.CategoriesCount = categoriesCount;
        ViewBag.TotalSales = totalSales;
        ViewBag.TotalTicketsSold = totalTicketsSold;

        var recentTickets = tickets.Take(5);

        return View(recentTickets);
    }

    [HttpGet]
    public async Task<IActionResult> Categories()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync();
        return View(categories);
    }

    [HttpGet]
    public IActionResult CreateCategory()
    {
        return View(new Category());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCategory(Category category)
    {
        if (ModelState.IsValid)
        {
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();
            TempData["SuccessMessage"] = "Kategori başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Categories));
        }
        return View(category);
    }

    [HttpGet]
    public async Task<IActionResult> EditCategory(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditCategory(Category category)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveChangesAsync();
            TempData["SuccessMessage"] = "Kategori başarıyla güncellendi.";
            return RedirectToAction(nameof(Categories));
        }
        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        var hasEvents = await _unitOfWork.Events.Where(e => e.CategoryId == id).AnyAsync();
        if (hasEvents)
        {
            TempData["ErrorMessage"] = "Bu kategoriye bağlı etkinlikler olduğu için kategori silinemez.";
            return RedirectToAction(nameof(Categories));
        }

        _unitOfWork.Categories.Delete(category);
        await _unitOfWork.SaveChangesAsync();
        TempData["SuccessMessage"] = "Kategori başarıyla silindi.";
        return RedirectToAction(nameof(Categories));
    }

    [HttpGet]
    public async Task<IActionResult> Events()
    {
        var events = await _unitOfWork.Events
            .Where(e => true)
            .Include(e => e.Category)
            .ToListAsync();
        return View(events);
    }

    [HttpGet]
    public async Task<IActionResult> CreateEvent()
    {
        var categories = await _unitOfWork.Categories.Where(c => c.IsActive).ToListAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");
        
        var @event = new Event
        {
            Date = DateTime.Today.AddDays(7),
            Capacity = 100
        };
        
        return View(@event);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateEvent(Event @event)
    {
        if (ModelState.IsValid)
        {
            @event.AvailableSeats = @event.Capacity;
            if (string.IsNullOrEmpty(@event.ImageUrl))
            {
                @event.ImageUrl = "/images/default-event.jpg";
            }
            await _eventService.AddAsync(@event);
            TempData["SuccessMessage"] = "Etkinlik başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Events));
        }

        var categories = await _unitOfWork.Categories.Where(c => c.IsActive).ToListAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", @event.CategoryId);
        return View(@event);
    }

    [HttpGet]
    public async Task<IActionResult> EditEvent(int id)
    {
        var @event = await _eventService.GetByIdAsync(id);
        if (@event == null)
        {
            return NotFound();
        }

        var categories = await _unitOfWork.Categories.Where(c => c.IsActive).ToListAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", @event.CategoryId);
        return View(@event);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditEvent(Event @event)
    {
        if (ModelState.IsValid)
        {
            var oldEvent = await _unitOfWork.Events.Where(e => e.Id == @event.Id).AsNoTracking().FirstOrDefaultAsync();
            if (oldEvent != null)
            {
                var seatsBooked = oldEvent.Capacity - oldEvent.AvailableSeats;
                @event.AvailableSeats = @event.Capacity - seatsBooked;
                if (@event.AvailableSeats < 0)
                {
                    ModelState.AddModelError(string.Empty, $"Yeni kapasite ({@event.Capacity}), satılmış bilet sayısından ({seatsBooked}) daha küçük olamaz.");
                    var categoriesList = await _unitOfWork.Categories.Where(c => c.IsActive).ToListAsync();
                    ViewBag.Categories = new SelectList(categoriesList, "Id", "Name", @event.CategoryId);
                    return View(@event);
                }
            }

            if (string.IsNullOrEmpty(@event.ImageUrl))
            {
                @event.ImageUrl = "/images/default-event.jpg";
            }

            _unitOfWork.Events.Update(@event);
            await _unitOfWork.SaveChangesAsync();
            
            TempData["SuccessMessage"] = "Etkinlik başarıyla güncellendi.";
            return RedirectToAction(nameof(Events));
        }

        var categories = await _unitOfWork.Categories.Where(c => c.IsActive).ToListAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", @event.CategoryId);
        return View(@event);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var @event = await _eventService.GetByIdAsync(id);
        if (@event == null)
        {
            return NotFound();
        }

        var hasTickets = await _unitOfWork.Tickets.Where(t => t.EventId == id).AnyAsync();
        if (hasTickets)
        {
            TempData["ErrorMessage"] = "Bu etkinliğe ait satılmış biletler bulunduğu için etkinlik silinemez.";
            return RedirectToAction(nameof(Events));
        }

        await _eventService.DeleteAsync(@event);
        TempData["SuccessMessage"] = "Etkinlik başarıyla silindi.";
        return RedirectToAction(nameof(Events));
    }
}
