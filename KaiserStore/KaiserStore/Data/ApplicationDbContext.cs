using KaiserStore.Models;
using Microsoft.EntityFrameworkCore;

namespace KaiserStore.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }
        public DbSet<AccountsVM> accounts {get; set; }
        public DbSet<CategoryVM> categorys { get; set; }
        public DbSet<ProductsVM> products { get; set; }
        public DbSet<SizeItem> sizes { get; set; }
        public DbSet<Payment> payments { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<ImportDetails> importDetails { get; set; }
        public DbSet<Slide> slides { get; set; }
        public DbSet<ProductType> productTypes { get; set; }

    }
}
