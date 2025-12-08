using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Handel.Models;

namespace E_Handel
{
    public class ShopContext : DbContext
    {
        public DbSet <Product> Products { get; set; } = null!;

        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<Order> Orders { get; set; } = null!;

        public DbSet<Customer> Customers { get; set; } = null!;

        public DbSet<OrderRow> OrderRows { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var DbPath = Path.Combine(Environment.CurrentDirectory, "shop.db");
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(e => {
                e.HasKey(c => c.CustomerId);

                e.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

                e.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(100);

                e.Property(c => c.City)
                .IsRequired()
                .HasMaxLength(100);
            });

            modelBuilder.Entity<Order>(e => {
                e.HasKey(o => o.OrderId);

                e.Property(o => o.OrderDate)
                .IsRequired();

                e.Property(o => o.Status)
                .IsRequired();

                e.Property(o => o.TotalAmount)
                .IsRequired();

                e.HasOne(o => o.Customer)
                    .WithMany(c => c.Orders)
                    .HasForeignKey(o => o.CustomerId);

                e.HasMany(o => o.OrderRows)
                    .WithOne()
                    .HasForeignKey("OrderId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderRow>(e =>
            {
                e.HasKey(l => l.OrderRowId);

                e.Property(l => l.Quantity)
                    .IsRequired();

                e.Property(l => l.UnitPrice)
                    .IsRequired();

                e.HasOne(l => l.Order)
                    .WithMany(o => o.OrderRows)
                    .HasForeignKey(l => l.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(l => l.Product)
                    .WithMany()
                    .HasForeignKey("ProductId")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Product>(e => {
                e.HasKey(p => p.ProductId);
                e.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);
                e.Property(p => p.Price)
                .IsRequired();
                e.Property(p => p.Stock)
                .IsRequired(); 
                e.HasOne(p => p.Category)
                .WithMany(p => p.Products)
                .HasForeignKey("CategoryId")
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Category>(e => {
                e.HasKey(c => c.CategoryId);
                e.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
                e.Property(c => c.Description)               
                .HasMaxLength(5000);
            });
        }
    }
}
