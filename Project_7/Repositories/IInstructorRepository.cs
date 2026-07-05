using System.Collections.Generic;
using System.Threading.Tasks;
using Project_7.Models;

namespace Project_7.Repositories
{
    public interface IInstructorRepository
    {
        Task<IEnumerable<Instructor>> GetAllAsync();
        Task<Instructor?> GetByIdAsync(int id);
        Task<int> AddAsync(Instructor instructor);
        Task<bool> UpdateAsync(Instructor instructor);
        Task<bool> DeleteAsync(int id);
    }
}
