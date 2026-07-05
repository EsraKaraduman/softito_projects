using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_7.Repositories;

namespace Project_7.Controllers
{
    [Authorize]
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public EnrollmentController(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Enroll(int courseId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Challenge();
            }

            int userId = Convert.ToInt32(userIdClaim.Value);
            var result = await _enrollmentRepository.EnrollAsync(userId, courseId);

            if (result > 0)
            {
                TempData["SuccessMessage"] = "Kursa başarıyla kaydoldunuz!";
            }
            else
            {
                TempData["ErrorMessage"] = "Bu kursa zaten kayıtlısınız veya kayıt sırasında bir sorun oluştu.";
            }

            return RedirectToAction("MyCourses");
        }

        [HttpGet]
        public async Task<IActionResult> MyCourses()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Challenge();
            }

            int userId = Convert.ToInt32(userIdClaim.Value);
            var enrollments = await _enrollmentRepository.GetEnrollmentsAsync(userId);
            return View(enrollments);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsAsync(null);
            return View(enrollments);
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id, string returnUrl)
        {
            await _enrollmentRepository.CancelEnrollmentAsync(id);
            TempData["SuccessMessage"] = "Kayıt başarıyla iptal edildi.";

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(MyCourses));
        }
    }
}
