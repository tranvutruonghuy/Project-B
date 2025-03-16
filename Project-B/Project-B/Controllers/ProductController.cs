using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
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
        public async Task<IActionResult> Index(int pg = 1, string sortType = "default")
        {
            List<ProductModel> product =  _context.Products.ToList();

            const int pageSize = 8;

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

            switch (sortType)
            {
                case "asc":
                    data.Sort((a, b) => a.Price.CompareTo(b.Price));
                    break;
                case "des":
                    data.Sort((a, b) => b.Price.CompareTo(a.Price));
                    break;
                default: 
                    break;
            }

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
            List<string> images = _context.ImagePath.Where(w => w.ProductId == id).Select(w => w.Path).ToList();
            foreach(var item in images)
            {
                Console.WriteLine(item);
            }
            ViewBag.Images = images;
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
        public async Task<IActionResult> Create([Bind("Id,Name,CategoryId,Description,ShortDescription,InStock,Price,Unit, Image")] ProductModel productModel, IFormFile[] Image)
        {
            ModelState.Remove("Image");
            ModelState.Remove("Category");

            if (ModelState.IsValid)
            {
                try
                {
                    
                    
                    ProductModel newProduct = new ProductModel
                    {
                        Name = productModel.Name,
                        Description = productModel.Description,
                        CategoryId = productModel.CategoryId,
                        ShortDescription = productModel.ShortDescription,
                        Price = productModel.Price,
                        InStock = productModel.InStock,
                        Image = "",
                        Unit = productModel.Unit,
                        Status = 1
                    };
                    _context.Add(newProduct);
                    await _context.SaveChangesAsync();
                    string currentDate = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss_fff");
                    string fileName = "";
                    int index = 0;
                    foreach (var file in Request.Form.Files)
                    {
                        if (file.Length > 0)
                        {
                            string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploadImages");
                            fileName = $"Product_{newProduct.Id}_{currentDate}_{file.FileName}";
                            string filePath = Path.Combine(uploadFolder, fileName);
                            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
                            {
                                await file.CopyToAsync(fileStream);
                            }
                            ImagePathModel newImagePath = new ImagePathModel
                            {
                                Path = fileName,
                                ProductId = newProduct.Id,
                                Index = index++,
                                
                            };
                            _context.Add(newImagePath);
                            await _context.SaveChangesAsync();
                        }
                    }
                    if (fileName == "")
                    {
                        fileName = "no_image.webp";
                        newProduct.Image = fileName;
                    } else
                    {
                        newProduct.Image = _context.ImagePath
                            .Where(w => w.ProductId == newProduct.Id && w.Index == 0)
                            .Select(w => w.Path)
                            .FirstOrDefault();
                    }
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
            List<string> filesName = await _context.ImagePath.Where(each => each.ProductId == id).Select(each => each.Path).ToListAsync();
            ViewBag.FilesName = filesName;
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", productModel.CategoryId);
            return View(productModel);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/Product/Edit")]

        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CategoryId,Description,ShortDescription,InStock,Price,Unit,Image")] ProductModel productModel, IFormFile Image)
        {
                var newProduct = await _context.Products.FindAsync(id);
                
                
                newProduct.Name = productModel.Name;
                newProduct.Description = productModel.Description;
                newProduct.CategoryId = productModel.CategoryId;
                newProduct.ShortDescription = productModel.ShortDescription;
                newProduct.Price = productModel.Price;
                newProduct.InStock = productModel.InStock;
                newProduct.Image = "";      
                newProduct.Unit = productModel.Unit;
                //_context.Add(newProduct);
                await _context.SaveChangesAsync();

                var imagesToDelete = _context.ImagePath.Where(img => img.ProductId == id);
                string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploadImages");
                foreach (var image in imagesToDelete)
                {
                    string filePathD = Path.Combine(uploadFolder, image.Path);
                    if (System.IO.File.Exists(filePathD))
                    {
                        System.IO.File.Delete(filePathD);
                    }
                }

                _context.ImagePath.RemoveRange(imagesToDelete);
                await _context.SaveChangesAsync();
                //coppy lai duong dan cua img
                string currentDate = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss_fff");
                String fileName = "";
                int index = 0;
                foreach (var file in Request.Form.Files)
                {
                    if (file.Length > 0)
                    {

                        fileName = $"Product_{newProduct.Id}_{currentDate}_{file.FileName}";
                        string filePath = Path.Combine(uploadFolder, fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        ImagePathModel newImagePath = new ImagePathModel
                        {
                            Path = fileName,
                            ProductId = newProduct.Id,
                            Index = index++,

                        };
                        _context.Add(newImagePath);
                        await _context.SaveChangesAsync();
                    }
                }
                if (fileName == "")
                {
                    fileName = "no_image.webp";
                    newProduct.Image = fileName;

                    ImagePathModel newImagePath = new ImagePathModel
                    {
                        Path = fileName,
                        ProductId = newProduct.Id,
                        Index = index++,

                    };
                    _context.Add(newImagePath);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    newProduct.Image = _context.ImagePath
                        .Where(w => w.ProductId == newProduct.Id && w.Index == 0)
                        .Select(w => w.Path)
                        .FirstOrDefault();
                }
            await _context.SaveChangesAsync();


                return Json(new { success = true, message = "Product edit successfully!" });
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
                if (productModel.Image != "no_image.webp")
                {
                    string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploadImages");
                    string filePath = Path.Combine(uploadFolder, productModel.Image);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
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
            var productList = _context.Products.Where(pr => pr.Status == 1).ToList();
            var categoryList = _context.Categories.ToList();

            ViewBag.ProductList = productList;
            ViewBag.CategoryList = categoryList;

            return View();
        }

        [HttpGet]
        public IActionResult LoadCategoryMenu()
        {
            var categories = _context.Categories.ToList();
            var products = _context.Products.ToList();
            ViewBag.Categories = categories;
            ViewBag.Products = products;
            return PartialView("_CategoryListPartial");
        }

        private bool ProductModelExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
