using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimakYolSupurge.Data;
using SimakYolSupurge.Models;

namespace SimakYolSupurge.Controllers.Api
{
    [ApiController]
    [Route("api/maintenances")]
    [Authorize]
    public class ApiMaintenancesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ApiMaintenancesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Maintenance>>> GetMaintenances([FromQuery] string? search)
        {
            var query = _context.Maintenances.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                var lowerSearch = search.ToLower();
                query = query.Where(m => m.VehicleModel.ToLower().Contains(lowerSearch) ||
                                         m.Description.ToLower().Contains(lowerSearch) ||
                                         m.Status.ToLower().Contains(lowerSearch) ||
                                         m.TechnicianName.ToLower().Contains(lowerSearch));
            }

            return await query.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Maintenance>> GetMaintenance(int id)
        {
            var maintenance = await _context.Maintenances.FindAsync(id);

            if (maintenance == null)
            {
                return NotFound("Bakım kaydı bulunamadı.");
            }

            return maintenance;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Maintenance>> PostMaintenance(Maintenance maintenance)
        {
            _context.Maintenances.Add(maintenance);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMaintenance), new { id = maintenance.Id }, maintenance);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutMaintenance(int id, Maintenance maintenance)
        {
            if (id != maintenance.Id)
            {
                return BadRequest("ID uyuşmazlığı.");
            }

            _context.Entry(maintenance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaintenanceExists(id))
                {
                    return NotFound("Bakım kaydı bulunamadı.");
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
        public async Task<IActionResult> DeleteMaintenance(int id)
        {
            var maintenance = await _context.Maintenances.FindAsync(id);
            if (maintenance == null)
            {
                return NotFound("Bakım kaydı bulunamadı.");
            }

            _context.Maintenances.Remove(maintenance);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("export/excel")]
        public async Task<IActionResult> ExportToExcel()
        {
            var maintenances = await _context.Maintenances.ToListAsync();
            var builder = new System.Text.StringBuilder();
            builder.AppendLine("ID;Araç Modeli;Açıklama;Başlangıç Tarihi;Bitiş Tarihi;Maliyet;Durum;Teknisyen/Usta");
            foreach (var m in maintenances)
            {
                builder.AppendLine($"{m.Id};{m.VehicleModel};{m.Description};{m.StartDate.ToString("dd.MM.yyyy")};{m.EndDate?.ToString("dd.MM.yyyy") ?? "-"};{m.Cost};{m.Status};{m.TechnicianName}");
            }
            var fileBytes = System.Text.Encoding.UTF8.GetPreamble().Concat(System.Text.Encoding.UTF8.GetBytes(builder.ToString())).ToArray();
            return File(fileBytes, "text/csv", "Bakimlar.csv");
        }

        private bool MaintenanceExists(int id)
        {
            return _context.Maintenances.Any(e => e.Id == id);
        }
    }
}
