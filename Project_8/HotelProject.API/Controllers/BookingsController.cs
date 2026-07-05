using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using HotelProject.API.Data;
using HotelProject.API.Models;
using System.Data;

namespace HotelProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly DbConnectionProvider _dbProvider;

        public BookingsController(DbConnectionProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bookings = new List<Booking>();
            using var connection = _dbProvider.CreateSqlConnection();
            using var command = new SqlCommand("sp_GetAllBookings", connection);
            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                bookings.Add(MapBooking(reader));
            }

            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            using var connection = _dbProvider.CreateSqlConnection();
            using var command = new SqlCommand("sp_GetBookingById", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Id", id);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return Ok(MapBooking(reader));
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Booking booking)
        {
            using var connection = _dbProvider.CreateSqlConnection();
            using var command = new SqlCommand("sp_InsertBooking", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@RoomId", booking.RoomId);
            command.Parameters.AddWithValue("@GuestName", booking.GuestName);
            command.Parameters.AddWithValue("@GuestEmail", booking.GuestEmail);
            command.Parameters.AddWithValue("@GuestPhone", booking.GuestPhone);
            command.Parameters.AddWithValue("@CheckInDate", booking.CheckInDate);
            command.Parameters.AddWithValue("@CheckOutDate", booking.CheckOutDate);
            command.Parameters.AddWithValue("@TotalPrice", booking.TotalPrice);
            command.Parameters.AddWithValue("@BookingStatus", booking.BookingStatus);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            return Ok(new { Message = "Booking created successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Booking booking)
        {
            using var connection = _dbProvider.CreateSqlConnection();
            using var command = new SqlCommand("sp_UpdateBooking", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@RoomId", booking.RoomId);
            command.Parameters.AddWithValue("@GuestName", booking.GuestName);
            command.Parameters.AddWithValue("@GuestEmail", booking.GuestEmail);
            command.Parameters.AddWithValue("@GuestPhone", booking.GuestPhone);
            command.Parameters.AddWithValue("@CheckInDate", booking.CheckInDate);
            command.Parameters.AddWithValue("@CheckOutDate", booking.CheckOutDate);
            command.Parameters.AddWithValue("@TotalPrice", booking.TotalPrice);
            command.Parameters.AddWithValue("@BookingStatus", booking.BookingStatus);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            return Ok(new { Message = "Booking updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            using var connection = _dbProvider.CreateSqlConnection();
            using var command = new SqlCommand("sp_DeleteBooking", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Id", id);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            return Ok(new { Message = "Booking deleted successfully" });
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            var bookings = new List<Booking>();
            using var connection = _dbProvider.CreateSqlConnection();
            using var command = new SqlCommand("sp_SearchBookings", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@SearchTerm", query ?? string.Empty);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                bookings.Add(MapBooking(reader));
            }

            return Ok(bookings);
        }

        private Booking MapBooking(SqlDataReader reader)
        {
            return new Booking
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                RoomId = reader.GetInt32(reader.GetOrdinal("RoomId")),
                GuestName = reader.GetString(reader.GetOrdinal("GuestName")),
                GuestEmail = reader.GetString(reader.GetOrdinal("GuestEmail")),
                GuestPhone = reader.GetString(reader.GetOrdinal("GuestPhone")),
                CheckInDate = reader.GetDateTime(reader.GetOrdinal("CheckInDate")),
                CheckOutDate = reader.GetDateTime(reader.GetOrdinal("CheckOutDate")),
                TotalPrice = reader.GetDecimal(reader.GetOrdinal("TotalPrice")),
                BookingStatus = reader.GetString(reader.GetOrdinal("BookingStatus")),
                RoomNumber = reader.GetString(reader.GetOrdinal("RoomNumber")),
                RoomType = reader.GetString(reader.GetOrdinal("RoomType"))
            };
        }
    }
}
