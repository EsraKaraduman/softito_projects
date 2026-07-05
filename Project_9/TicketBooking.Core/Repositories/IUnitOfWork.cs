using System;
using System.Threading.Tasks;
using TicketBooking.Core.Entities;

namespace TicketBooking.Core.Repositories;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Category> Categories { get; }
    IGenericRepository<Event> Events { get; }
    IGenericRepository<User> Users { get; }
    IGenericRepository<Ticket> Tickets { get; }
    Task<int> SaveChangesAsync();
}
