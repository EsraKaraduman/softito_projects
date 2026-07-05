using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project_7.Models;
using Project_7.Repositories;

namespace Project_7.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CourseController : Controller
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IInstructorRepository _instructorRepository;

        public CourseController(ICourseRepository courseRepository, IInstructorRepository instructorRepository)
        {
            _courseRepository = courseRepository;
            _instructorRepository = instructorRepository;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _courseRepository.GetAllAsync();
            return View(courses);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var instructors = await _instructorRepository.GetAllAsync();
            ViewBag.Instructors = new SelectList(instructors, "Id", "FullName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Course course)
        {
            if (ModelState.IsValid)
            {
                await _courseRepository.AddAsync(course);
                return RedirectToAction(nameof(Index));
            }
            var instructors = await _instructorRepository.GetAllAsync();
            ViewBag.Instructors = new SelectList(instructors, "Id", "FullName", course.InstructorId);
            return View(course);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            var instructors = await _instructorRepository.GetAllAsync();
            ViewBag.Instructors = new SelectList(instructors, "Id", "FullName", course.InstructorId);
            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                await _courseRepository.UpdateAsync(course);
                return RedirectToAction(nameof(Index));
            }
            var instructors = await _instructorRepository.GetAllAsync();
            ViewBag.Instructors = new SelectList(instructors, "Id", "FullName", course.InstructorId);
            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _courseRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
