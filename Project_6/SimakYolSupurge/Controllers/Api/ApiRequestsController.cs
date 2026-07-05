using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimakYolSupurge.Data;
using SimakYolSupurge.Models;

namespace SimakYolSupurge.Controllers.Api
{
    [ApiController]
    [Route("api/requests")]
    [Authorize]
    public class ApiRequestsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ApiRequestsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests([FromQuery] string? search)
        {
            var query = _context.Requests.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                var lowerSearch = search.ToLower();
                query = query.Where(r => r.CustomerName.ToLower().Contains(lowerSearch) ||
                                         r.CustomerEmail.ToLower().Contains(lowerSearch) ||
                                         r.CustomerPhone.ToLower().Contains(lowerSearch) ||
                                         r.VehicleModel.ToLower().Contains(lowerSearch) ||
                                         r.RequestType.ToLower().Contains(lowerSearch) ||
                                         r.Status.ToLower().Contains(lowerSearch) ||
                                         r.Notes.ToLower().Contains(lowerSearch));
            }

            return await query.ToListAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound("Talep bulunamadı.");
            }

            return request;
        }

        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(Request request)
        {
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRequest), new { id = request.Id }, request);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutRequest(int id, Request request)
        {
            if (id != request.Id)
            {
                return BadRequest("ID uyuşmazlığı.");
            }

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
                {
                    return NotFound("Talep bulunamadı.");
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
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound("Talep bulunamadı.");
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("export/excel")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExportToExcel()
        {
            var requests = await _context.Requests.ToListAsync();
            var builder = new System.Text.StringBuilder();
            builder.AppendLine("ID;Müşteri Adı;E-posta;Telefon;Araç Modeli;Talep Türü;Tarih;Durum;Notlar");
            foreach (var r in requests)
            {
                builder.AppendLine($"{r.Id};{r.CustomerName};{r.CustomerEmail};{r.CustomerPhone};{r.VehicleModel};{r.RequestType};{r.RequestDate.ToString("dd.MM.yyyy")};{r.Status};{r.Notes}");
            }
            var fileBytes = System.Text.Encoding.UTF8.GetPreamble().Concat(System.Text.Encoding.UTF8.GetBytes(builder.ToString())).ToArray();
            return File(fileBytes, "text/csv", "Talepler.csv");
        }

        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }
    }
}
