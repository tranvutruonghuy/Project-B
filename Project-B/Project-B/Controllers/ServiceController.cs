using Microsoft.AspNetCore.Mvc;

namespace Project_B.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ServiceDetails()
        {
            return View();
        }
    }
}
