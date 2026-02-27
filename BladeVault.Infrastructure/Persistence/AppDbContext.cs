using BladeVault.Domain.Entities;
using BladeVault.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace BladeVault.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Користувачі
        public DbSet<User> Users => Set<User>();
        public DbSet<Address> Addresses => Set<Address>();

        // Продукти
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Knife> Knives => Set<Knife>();
        public DbSet<MultiTool> MultiTools => Set<MultiTool>();
        public DbSet<ToolComponent> ToolComponents => Set<ToolComponent>();
        public DbSet<ProductImage> ProductImages => Set<ProductImage>();
        public DbSet<Stock> Stocks => Set<Stock>();

        // Категорії
        public DbSet<Category> Categories => Set<Category>();

        // Замовлення
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Payment> Payments => Set<Payment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Підхоплює всі Configuration класи з цього Assembly автоматично
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
