using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project_7.Models;
using Project_7.Repositories;

namespace Project_7.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IInstructorRepository _instructorRepository;

        public HomeController(ICourseRepository courseRepository, IInstructorRepository instructorRepository)
        {
            _courseRepository = courseRepository;
            _instructorRepository = instructorRepository;
        }

        public async Task<IActionResult> Index(string searchQuery, string category)
        {
            var courses = string.IsNullOrEmpty(searchQuery)
                ? await _courseRepository.GetAllAsync()
                : await _courseRepository.SearchAsync(searchQuery);

            if (!string.IsNullOrEmpty(category))
            {
                courses = courses.Where(c => c.Category.Equals(category, System.StringComparison.OrdinalIgnoreCase));
            }

            ViewBag.SearchQuery = searchQuery;
            ViewBag.SelectedCategory = category;

            return View(courses);
        }

        public async Task<IActionResult> Details(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        public async Task<IActionResult> Instructors()
        {
            var instructors = await _instructorRepository.GetAllAsync();
            return View(instructors);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
