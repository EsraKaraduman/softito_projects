using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Project_7.Models;

namespace Project_7.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly string _connectionString;

        public EnrollmentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<int> EnrollAsync(int userId, int courseId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_EnrollUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@CourseId", courseId);

                    await connection.OpenAsync();
                    var result = await command.ExecuteScalarAsync();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        public async Task<IEnumerable<EnrollmentDetailViewModel>> GetEnrollmentsAsync(int? userId)
        {
            var list = new List<EnrollmentDetailViewModel>();
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_GetEnrollments", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", userId ?? (object)DBNull.Value);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new EnrollmentDetailViewModel
                            {
                                EnrollmentId = reader.GetInt32(reader.GetOrdinal("EnrollmentId")),
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                UserFullName = reader.GetString(reader.GetOrdinal("UserFullName")),
                                UserEmail = reader.GetString(reader.GetOrdinal("UserEmail")),
                                CourseId = reader.GetInt32(reader.GetOrdinal("CourseId")),
                                CourseTitle = reader.GetString(reader.GetOrdinal("CourseTitle")),
                                CoursePrice = reader.GetDecimal(reader.GetOrdinal("CoursePrice")),
                                Category = reader.GetString(reader.GetOrdinal("Category")),
                                EnrollmentDate = reader.GetDateTime(reader.GetOrdinal("EnrollmentDate")),
                                Status = reader.GetString(reader.GetOrdinal("Status"))
                            });
                        }
                    }
                }
            }
            return list;
        }

        public async Task<bool> CancelEnrollmentAsync(int enrollmentId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_CancelEnrollment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EnrollmentId", enrollmentId);

                    await connection.OpenAsync();
                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
    }
}
