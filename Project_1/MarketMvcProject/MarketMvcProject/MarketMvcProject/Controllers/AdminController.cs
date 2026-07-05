using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace MarketMvcProject.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");

            if (role == null)
                return RedirectToAction("Login", "Account");

            if (role != "Admin")
                return RedirectToAction("Login", "Account");

            return View();
        }

        public IActionResult Dashboard()
        {
            var role = HttpContext.Session.GetString("Role");

            if (role != "Admin")
                return RedirectToAction("Login", "Account");

            return View();
        }
    }
}
