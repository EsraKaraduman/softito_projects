using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Project_7.Models;

namespace Project_7.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<int> RegisterAsync(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_RegisterUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FullName", user.FullName);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    command.Parameters.AddWithValue("@Role", user.Role);

                    await connection.OpenAsync();
                    var result = await command.ExecuteScalarAsync();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_GetUserByEmail", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", email);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new User
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FullName = reader.GetString(reader.GetOrdinal("FullName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                                Role = reader.GetString(reader.GetOrdinal("Role"))
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
