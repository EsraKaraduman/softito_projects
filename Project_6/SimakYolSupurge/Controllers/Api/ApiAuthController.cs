using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimakYolSupurge.Data;
using SimakYolSupurge.Helpers;
using SimakYolSupurge.Models;
using SimakYolSupurge.Services;

namespace SimakYolSupurge.Controllers.Api
{
    [ApiController]
    [Route("api/auth")]
    public class ApiAuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;

        public ApiAuthController(AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (await _context.Users.AnyAsync(u => u.Username == model.Username))
                return BadRequest("Bu kullanıcı adı zaten alınmış.");

            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                return BadRequest("Bu e-posta adresi zaten kullanımda.");

            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = HashHelper.HashPassword(model.Password),
                Role = model.Role ?? "User",
                CreatedDate = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Kayıt işlemi başarıyla tamamlandı." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
            if (user == null || !HashHelper.VerifyPassword(model.Password, user.PasswordHash))
                return Unauthorized("Kullanıcı adı veya şifre hatalı.");

            var token = await _tokenService.GenerateTokenAsync(user);

            return Ok(new AuthResponseDto
            {
                Token = token,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            });
        }
    }

    public class RegisterDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Role { get; set; }
    }

    public class LoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
