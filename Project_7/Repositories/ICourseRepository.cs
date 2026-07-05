using System.Collections.Generic;
using System.Threading.Tasks;
using Project_7.Models;

namespace Project_7.Repositories
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course?> GetByIdAsync(int id);
        Task<int> AddAsync(Course course);
        Task<bool> UpdateAsync(Course course);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Course>> SearchAsync(string query);
    }
}
