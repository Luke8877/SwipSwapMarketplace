using Microsoft.EntityFrameworkCore;
using SwipSwapMarketplace.Models;

namespace SwipSwapMarketplace.Data
{
    /// <summary>
    /// Defines the application's primary database context.
    /// Handles entity configuration, relationships, and database mapping for the SwipSwap marketplace.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the database context using the specified options.
        /// </summary>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets represent the tables in the database
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Address> Addresses { get; set; }

        /// <summary>
        /// Configures model relationships and database-specific behaviors.
        /// </summary>
        /// <param name="modelBuilder">The model builder used to define entity relationships.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- USER → ORDER RELATIONSHIP ---
            // Prevent cascade delete loops between User and Order entities
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Buyer)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- USER → PRODUCT RELATIONSHIP ---
            // Allow cascading deletes for Products when a Seller (User) is removed
            modelBuilder.Entity<Product>()
                .HasOne(p => p.User)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.SellerId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- ORDER → PAYMENT RELATIONSHIP ---
            // Defines a one-to-one relationship where Payment depends on Order
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Electronics" }
            );

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Username = "JaneDoe",
                    PasswordHash = "placeholder",
                    Email = "jane@example.com"
                },
                new User
                {
                    UserId = 2,
                    Username = "JohnSmith",
                    PasswordHash = "placeholder",
                    Email = "john@example.com"
                }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    Name = "Gaming Laptop",
                    Description = "High-end gaming laptop with RTX 4070",
                    Price = 1800m,
                    CategoryId = 1,   // FK to Electronics
                    SellerId = 1,     // FK to Jane
                    ImageUrl = "/uploads/laptop.jpg",
                    IsSold = false,
                    DatePosted = new DateTime(2025, 11, 1)
                    // Navigation properties left out, EF resolves via FKs
                },
                new Product
                {
                    ProductId = 2,
                    Name = "Smartphone",
                    Description = "Flagship smartphone with OLED display",
                    Price = 900m,
                    CategoryId = 1,   // Electronics
                    SellerId = 2,     // John
                    ImageUrl = "/uploads/smartphone.jpg",
                    IsSold = true,
                    DatePosted = new DateTime(2025, 10, 28)
                }
                // Add more products as needed
            );


        }
    }
}
