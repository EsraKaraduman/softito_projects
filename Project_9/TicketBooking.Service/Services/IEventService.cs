using System.Collections.Generic;
using System.Threading.Tasks;
using TicketBooking.Core.Entities;

namespace TicketBooking.Service.Services;

public interface IEventService : IService<Event>
{
    Task<IEnumerable<Event>> SearchEventsAsync(string? query, int? categoryId);
    Task<IEnumerable<Category>> GetActiveCategoriesAsync();
}
