using Microsoft.EntityFrameworkCore;
using SimakYolSupurge.Data;
using SimakYolSupurge.Models;

namespace SimakYolSupurge.Services
{
    public class TokenService
    {
        private readonly AppDbContext _context;

        public TokenService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateTokenAsync(User user)
        {
            var token = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
            user.Token = token;
            user.TokenExpireDate = DateTime.UtcNow.AddDays(7);
            
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return token;
        }
    }
}
