using Microsoft.AspNetCore.Mvc;

namespace Renting_Immobile.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
