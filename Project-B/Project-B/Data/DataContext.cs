using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project_B.Models;

namespace Project_B.Data
{
    public class DataContext : IdentityDbContext<AppUserModel>
    {
        public DataContext (DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Project_B.Models.CategoryModel> Categories { get; set; } = default!;
        public DbSet<Project_B.Models.ProductModel> Products { get; set; } = default!;
        public DbSet<Project_B.Models.WishListModel> WishLists { get; set; } = default!;
        public DbSet<Project_B.Models.OrderModel> Orders { get; set; } = default!;
        public DbSet<Project_B.Models.OrderDetailsModel> OrderDetails { get; set; } = default!;
        public DbSet<Project_B.Models.ImagePathModel> ImagePath { get; set; } = default!;
        public DbSet<Project_B.Models.AddressModel> Address{ get; set; } = default!;
    }
}
