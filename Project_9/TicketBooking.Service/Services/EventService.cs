using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketBooking.Core.Entities;
using TicketBooking.Core.Repositories;

namespace TicketBooking.Service.Services;

public class EventService : Service<Event>, IEventService
{
    public EventService(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.Events)
    {
    }

    public async Task<IEnumerable<Event>> SearchEventsAsync(string? query, int? categoryId)
    {
        var eventsQuery = _unitOfWork.Events.Where(e => true);

        if (!string.IsNullOrEmpty(query))
        {
            var q = query.ToLower();
            eventsQuery = eventsQuery.Where(e => e.Title.ToLower().Contains(q) || 
                                                 e.Description.ToLower().Contains(q) || 
                                                 e.Location.ToLower().Contains(q));
        }

        if (categoryId.HasValue && categoryId.Value > 0)
        {
            eventsQuery = eventsQuery.Where(e => e.CategoryId == categoryId.Value);
        }

        return await eventsQuery.Include(e => e.Category).OrderBy(e => e.Date).ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
    {
        return await _unitOfWork.Categories.Where(c => c.IsActive).ToListAsync();
    }
}
