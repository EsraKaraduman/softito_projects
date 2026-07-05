using System;

namespace TicketBooking.Core.Entities;

public class Ticket : BaseEntity
{
    public int UserId { get; set; }
    public virtual User? User { get; set; }

    public int EventId { get; set; }
    public virtual Event? Event { get; set; }

    public string SeatNumber { get; set; } = string.Empty;
    public DateTime BookingDate { get; set; } = DateTime.UtcNow;
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}
