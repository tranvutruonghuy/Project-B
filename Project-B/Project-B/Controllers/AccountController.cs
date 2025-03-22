using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Project_B.Data;
using Project_B.Models;
using Project_B.Models.ViewModel;
using System.Security.Claims;

namespace Project_B.Controllers
{

    public class AccountController : Controller
    {
        private UserManager<AppUserModel> _userManager;
        private SignInManager<AppUserModel> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly DataContext _context;

        public AccountController(UserManager<AppUserModel> userManager, SignInManager<AppUserModel> signInManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender, DataContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserModel user)
        {
            if (ModelState.IsValid)
            { 
                AppUserModel newUser = new AppUserModel 
                {
                    UserName = user.Username, 
                    Email = user.Email, 
                    PhoneNumber = user.Phone,
                    FullName = user.Name,
                };
                IdentityResult result = await _userManager.CreateAsync(newUser, user.Password);
                if (result.Succeeded) 
                {
                    var roleExists = await _roleManager.RoleExistsAsync("User");
                    if (roleExists)
                    {
                        await _userManager.AddToRoleAsync(newUser, "User");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Role 'User' does not exist.");
                    }
                    TempData["success"] = "Register successfully!";
                    return Redirect("/account");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }
        [HttpGet]
        public Task<IActionResult> Login()
        {
            return Task.FromResult<IActionResult>(View());
        }
        [HttpPost]
        public async Task<IActionResult> Login([Bind("Username, Password")] UserModel user)
        {
            ModelState.Remove("Email");
            ModelState.Remove("Name");
            ModelState.Remove("Phone");
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user.Username, user.Password, false, false);
                if (result.Succeeded)
                {
                    var existingUser = await _userManager.FindByNameAsync(user.Username);
                    var roles = await _userManager.GetRolesAsync(existingUser);

                    var claims = new List<Claim>
                    {
                        new Claim("FullName", existingUser.FullName)
                    };

                    

                    if (roles.Contains("User"))
                    {
                        await _userManager.AddToRoleAsync(existingUser, "User");
                        Console.WriteLine("Welcome!");
                        await _signInManager.SignInWithClaimsAsync(existingUser, false, claims);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Sorry! Admin cannot loggin into user pages.");
                        await _signInManager.SignOutAsync(); // Đăng xuất nếu không có vai trò User
                        return View(user);
                    }
                }
                ModelState.AddModelError("", "Invalid username or password");
            }
            return View(user);
        }


        public async Task<IActionResult> Logout (string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }
        [HttpGet]
        [Route("/admin/create")]
        public IActionResult CreateAdmin()
        {
            return View();
        }
        [HttpPost]
        [Route("/admin/create")]
        public async Task<IActionResult> CreateAdmin(UserModel user)
        {
            if (ModelState.IsValid)
            {
                AppUserModel newUser = new AppUserModel 
                { 
                    UserName = user.Username,
                    Email = user.Email,
                    PhoneNumber = user.Phone,
                    FullName = user.Name
                };
                IdentityResult result = await _userManager.CreateAsync(newUser, user.Password);
                if (result.Succeeded)
                {
                    var roleExists = await _roleManager.RoleExistsAsync("Admin");
                    if (roleExists)
                    {

                        await _userManager.AddToRoleAsync(newUser, "Admin");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Role 'User' does not exist.");
                    }
                    TempData["success"] = "Register successfully!";
                    return Redirect("/admin/login");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }
        [HttpGet]
        [Route("/admin/login")]
        public IActionResult LoginAdmin()
        {
            return View();
        }
        [HttpPost]
        [Route("/admin/login")]
        public async Task<IActionResult> LoginAdmin([Bind("Username, Password")] UserModel user)
        {
            ModelState.Remove("Email");
            ModelState.Remove("Name");
            ModelState.Remove("Phone");
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user.Username, user.Password, false, false);
                if (result.Succeeded)
                {
                    var existingUser = await _userManager.FindByNameAsync(user.Username);
                    var roles = await _userManager.GetRolesAsync(existingUser);

                    var claims = new List<Claim>
                    {
                        new Claim("FullName", existingUser.FullName)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "Login");
                    

                    if (roles.Contains("Admin"))
                    {
                        await _userManager.AddToRoleAsync(existingUser, "Admin");
                        Console.WriteLine("Welcome!");
                        await _signInManager.SignInWithClaimsAsync(existingUser, false, claims);
                        return RedirectToAction("ProductList", "Product");

                    }
                    else
                    {
                        ModelState.AddModelError("", "User does not have the required role.");
                        await _signInManager.SignOutAsync(); // Đăng xuất nếu không có vai trò User
                        return View(user);
                    }

                }
                ModelState.AddModelError("", "Invalid username or password");
            }
            return View(user);
        }
        [Route("/admin/logout")]
        public async Task<IActionResult> LogoutAdmin()
        {
            await _signInManager.SignOutAsync();
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var callbackUrl = Url.Action("ResetPassword", "Account", new { token = token, email = model.Email }, protocol: HttpContext.Request.Scheme);
                    await _emailSender.SendEmailAsync(model.Email, "Reset Password", $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                }
                return RedirectToAction("ForgotPasswordConfirmation");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
                if (token == null || email == null)
                {
                    return RedirectToAction("Error");
                }
                var model = new ResetPasswordViewModel { Token = token, Email = email };
                return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Không để lộ rằng người dùng không tồn tại
                return RedirectToAction("ResetPasswordConfirmation");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [Route("/user/account")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AccountDetails()
        {
            var user = await _userManager.GetUserAsync(User);
            var orders = _context.Orders.Where(w => w.ClientId == user.Id).ToList();
            ViewBag.Orders = orders;
            return View();
        }
    }
}
