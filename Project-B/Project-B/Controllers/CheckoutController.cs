using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_B.Data;
using Project_B.Models;

namespace Project_B.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly UserManager<AppUserModel> _userManager;
        private readonly DataContext _context;
        private readonly ILogger<WishListController> _logger;


        public CheckoutController(UserManager<AppUserModel> userManager, DataContext context, ILogger<WishListController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.User = user;
            var userId = user.Id;
            ViewBag.WishList = await _context.WishLists
                                 .Where(p => p.UserId == userId)
                                 .ToListAsync();
            return View();
        }
    }
}
