using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Project_B.Controllers
{
    public class TipController : Controller
    {
        [Route("user/Tips")]
        [Authorize(Roles = "User")]

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
