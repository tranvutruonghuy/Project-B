using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project_B.Models;

namespace Project_B.Data
{
    public class DataContext : DbContext
    {
        public DataContext (DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Project_B.Models.CategoryModel> Categories { get; set; } = default!;
        public DbSet<Project_B.Models.ProductModel> Products { get; set; } = default!;
    }
}
