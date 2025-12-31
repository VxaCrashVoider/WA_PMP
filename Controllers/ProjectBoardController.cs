using Microsoft.AspNetCore.Mvc;

namespace WA_ProjectManagement.Controllers
{
    public class ProjectBoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
