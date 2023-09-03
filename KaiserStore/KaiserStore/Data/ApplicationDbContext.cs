using KaiserStore.Models;
using Microsoft.EntityFrameworkCore;

namespace KaiserStore.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }
        public DbSet<AccountsVM> accounts {get; set; }
        public DbSet<CategoryVM> category { get; set; }
    }
}
