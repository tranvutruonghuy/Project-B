using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_B.Data;
using Project_B.Models;
using Project_B.Models.ViewModel;

namespace Project_B.Controllers
{
    public class WishListController : Controller
    {
        private readonly UserManager<AppUserModel> _userManager;
        private readonly DataContext _context;
        private readonly ILogger<WishListController> _logger;
        public WishListController(UserManager<AppUserModel> userManager, DataContext context, ILogger<WishListController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }


        [HttpGet]
        public IActionResult Create()
        {
            return RedirectToAction("Index", "Home");
        }

        // POST: WishListController/Create
        [HttpPost]
        public async Task<ActionResult> Create(int productId, int quantity)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var userId = user.Id;
                    var product = await _context.Products.FindAsync(productId);
                    var wishlistItemExist = await _context.WishLists.FirstOrDefaultAsync(w => w.ProductId == productId && w.UserId == userId);
                    if (wishlistItemExist != null) {
                        wishlistItemExist.Quantity += quantity;
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index", "Home");
                    }
                    if (product != null)
                    {
                        var newWishListItem = new WishListModel
                        {
                            ProductId = productId,
                            UserId = userId,
                            ProductName = product.Name,
                            Quantity = quantity,
                            Price = product.Price * quantity,
                        };

                        await _context.WishLists.AddAsync(newWishListItem);
                        await _context.SaveChangesAsync(); 

                        
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // Xử lý trường hợp sản phẩm không tồn tại
                        ModelState.AddModelError("", "Product not found.");
                        return View("Error"); // Trả về trang lỗi hoặc trang hiện tại với thông báo lỗi
                    }
                }
                else
                {
                    // Xử lý trường hợp người dùng không tồn tại
                    ModelState.AddModelError("", "User not found.");
                    return View("Error"); // Trả về trang lỗi hoặc trang hiện tại với thông báo lỗi
                }
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi vào nhật ký
                _logger.LogError(ex, "An error occurred while adding product to wishlist.");
                ModelState.AddModelError("", "An error occurred while processing your request.");
                return View("Error"); // Trả về trang lỗi hoặc trang hiện tại với thông báo lỗi
            }
        }


        // POST: WishListController/Delete/5
        [HttpPost]
        
        public async Task<ActionResult> Delete(int id)
        {        

            try
            {
                var wishlistItem = await _context.WishLists
                    .FirstOrDefaultAsync(w => w.Id == id);

                if (wishlistItem != null)
                {
                    _context.WishLists.Remove(wishlistItem);
                    await _context.SaveChangesAsync();

                    return Json(new
                    {
                        success = true,
                        message = "Item deleted successfully."
                    });
                }

                return Json(new
                {
                    success = false,
                    message = "Item not found."
                });
            }
            catch (Exception ex)
            {
                // Log the error (optional)
                Console.WriteLine(ex.Message);

                return Json(new
                {
                    success = false,
                    message = "An error occurred while deleting the item."
                });
            }
        }



        [HttpGet]
        [Route("User/Wishlist/GetCartPartial")]
        public async Task<IActionResult> GetCartPartial()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var userId = user.Id;
                    var cart = _context.WishLists.Where(w => w.UserId == userId).ToList();
                    var products = new List<CartItemViewModel>();
                    foreach (var cartItem in cart)
                    {
                        var findProduct = await _context.Products.FindAsync(cartItem.ProductId);
                        CartItemViewModel cartItem1 = new CartItemViewModel();
                        cartItem1.WishlistId = cartItem.Id;
                        cartItem1.Product = findProduct;
                        cartItem1.Quantity = cartItem.Quantity;
                        products.Add(cartItem1);
                    }
                   
                    ViewBag.CartItems = products;

                    return PartialView("_CartPartial");
                }
                ViewBag.CartItems = null;

                return PartialView("_CartPartial");
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi vào nhật ký
                _logger.LogError(ex, "An error occurred while loading the cart partial view.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpGet]
        [Route("User/Wishlist/GetSmallCartPartial")]
        public async Task<IActionResult> GetSmallCartPartial()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var userId = user.Id;
                var cart = _context.WishLists.Where(w => w.UserId == userId).ToList();
                var total = 0;
                foreach (var cartItem in cart) {
                    total += (int)cartItem.Price*cartItem.Quantity;
                }
                ViewBag.CartItems = cart;
                ViewBag.GrandTotal = total;
                return PartialView("_SmallCartPartial");
            }
            
            return PartialView("_SmallCartPartial");
        }
    }
}
