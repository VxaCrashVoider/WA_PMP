using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WA_ProjectManagement.Data;

namespace WA_ProjectManagement.Controllers
{
    public class IssueTypesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public IssueTypesController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _db.IssueTypes.AsNoTracking().OrderBy(t => t.Name).ToListAsync();
            return View(items);
        }
    }
}
