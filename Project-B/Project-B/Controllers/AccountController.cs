using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project_B.Models;
using System.Security.Claims;

namespace Project_B.Controllers
{

    public class AccountController : Controller
    {
        private UserManager<AppUserModel> _userManager;
        private SignInManager<AppUserModel> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<AppUserModel> userManager, SignInManager<AppUserModel> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
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
                        Console.WriteLine("Welcome!");
                        await _signInManager.SignInWithClaimsAsync(existingUser, false, claims);
                        return RedirectToAction("Index", "Home");
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
    }
}
