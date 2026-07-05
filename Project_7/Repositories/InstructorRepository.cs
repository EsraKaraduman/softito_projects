using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Project_7.Models;

namespace Project_7.Repositories
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly string _connectionString;

        public InstructorRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<IEnumerable<Instructor>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var instructors = await connection.QueryAsync<Instructor>(
                    "sp_GetAllInstructors",
                    commandType: CommandType.StoredProcedure
                );
                return instructors;
            }
        }

        public async Task<Instructor?> GetByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var instructor = await connection.QueryFirstOrDefaultAsync<Instructor>(
                    "sp_GetInstructorById",
                    new { Id = id },
                    commandType: CommandType.StoredProcedure
                );
                return instructor;
            }
        }

        public async Task<int> AddAsync(Instructor instructor)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@FullName", instructor.FullName);
                parameters.Add("@Specialty", instructor.Specialty);
                parameters.Add("@Bio", instructor.Bio);
                parameters.Add("@ImageUrl", instructor.ImageUrl);

                var id = await connection.ExecuteScalarAsync<int>(
                    "sp_InsertInstructor",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return id;
            }
        }

        public async Task<bool> UpdateAsync(Instructor instructor)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", instructor.Id);
                parameters.Add("@FullName", instructor.FullName);
                parameters.Add("@Specialty", instructor.Specialty);
                parameters.Add("@Bio", instructor.Bio);
                parameters.Add("@ImageUrl", instructor.ImageUrl);

                var rowsAffected = await connection.ExecuteAsync(
                    "sp_UpdateInstructor",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var rowsAffected = await connection.ExecuteAsync(
                    "sp_DeleteInstructor",
                    new { Id = id },
                    commandType: CommandType.StoredProcedure
                );
                return rowsAffected > 0;
            }
        }
    }
}
