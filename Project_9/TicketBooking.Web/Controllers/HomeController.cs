using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketBooking.Service.Services;
using TicketBooking.Web.Models;

namespace TicketBooking.Web.Controllers;

public class HomeController : Controller
{
    private readonly IEventService _eventService;

    public HomeController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? q, int? c)
    {
        var events = await _eventService.SearchEventsAsync(q, c);
        var categories = await _eventService.GetActiveCategoriesAsync();

        var model = new HomeViewModel
        {
            Events = events,
            Categories = categories,
            SearchQuery = q,
            SelectedCategoryId = c
        };

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
