using System;
using System.Threading.Tasks;
using TicketBooking.Core.Entities;
using TicketBooking.Core.Repositories;

namespace TicketBooking.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IGenericRepository<Category>? _categories;
    private IGenericRepository<Event>? _events;
    private IGenericRepository<User>? _users;
    private IGenericRepository<Ticket>? _tickets;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IGenericRepository<Category> Categories => 
        _categories ??= new GenericRepository<Category>(_context);

    public IGenericRepository<Event> Events => 
        _events ??= new GenericRepository<Event>(_context);

    public IGenericRepository<User> Users => 
        _users ??= new GenericRepository<User>(_context);

    public IGenericRepository<Ticket> Tickets => 
        _tickets ??= new GenericRepository<Ticket>(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
