﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_B.Data;

namespace Project_B.Components
{
    public class CategorySideBarViewComponent : ViewComponent
    {
        private readonly DataContext _dataContext;
        public CategorySideBarViewComponent(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _dataContext
                .Categories
                .ToListAsync();
            var products = await _dataContext.Products.ToListAsync();
            ViewBag.Products = products;
            ViewBag.Categories = categories;
            return View();
        }
    }
}