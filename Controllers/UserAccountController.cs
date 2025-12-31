using Microsoft.AspNetCore.Mvc;

namespace WA_ProjectManagement.Controllers
{
    public class UserAccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Setting()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
    }
}
