using Microsoft.AspNetCore.Mvc;
using Dapper;
using HotelProject.API.Data;
using HotelProject.API.Models;
using System.Security.Cryptography;
using System.Text;
using System.Data;

namespace HotelProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DbConnectionProvider _dbProvider;

        public AuthController(DbConnectionProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            using var connection = _dbProvider.CreateConnection();
            user.PasswordHash = HashPassword(user.PasswordHash);
            user.Role = string.IsNullOrEmpty(user.Role) ? "Guest" : user.Role;

            var parameters = new DynamicParameters();
            parameters.Add("@Username", user.Username);
            parameters.Add("@Email", user.Email);
            parameters.Add("@PasswordHash", user.PasswordHash);
            parameters.Add("@Role", user.Role);

            try
            {
                await connection.ExecuteAsync("sp_RegisterUser", parameters, commandType: CommandType.StoredProcedure);
                return Ok(new { Message = "Registration successful" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            using var connection = _dbProvider.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@Email", request.Email);

            var user = await connection.QueryFirstOrDefaultAsync<User>(
                "sp_GetUserByEmail",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            if (user == null || user.PasswordHash != HashPassword(request.Password))
            {
                return Unauthorized(new { Message = "Invalid email or password" });
            }

            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            });
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var builder = new StringBuilder();
            foreach (var b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
