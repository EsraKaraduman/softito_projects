using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketBooking.Core.Repositories;
using TicketBooking.Service.Services;

namespace TicketBooking.Service.Services;

public class Service<T> : IService<T> where T : class
{
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IGenericRepository<T> _repository;

    public Service(IUnitOfWork unitOfWork, IGenericRepository<T> repository)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> predicate)
    {
        return await _repository.Where(predicate).ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _repository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _repository.Delete(entity);
        await _unitOfWork.SaveChangesAsync();
    }
}
