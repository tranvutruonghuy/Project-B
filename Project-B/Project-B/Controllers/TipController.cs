using Microsoft.AspNetCore.Mvc;

namespace Project_B.Controllers
{
    public class TipController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult TipDetails()
        {
            return View();
        }
    }
}
