using System.Collections.Generic;
using System.Threading.Tasks;
using TicketBooking.Core.Entities;

namespace TicketBooking.Service.Services;

public interface ITicketService : IService<Ticket>
{
    Task<Ticket?> BookTicketAsync(int userId, int eventId, int quantity, string seatNumber);
    Task<IEnumerable<Ticket>> GetUserTicketsAsync(int userId);
    Task<IEnumerable<Ticket>> GetAllTicketsWithDetailsAsync();
    Task<bool> CancelTicketAsync(int ticketId);
}
