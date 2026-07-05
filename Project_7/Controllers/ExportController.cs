using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_7.Repositories;

namespace Project_7.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ExportController : Controller
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public ExportController(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<IActionResult> ExportToExcel()
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsAsync(null);

            var builder = new StringBuilder();
            builder.AppendLine("Kayit ID;Ogrenci Adi;E-posta;Kurs Adi;Kategori;Fiyat;Kayit Tarihi;Durum");

            foreach (var e in enrollments)
            {
                builder.AppendLine($"{e.EnrollmentId};\"{e.UserFullName}\";\"{e.UserEmail}\";\"{e.CourseTitle}\";\"{e.Category}\";{e.CoursePrice:F2};\"{e.EnrollmentDate:dd.MM.yyyy HH:mm}\";\"{e.Status}\"");
            }

            var csvBytes = Encoding.UTF8.GetBytes(builder.ToString());
            var bom = new byte[] { 0xEF, 0xBB, 0xBF };
            var fileBytes = new byte[bom.Length + csvBytes.Length];
            Buffer.BlockCopy(bom, 0, fileBytes, 0, bom.Length);
            Buffer.BlockCopy(csvBytes, 0, fileBytes, bom.Length, csvBytes.Length);

            return File(fileBytes, "text/csv", $"ArtCourse_Kayitlar_{DateTime.Now:ddMMyyyy}.csv");
        }

        public async Task<IActionResult> PdfReport()
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsAsync(null);
            return View(enrollments);
        }
    }
}
