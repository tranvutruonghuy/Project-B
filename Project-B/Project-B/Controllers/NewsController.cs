using Microsoft.AspNetCore.Mvc;

namespace Project_B.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult NewsDetails()
        {
            return View();
        }
    }
}
