using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Project_B.Controllers
{
    [Authorize(Roles = "User")]
    public class NewsController : Controller
    {
        [Route("user/news")]
        
        public IActionResult Index()
        {
            return View();
        }
        [Route("user/newsdetails")]
        public IActionResult NewsDetails()
        {
            return View();
        }
    }
}
