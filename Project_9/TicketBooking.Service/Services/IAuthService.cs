using System.Threading.Tasks;
using TicketBooking.Core.Entities;

namespace TicketBooking.Service.Services;

public interface IAuthService
{
    Task<User?> RegisterAsync(string username, string email, string password);
    Task<User?> LoginAsync(string emailOrUsername, string password);
}
