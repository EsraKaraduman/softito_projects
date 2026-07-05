using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketBooking.Core.Entities;
using TicketBooking.Core.Repositories;

namespace TicketBooking.Service.Services;

public class TicketService : Service<Ticket>, ITicketService
{
    public TicketService(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.Tickets)
    {
    }

    public async Task<Ticket?> BookTicketAsync(int userId, int eventId, int quantity, string seatNumber)
    {
        var @event = await _unitOfWork.Events.GetByIdAsync(eventId);
        if (@event == null || @event.AvailableSeats < quantity)
        {
            return null;
        }

        var ticket = new Ticket
        {
            UserId = userId,
            EventId = eventId,
            Quantity = quantity,
            SeatNumber = seatNumber,
            BookingDate = DateTime.UtcNow,
            TotalPrice = @event.Price * quantity
        };

        @event.AvailableSeats -= quantity;

        _unitOfWork.Events.Update(@event);
        await _unitOfWork.Tickets.AddAsync(ticket);
        await _unitOfWork.SaveChangesAsync();

        return ticket;
    }

    public async Task<IEnumerable<Ticket>> GetUserTicketsAsync(int userId)
    {
        return await _unitOfWork.Tickets
            .Where(t => t.UserId == userId)
            .Include(t => t.Event)
            .OrderByDescending(t => t.BookingDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Ticket>> GetAllTicketsWithDetailsAsync()
    {
        return await _unitOfWork.Tickets
            .Where(t => true)
            .Include(t => t.User)
            .Include(t => t.Event)
            .OrderByDescending(t => t.BookingDate)
            .ToListAsync();
    }

    public async Task<bool> CancelTicketAsync(int ticketId)
    {
        var ticket = await _unitOfWork.Tickets.GetByIdAsync(ticketId);
        if (ticket == null)
        {
            return false;
        }

        var @event = await _unitOfWork.Events.GetByIdAsync(ticket.EventId);
        if (@event != null)
        {
            @event.AvailableSeats += ticket.Quantity;
            _unitOfWork.Events.Update(@event);
        }

        _unitOfWork.Tickets.Delete(ticket);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
