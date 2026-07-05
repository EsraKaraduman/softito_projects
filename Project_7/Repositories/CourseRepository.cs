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
    public class CourseRepository : ICourseRepository
    {
        private readonly string _connectionString;

        public CourseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var courses = await connection.QueryAsync<Course, Instructor, Course>(
                    "sp_GetAllCourses",
                    (course, instructor) =>
                    {
                        course.Instructor = instructor;
                        return course;
                    },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                );
                return courses;
            }
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var courses = await connection.QueryAsync<Course, Instructor, Course>(
                    "sp_GetCourseById",
                    (course, instructor) =>
                    {
                        course.Instructor = instructor;
                        return course;
                    },
                    new { Id = id },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                );
                return courses.FirstOrDefault();
            }
        }

        public async Task<int> AddAsync(Course course)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Title", course.Title);
                parameters.Add("@Description", course.Description);
                parameters.Add("@Category", course.Category);
                parameters.Add("@Price", course.Price);
                parameters.Add("@Duration", course.Duration);
                parameters.Add("@InstructorId", course.InstructorId ?? 0);
                parameters.Add("@ImageUrl", course.ImageUrl);

                var id = await connection.ExecuteScalarAsync<int>(
                    "sp_InsertCourse",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return id;
            }
        }

        public async Task<bool> UpdateAsync(Course course)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", course.Id);
                parameters.Add("@Title", course.Title);
                parameters.Add("@Description", course.Description);
                parameters.Add("@Category", course.Category);
                parameters.Add("@Price", course.Price);
                parameters.Add("@Duration", course.Duration);
                parameters.Add("@InstructorId", course.InstructorId ?? 0);
                parameters.Add("@ImageUrl", course.ImageUrl);

                var rowsAffected = await connection.ExecuteAsync(
                    "sp_UpdateCourse",
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
                    "sp_DeleteCourse",
                    new { Id = id },
                    commandType: CommandType.StoredProcedure
                );
                return rowsAffected > 0;
            }
        }

        public async Task<IEnumerable<Course>> SearchAsync(string query)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var courses = await connection.QueryAsync<Course, Instructor, Course>(
                    "sp_SearchCourses",
                    (course, instructor) =>
                    {
                        course.Instructor = instructor;
                        return course;
                    },
                    new { SearchQuery = query },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                );
                return courses;
            }
        }
    }
}
