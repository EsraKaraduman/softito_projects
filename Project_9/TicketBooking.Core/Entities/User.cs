using System;
using System.Collections.Generic;

namespace TicketBooking.Core.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
