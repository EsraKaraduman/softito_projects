using System.Collections.Generic;
using TicketBooking.Core.Entities;

namespace TicketBooking.Web.Models;

public class HomeViewModel
{
    public IEnumerable<Event> Events { get; set; } = new List<Event>();
    public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    public string? SearchQuery { get; set; }
    public int? SelectedCategoryId { get; set; }
}
