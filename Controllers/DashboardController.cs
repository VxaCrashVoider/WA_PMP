using Microsoft.AspNetCore.Mvc;

namespace WA_ProjectManagement.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MyWork()
        {
            return View();
        }

        public IActionResult Statistic()
        {
            return View();
        }

        public IActionResult Team()
        {
            return View();
        }
    }
}
