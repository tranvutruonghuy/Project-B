using Microsoft.AspNetCore.Mvc;

namespace Project_B.Controllers
{
    [Route("error")]
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [Route("404")]
        public IActionResult PageNotFound() {
            return View();
        }
    }
}
