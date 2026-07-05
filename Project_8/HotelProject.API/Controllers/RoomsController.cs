using Microsoft.AspNetCore.Mvc;
using Dapper;
using HotelProject.API.Data;
using HotelProject.API.Models;
using System.Data;

namespace HotelProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly DbConnectionProvider _dbProvider;

        public RoomsController(DbConnectionProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            using var connection = _dbProvider.CreateConnection();
            var rooms = await connection.QueryAsync<Room>(
                "sp_GetAllRooms",
                commandType: CommandType.StoredProcedure
            );
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            using var connection = _dbProvider.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            var room = await connection.QueryFirstOrDefaultAsync<Room>(
                "sp_GetRoomById",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            if (room == null)
            {
                return NotFound();
            }

            return Ok(room);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Room room)
        {
            using var connection = _dbProvider.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@RoomNumber", room.RoomNumber);
            parameters.Add("@RoomType", room.RoomType);
            parameters.Add("@PricePerNight", room.PricePerNight);
            parameters.Add("@IsAvailable", room.IsAvailable);
            parameters.Add("@Description", room.Description);
            parameters.Add("@ImageUrl", room.ImageUrl);

            await connection.ExecuteAsync(
                "sp_InsertRoom",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return Ok(new { Message = "Room created successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Room room)
        {
            using var connection = _dbProvider.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@RoomNumber", room.RoomNumber);
            parameters.Add("@RoomType", room.RoomType);
            parameters.Add("@PricePerNight", room.PricePerNight);
            parameters.Add("@IsAvailable", room.IsAvailable);
            parameters.Add("@Description", room.Description);
            parameters.Add("@ImageUrl", room.ImageUrl);

            await connection.ExecuteAsync(
                "sp_UpdateRoom",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return Ok(new { Message = "Room updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            using var connection = _dbProvider.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            await connection.ExecuteAsync(
                "sp_DeleteRoom",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return Ok(new { Message = "Room deleted successfully" });
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            using var connection = _dbProvider.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@SearchTerm", query ?? string.Empty);

            var rooms = await connection.QueryAsync<Room>(
                "sp_SearchRooms",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return Ok(rooms);
        }
    }
}
