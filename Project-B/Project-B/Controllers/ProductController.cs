using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_B.Data;
using Project_B.Models;

namespace Project_B.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataContext _context;
        IWebHostEnvironment webHostEnvironment;

        public ProductController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET: Product
        public async Task<IActionResult> Index(int pg = 1)
        {
            List<ProductModel> product = _context.Products.ToList();

            const int pageSize = 1;

            if(pg < 1)
            {
                pg = 1;
            }
            int recsCount = product.Count();

            var pager = new Paginate(recsCount, pg, pageSize);

            var recSkip = (pg - 1) * pageSize;

            var data = product.Skip(recSkip).Take(pager.PageSize).ToList();

            ViewBag.Pager = pager;
            var dataContext = _context.Products.Include(p => p.Category);
            return View(data);
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productModel = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productModel == null)
            {
                return NotFound();
            }
            ViewBag.Product = productModel;
            var category = await _context.Categories.FindAsync(productModel.CategoryId);
            ViewBag.CategoryName = category?.Name;
            return View();
        }

        // GET: Product/Create
        [Route("Admin/Product/Create")]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/Product/Create")]
        [Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Create([Bind("Id,Name,CategoryId,Description,ShortDescription,InStock,Price,Unit, Image")] ProductModel productModel, IFormFile[] Image)
        //{
        //    ModelState.Remove("Image");
        //    ModelState.Remove("Category");
        //    if (ModelState.IsValid)
        //    {
        //        string currentDate = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss_fff");
        //        String fileName = "";
        //        foreach (var file in Request.Form.Files)
        //        {
        //            if (file.Length > 0)
        //            {
                        
        //                String uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploadImages");
        //                fileName = productModel.Name + "_" + currentDate + "_" + file.FileName;
        //                String filePath = Path.Combine(uploadFolder, fileName);
        //                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
        //                {
        //                    file.CopyTo(fileStream);
        //                }
        //            }
        //        }
               
        //        ProductModel newProduct = new ProductModel();
        //        newProduct.Name = productModel.Name;
        //        newProduct.Description = productModel.Description;
        //        newProduct.CategoryId = productModel.CategoryId;
        //        newProduct.ShortDescription = productModel.ShortDescription;
        //        newProduct.Price = productModel.Price;
        //        newProduct.InStock = productModel.InStock;
        //        newProduct.Image = fileName;
        //        newProduct.Unit = productModel.Unit;
        //        _context.Add(newProduct);
        //        await _context.SaveChangesAsync();
        //        return Redirect("admin/product");
            
        //}
        //    if (!ModelState.IsValid)
        //    {
        //        var errors = ModelState.Values.SelectMany(v => v.Errors);
        //        foreach (var error in errors)
        //        {
        //            Console.WriteLine(error.ErrorMessage);
        //        }
        //    }
        //    //ViewData["CategoryId"] = new SelectList(_context.CategoryModel, "Id", "Name", productModel.CategoryId);
        //    return View(productModel);
        //}
        public async Task<IActionResult> Create([Bind("Id,Name,CategoryId,Description,ShortDescription,InStock,Price,Unit, Image")] ProductModel productModel, IFormFile[] Image)
        {
            ModelState.Remove("Image");
            ModelState.Remove("Category");

            if (ModelState.IsValid)
            {
                try
                {
                    string currentDate = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss_fff");
                    string fileName = "";
                    foreach (var file in Request.Form.Files)
                    {
                        if (file.Length > 0)
                        {
                            string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploadImages");
                            fileName = $"{productModel.Name}_{currentDate}_{file.FileName}";
                            string filePath = Path.Combine(uploadFolder, fileName);
                            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
                            {
                                await file.CopyToAsync(fileStream);
                            }
                        }
                    }

                    ProductModel newProduct = new ProductModel
                    {
                        Name = productModel.Name,
                        Description = productModel.Description,
                        CategoryId = productModel.CategoryId,
                        ShortDescription = productModel.ShortDescription,
                        Price = productModel.Price,
                        InStock = productModel.InStock,
                        Image = fileName,
                        Unit = productModel.Unit
                    };

                    _context.Add(newProduct);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Product created successfully!" });
                }
                catch (Exception ex)
                {
                    // Log the exception (optional)
                    return Json(new { success = false, message = ex.Message });
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, errors = errors });
        }

        // GET: Product/Edit/5
        [Route("Admin/Product/Edit")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productModel = await _context.Products.FindAsync(id);
            if (productModel == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", productModel.CategoryId);
            return View(productModel);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/Product/Edit")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CategoryId,Description,ShortDescription,InStock,Price,Unit,Image")] ProductModel productModel, IFormFile Image)
        {
            if (id != productModel.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Image");
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
                var newProduct = await _context.Products.FindAsync(id);
                newProduct.Name = productModel.Name;
                newProduct.Description = productModel.Description;
                newProduct.CategoryId = productModel.CategoryId;
                newProduct.ShortDescription = productModel.ShortDescription;
                newProduct.Price = productModel.Price;
                newProduct.InStock = productModel.InStock;
                if(Image != null)
                {
                    newProduct.Image = fileName;
                }
            
                newProduct.Unit = productModel.Unit;
                //_context.Add(newProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction("ProductList", "Product");
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", productModel.CategoryId);
            return View(productModel);
        }

        // GET: Product/Delete/5
        [Route("Admin/Product/Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productModel = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productModel == null)
            {
                return NotFound();
            }

            return View(productModel);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Admin/Product/Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productModel = await _context.Products.FindAsync(id);
            if (productModel != null)
            {
                _context.Products.Remove(productModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ProductList));
        }

        // Get: Product/ProductList
        [Route("Admin/Product")]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult ProductList()
        {
            var productList = _context.Products.ToList();
            var categoryList = _context.Categories.ToList();

            ViewBag.ProductList = productList;
            ViewBag.CategoryList = categoryList;

            return View();
        }

        private bool ProductModelExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
