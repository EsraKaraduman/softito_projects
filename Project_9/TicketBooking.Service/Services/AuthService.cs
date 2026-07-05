using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketBooking.Core.Entities;
using TicketBooking.Core.Repositories;

namespace TicketBooking.Service.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<User?> RegisterAsync(string username, string email, string password)
    {
        var existingUser = await _unitOfWork.Users
            .Where(u => u.Username.ToLower() == username.ToLower() || u.Email.ToLower() == email.ToLower())
            .FirstOrDefaultAsync();

        if (existingUser != null)
        {
            return null;
        }

        var passwordHash = HashPassword(password);

        var newUser = new User
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                Role = "User",
                CreatedDate = DateTime.UtcNow
            };

        await _unitOfWork.Users.AddAsync(newUser);
        await _unitOfWork.SaveChangesAsync();

        return newUser;
    }

    public async Task<User?> LoginAsync(string emailOrUsername, string password)
    {
        var passwordHash = HashPassword(password);

        var user = await _unitOfWork.Users
            .Where(u => (u.Email.ToLower() == emailOrUsername.ToLower() || u.Username.ToLower() == emailOrUsername.ToLower()) 
                         && u.PasswordHash == passwordHash)
            .FirstOrDefaultAsync();

        return user;
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        var sb = new StringBuilder();
        foreach (var b in bytes)
        {
            sb.Append(b.ToString("x2"));
        }
        return sb.ToString();
    }
}
