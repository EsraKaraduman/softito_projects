using System.Threading.Tasks;
using Project_7.Models;

namespace Project_7.Repositories
{
    public interface IUserRepository
    {
        Task<int> RegisterAsync(User user);
        Task<User?> GetByEmailAsync(string email);
    }
}
