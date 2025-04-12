using Microsoft.EntityFrameworkCore;
using StoreManagementSystem.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace StoreManagementSystem.Datas
{
    public class StoreDbContext
    {
          


        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed initial data
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Electronics", Description = "Electronic devices and components", DisplayOrder = 1 },
                new Category { CategoryId = 2, Name = "Clothing", Description = "Apparel and accessories", DisplayOrder = 2 },
                new Category { CategoryId = 3, Name = "Home & Garden", Description = "Home improvement and gardening", DisplayOrder = 3 }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, Name = "Smartphone", Description = "Latest model smartphone", Price = 699.99m, StockQuantity = 50, CategoryId = 1 },
                new Product { ProductId = 2, Name = "Laptop", Description = "High-performance laptop", Price = 1299.99m, StockQuantity = 25, CategoryId = 1 },
                new Product { ProductId = 3, Name = "T-Shirt", Description = "Cotton t-shirt", Price = 19.99m, StockQuantity = 100, CategoryId = 2 }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}

    }
}
