using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_B.Data;
using Project_B.Models;

namespace Project_B.Controllers
{
    public class AdminController : Controller
    {
        private readonly DataContext _context;
        IWebHostEnvironment webHostEnvironment;
        public AdminController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: Admin/CreateProduct
        public IActionResult CreateProduct()
        {
            ViewData["CategoryId"] = new SelectList(_context.CategoryModel, "Id", "Name");
            return View();
        }

        // POST: Admin/CreateProduct
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct([Bind("Id,Name,CategoryId,Description,ShortDescription,InStock,Price,Unit, Image")] ProductModel productModel, IFormFile Image)
        {
           
            if (ModelState.IsValid)
            {
                //coppy lai duong dan cua img
                string currentDate = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss_fff");
                String fileName = "";
                if (Image != null)
                {
                    String uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploadImages");
                    fileName = productModel.Name + "_" + currentDate + "_" + Image.FileName;
                    String filePath = Path.Combine(uploadFolder, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
                    {
                        Image.CopyTo(fileStream);
                    }

                }
                ProductModel newProduct = new ProductModel();
                newProduct.Name = productModel.Name;
                newProduct.Description = productModel.Description;
                newProduct.CategoryId = productModel.CategoryId;
                newProduct.ShortDescription = productModel.ShortDescription;
                newProduct.Price = productModel.Price;
                newProduct.InStock = productModel.InStock;
                newProduct.Image = fileName;
                newProduct.Unit = productModel.Unit;
                _context.Add(newProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            //ViewData["CategoryId"] = new SelectList(_context.CategoryModel, "Id", "Name", productModel.CategoryId);
            return View(productModel);
        }

        // POST: Admin/ProductList
        public IActionResult ProductList()
        {
            var productList = _context.ProductModel.ToList();
            var categoryList = _context.CategoryModel.ToList();
           
            ViewBag.ProductList = productList;
            ViewBag.CategoryList = categoryList;
         
            return View();
        }

        // GET: Product/DeleteProduct/5
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productModel = await _context.ProductModel
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productModel == null)
            {
                return NotFound();
            }

            return RedirectToAction("ProductList");
        }

    }
}
