using Microsoft.EntityFrameworkCore;
using StockManager.Models;
using System;
using System.Security.Cryptography;
using System.Text;

namespace StockManager.Data
{
    public class StockDbContext : DbContext
    {
        public StockDbContext(DbContextOptions<StockDbContext> options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Supplier> Suppliers { get; set; } = null!;
        public DbSet<Warehouse> Warehouses { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var adminUser = new AppUser
            {
                Id = 1,
                Username = "admin",
                FullName = "System Admin",
                Role = "Admin",
                PasswordHash = HashPassword("admin123")
            };

            var standardUser = new AppUser
            {
                Id = 2,
                Username = "user",
                FullName = "Stock Clerk",
                Role = "User",
                PasswordHash = HashPassword("user123")
            };

            modelBuilder.Entity<AppUser>().HasData(adminUser, standardUser);

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "iPhone 15 Pro", Sku = "IPH15P-128", Category = "Electronics", Quantity = 45, UnitPrice = 1199.99m, LastUpdated = DateTime.Now.AddDays(-2) },
                new Product { Id = 2, Name = "MacBook Pro 16", Sku = "MBP16-M3", Category = "Electronics", Quantity = 12, UnitPrice = 2499.00m, LastUpdated = DateTime.Now.AddDays(-1) },
                new Product { Id = 3, Name = "Ergonomic Office Chair", Sku = "CHR-ERG-01", Category = "Furniture", Quantity = 85, UnitPrice = 299.50m, LastUpdated = DateTime.Now.AddDays(-5) },
                new Product { Id = 4, Name = "Wireless Noise Cancelling Headphones", Sku = "HDP-WRL-05", Category = "Electronics", Quantity = 150, UnitPrice = 199.99m, LastUpdated = DateTime.Now },
                new Product { Id = 5, Name = "Standing Desk", Sku = "DSK-STN-02", Category = "Furniture", Quantity = 30, UnitPrice = 450.00m, LastUpdated = DateTime.Now.AddDays(-3) }
            );

            modelBuilder.Entity<Supplier>().HasData(
                new Supplier { Id = 1, Name = "Global Tech Distributors", ContactPerson = "John Smith", Email = "john@globaltech.com", Phone = "+1-555-0199", Address = "New York, USA" },
                new Supplier { Id = 2, Name = "Apex Office Solutions", ContactPerson = "Sarah Connor", Email = "sarah@apexoffice.com", Phone = "+1-555-0255", Address = "Chicago, USA" },
                new Supplier { Id = 3, Name = "Nexus Electronics", ContactPerson = "David Lee", Email = "david@nexusele.com", Phone = "+82-2-555-1234", Address = "Seoul, South Korea" }
            );

            modelBuilder.Entity<Warehouse>().HasData(
                new Warehouse { Id = 1, Name = "Central Distribution Center", Location = "New York", Capacity = 5000, ManagerName = "Robert Downey" },
                new Warehouse { Id = 2, Name = "Midwest Hub", Location = "Chicago", Capacity = 3000, ManagerName = "Emma Watson" },
                new Warehouse { Id = 3, Name = "West Coast Logistics", Location = "Los Angeles", Capacity = 4000, ManagerName = "Chris Evans" }
            );
        }

        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
