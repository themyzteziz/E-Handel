using E_Handel.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.Design;
using System.Linq;

namespace E_Handel.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync()
        {
            using var db = new ShopContext();

            await db.Database.MigrateAsync();

            if (!await db.Categories.AnyAsync())
            {
                var categories = new[]
                {
                    new Category { Name = "Electronics" },
                    new Category { Name = "Books" },
                    new Category { Name = "Clothes" }
                };

            await db.Categories.AddRangeAsync(categories);
            await db.SaveChangesAsync();

            Console.WriteLine("Seeded initial categories.");                       
            }

            if (!await db.Products.AnyAsync())
            {
                var defaultCategory = await db.Categories.FirstAsync();

                var products = new[]
                {
            new Product { Name = "Product A", Price = 10.0m, Stock = 100, CategoryId = defaultCategory.CategoryId },
            new Product { Name = "Product B", Price = 20.0m, Stock = 200, CategoryId = defaultCategory.CategoryId },
            new Product { Name = "Product C", Price = 30.0m, Stock = 300, CategoryId = defaultCategory.CategoryId }
        };
                await db.Products.AddRangeAsync(products);
                await db.SaveChangesAsync();

                Console.WriteLine("Seeded initial products.");
            }

            if (await db.Products.CountAsync() < 50)
            { 
                var extraProduct = new List<Product>();
                
                var defaultCategory = await db.Categories.FirstAsync();

                for (int i = 0; i < 50; i++)
                {
                    extraProduct.Add(new Product
                    {
                        Name = $"Extra Product {i + 1}",
                        Price = 15.0m + i,
                        Stock = 50 + i,
                        CategoryId = defaultCategory.CategoryId
                    });
                }

                await db.Products.AddRangeAsync(extraProduct);
                await db.SaveChangesAsync();

                Console.WriteLine("Seeded extra products for pagination.");
            }

            if (!await db.Customers.AnyAsync())
            {
                var customers = new[]
                {

            new Customer { Name = "Alice", Email = "alice@example.com", City = "Wonderland" },
            new Customer { Name = "Bob", Email = "bob@example.com", City = "Builderland" },
            new Customer { Name = "Charlie", Email = "charlie@example.com", City = "Chocolate Factory" }
        };

                await db.Customers.AddRangeAsync(customers);
                await db.SaveChangesAsync();

                Console.WriteLine("Seeded initial customers.");
            }

            if (!await db.Orders.AnyAsync())
            {
                var firstCustomer = await db.Customers.FirstAsync();
                var firstProduct = await db.Products.FirstAsync();

                var order = new Order
                {
                    CustomerId = firstCustomer.CustomerId,
                    OrderDate = DateTime.UtcNow,
                    Status = "Pending",
                    TotalAmount = firstProduct.Price * 2,
                    OrderRows = new[]
                    {
                new OrderRow
                {
                    Product = firstProduct,
                    Quantity = 2,
                    UnitPrice = firstProduct.Price
                    
                }
            }.ToList()
                };

                await db.Orders.AddAsync(order);
                await db.SaveChangesAsync();
                Console.WriteLine("Seeded initial order.");
            }                  
           
            }
        }
    }
