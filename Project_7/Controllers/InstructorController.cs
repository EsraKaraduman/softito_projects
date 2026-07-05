using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_7.Models;
using Project_7.Repositories;

namespace Project_7.Controllers
{
    [Authorize(Roles = "Admin")]
    public class InstructorController : Controller
    {
        private readonly IInstructorRepository _instructorRepository;

        public InstructorController(IInstructorRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }

        public async Task<IActionResult> Index()
        {
            var instructors = await _instructorRepository.GetAllAsync();
            return View(instructors);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                await _instructorRepository.AddAsync(instructor);
                return RedirectToAction(nameof(Index));
            }
            return View(instructor);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var instructor = await _instructorRepository.GetByIdAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                await _instructorRepository.UpdateAsync(instructor);
                return RedirectToAction(nameof(Index));
            }
            return View(instructor);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _instructorRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
