using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_B.Data;
using Project_B.Models;
using Project_B.Models.ViewModel;
using System.Globalization;

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
        [Authorize(Roles = "User")]
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

        
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ProceedCheckout([FromBody] CheckOutViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if(user != null)
                {
                    var userId = user.Id;
                    List<WishListModel> wishLists = await _context.WishLists.Where(p => p.UserId == userId).ToListAsync();

                    decimal totalValue = 0;
                    foreach (var item in wishLists)
                    {
                        //kiem tra so luong cua product trong kho
                        ProductModel productInWishlist = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
                        if(productInWishlist.InStock < item.Quantity)
                        {
                            return Json(new { success = false, message = $"Item {productInWishlist.Name} is out of stock." });
                        } else
                        {
                            productInWishlist.InStock -= item.Quantity;
                            _context.SaveChanges();
                        }

                        totalValue += item.Price;
                    }
                    string currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff");

                    // tao bang orders
                    OrderModel orderModel = new OrderModel
                    {
                        ClientId = user.Id,
                        OrderDate = DateTime.ParseExact(currentDate, "yyyy-MM-dd HH:mm:ss.ffffff", CultureInfo.InvariantCulture),
                        TotalValue = totalValue,
                        Note = model.Note,
                        PaymentMethod = model.PaymentMethod,
                        Status = 0,
                    };
                    _context.Add(orderModel);
                    await _context.SaveChangesAsync();

                    //tao bang address ung voi moi orders
                    AddressModel address = new AddressModel
                    {
                        District = model.District,
                        Province = model.Province,
                        Street = model.Street,
                        Ward = model.Ward,
                        OrderId = orderModel.Id
                    };

                    _context.Add(address);
                    await _context.SaveChangesAsync();

                    //tao ra orderdetails ung voi moi item trong wishlist, sau do xoas ra khoi wishlist
                    foreach (var item in wishLists)
                    {
                        OrderDetailsModel orderDetailsModel = new OrderDetailsModel
                        {
                            OrderId = orderModel.Id,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            Price = item.Price,
                        };
                        _context.WishLists.Remove(item);
                        _context.Add(orderDetailsModel);
                        await _context.SaveChangesAsync();
                    }
                }
                return Json(new { success = true, message = "Add successfully!" });
            }

           
            return Json(new { success = false, message = "NO OK!" });
        }

        
        [Route("Admin/Orders")]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> OrderList()
        {
            var orders = await _context.Orders.ToListAsync();
            List<UserOrderViewModel> ordersList = new List<UserOrderViewModel>();

            foreach (var order in orders) 
            {
                var user = await _context.Users.FirstOrDefaultAsync(w => w.Id == order.ClientId);
                UserOrderViewModel each = new UserOrderViewModel
                {
                    Order = order,
                    User = user
                };
                ordersList.Add(each);
            }
            ViewBag.OrdersList = ordersList;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetOrderDetails(int id)
        {
            var orderDetails = _context.OrderDetails
                             .Where(od => od.OrderId == id)
                             .Select(od => new {
                                 od.Product.Name,
                                 od.Quantity,
                                 od.Price
                             })
                             .ToList();
            return Json(orderDetails);

        }

        [HttpPost]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(w => w.Id == id);
            if (order != null) 
            {
                if (order.IsCancel == 1)
                {
                    return BadRequest(new { message = "This order has already been canceled." });
                }
                if(order.Status == 1)
                {
                    return BadRequest(new { message = "This order has already been confirm. The cancel form has been send to admin. Please wait for the response of admin" });
                }
                order.IsCancel = 1; 
                await _context.SaveChangesAsync(); 
                return Ok(new { message = "Order successfully canceled." });
            }
            return NotFound(new { message = "Order not found." });

        }

        [HttpPost]
        public async Task<IActionResult> ConfirmOrder(int id)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(w => w.Id == id);
            if (order != null)
            {
                if(order.IsCancel == 1)
                {
                    return BadRequest(new { message = "This order has already been canceled." });
                }
                order.Status = 1;
                await _context.SaveChangesAsync();
                return Ok(new { message = "Order successfully canceled." });
            }
            return NotFound(new { message = "Order not found." });

        }

    }
}
