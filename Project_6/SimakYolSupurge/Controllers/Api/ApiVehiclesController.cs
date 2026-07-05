using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimakYolSupurge.Data;
using SimakYolSupurge.Models;

namespace SimakYolSupurge.Controllers.Api
{
    [ApiController]
    [Route("api/vehicles")]
    [Authorize]
    public class ApiVehiclesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ApiVehiclesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicles([FromQuery] string? search)
        {
            var query = _context.Vehicles.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                var lowerSearch = search.ToLower();
                query = query.Where(v => v.Brand.ToLower().Contains(lowerSearch) ||
                                         v.Model.ToLower().Contains(lowerSearch) ||
                                         v.Type.ToLower().Contains(lowerSearch) ||
                                         v.Status.ToLower().Contains(lowerSearch) ||
                                         v.Description.ToLower().Contains(lowerSearch));
            }

            return await query.ToListAsync();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Vehicle>> GetVehicle(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);

            if (vehicle == null)
            {
                return NotFound("Araç bulunamadı.");
            }

            return vehicle;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Vehicle>> PostVehicle(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVehicle), new { id = vehicle.Id }, vehicle);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutVehicle(int id, Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return BadRequest("ID uyuşmazlığı.");
            }

            _context.Entry(vehicle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(id))
                {
                    return NotFound("Araç bulunamadı.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound("Araç bulunamadı.");
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("export/excel")]
        [AllowAnonymous]
        public async Task<IActionResult> ExportToExcel()
        {
            var vehicles = await _context.Vehicles.ToListAsync();
            var builder = new System.Text.StringBuilder();
            builder.AppendLine("ID;Marka;Model;Tür;Yıl;Fiyat;Durum;Açıklama");
            foreach (var v in vehicles)
            {
                builder.AppendLine($"{v.Id};{v.Brand};{v.Model};{v.Type};{v.Year};{v.Price};{v.Status};{v.Description}");
            }
            var fileBytes = System.Text.Encoding.UTF8.GetPreamble().Concat(System.Text.Encoding.UTF8.GetBytes(builder.ToString())).ToArray();
            return File(fileBytes, "text/csv", "Araclar.csv");
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicles.Any(e => e.Id == id);
        }
    }
}
