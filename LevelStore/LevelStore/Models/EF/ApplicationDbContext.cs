using Microsoft.EntityFrameworkCore;

namespace LevelStore.Models.EF
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<TypeColor> TypeColors { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<CartLine> CartLines { get; set; }
        public DbSet<Share> Shares { get; set; }
        public DbSet<Promo> PromoCodes { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
