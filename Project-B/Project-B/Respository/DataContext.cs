using Microsoft.EntityFrameworkCore;
using Project_B.Models;

namespace Project_B.Respository
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<AddressModel> Addresses { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<ClientModel> Clients { get; set; }
        public DbSet<OrderDetailsModel> OrderDetails { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
    }
}
