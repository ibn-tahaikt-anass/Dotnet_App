using Microsoft.EntityFrameworkCore;
using MES.Models;

namespace MES.Data
{
    public class ProductsContext : DbContext
    {
      
     

        public ProductsContext(DbContextOptions<ProductsContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
