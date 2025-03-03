using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_B.Data;
using Project_B.Models;

namespace Project_B.Controllers
{
    public class CartController : Controller
    {
        private readonly UserManager<AppUserModel> _userManager;
        private readonly DataContext _context;
        public CartController(UserManager<AppUserModel> userManager, DataContext context)
        {
            this._userManager = userManager;
            this._context = context;
        }
        public async Task<IActionResult> Index()
        {
            
            var user = await _userManager.GetUserAsync(User);
                
            var userId = user.Id;
            var wishlist = await _context.WishLists
                                 .Where(p => p.UserId == userId)
                                 .ToListAsync();
            return View(wishlist);
                
            

        }
    }
}
