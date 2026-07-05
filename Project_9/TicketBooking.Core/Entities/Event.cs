using System;
using System.Collections.Generic;

namespace TicketBooking.Core.Entities;

public class Event : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Location { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Capacity { get; set; }
    public int AvailableSeats { get; set; }
    public string ImageUrl { get; set; } = string.Empty;

    public int CategoryId { get; set; }
    public virtual Category? Category { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
