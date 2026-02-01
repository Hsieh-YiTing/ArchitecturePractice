using Microsoft.AspNetCore.Mvc;

namespace ArchitecturePractice.Controllers.ExportReport
{
    public class ExportReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
