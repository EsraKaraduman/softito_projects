using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using SimakYolSupurge.Data;

namespace SimakYolSupurge.Services
{
    public class TokenAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
        {
            var authHeader = context.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                var token = authHeader.Substring(7).Trim();
                if (!string.IsNullOrEmpty(token))
                {
                    var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Token == token);
                    if (user != null && user.TokenExpireDate > DateTime.UtcNow)
                    {
                        var claims = new[]
                        {
                            new Claim(ClaimTypes.Name, user.Username),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Role, user.Role),
                            new Claim("UserId", user.Id.ToString())
                        };
                        var identity = new ClaimsIdentity(claims, "Token");
                        context.User = new ClaimsPrincipal(identity);
                    }
                }
            }

            await _next(context);
        }
    }
}
