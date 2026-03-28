using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Data;

/// <summary>
/// Seeds sample products and categories into the database.
/// This implementation performs idempotent upserts: it will add missing categories/products
/// and update description/IsActive for existing items. Safe to run on startup.
/// </summary>
public static class SeedData
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        var seedCategories = new List<Category>
        {
            new Category { Name = "Electronics", Description = "Electronic devices and gadgets", IsActive = true },
            new Category { Name = "Clothing", Description = "Apparel and fashion items", IsActive = true },
            new Category { Name = "Home & Garden", Description = "Home improvement and garden supplies", IsActive = true },
            new Category { Name = "Books", Description = "Books and educational materials", IsActive = true },
            new Category { Name = "Sports", Description = "Sports equipment and accessories", IsActive = true }
        };

            // Upsert categories
        foreach (var seedCat in seedCategories)
        {
            var existing = await context.Categories
                .FirstOrDefaultAsync(c => c.Name == seedCat.Name);

            if (existing == null)
            {
                seedCat.CreatedAt = DateTime.UtcNow;
                context.Categories.Add(seedCat);
            }
            else
            {
                existing.Description = seedCat.Description;
                existing.IsActive = seedCat.IsActive;
                existing.UpdatedAt = DateTime.UtcNow;
            }
        }

        await context.SaveChangesAsync();

        // Prepare product seeds using category names so IDs are resolved dynamically
        var productSeeds = new[]
        {
            new { Name = "Wireless Bluetooth Headphones", Description = "High-quality noise-cancelling headphones with 30hr battery", Price = 79.99m, Stock = 100, Category = "Electronics", Image = "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=800&q=80" },
            new { Name = "Smart Watch Pro", Description = "Fitness tracking smartwatch with GPS and heart rate monitor", Price = 199.99m, Stock = 50, Category = "Electronics", Image = "https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=800&q=80" },
            new { Name = "USB-C Hub", Description = "7-in-1 USB-C Hub with HDMI, USB 3.0, and SD card reader", Price = 49.99m, Stock = 200, Category = "Electronics", Image = "https://images.unsplash.com/photo-1563991655280-cb95c90ca2fb?w=800&q=80" },
            new { Name = "Gaming Mouse", Description = "High precision RGB gaming mouse", Price = 59.99m, Stock = 120, Category = "Electronics", Image = "https://images.unsplash.com/photo-1587202372775-9897d3d2f4b3?w=800&q=80" },
            new { Name = "Mechanical Keyboard", Description = "Backlit mechanical keyboard with blue switches", Price = 89.99m, Stock = 80, Category = "Electronics", Image = "https://images.unsplash.com/photo-1517336714739-489689fd1ca8?w=800&q=80" },
            new { Name = "Portable SSD 1TB", Description = "High-speed external solid state drive", Price = 149.99m, Stock = 60, Category = "Electronics", Image = "https://images.unsplash.com/photo-1593642532973-d31b6557fa68?w=800&q=80" },

            new { Name = "Cotton T-Shirt", Description = "Premium 100% organic cotton t-shirt", Price = 24.99m, Stock = 500, Category = "Clothing", Image = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=800&q=80" },
            new { Name = "Denim Jeans", Description = "Classic fit stretch denim jeans", Price = 59.99m, Stock = 150, Category = "Clothing", Image = "https://images.unsplash.com/photo-1541099649105-f69ad21f3246?w=800&q=80" },
            new { Name = "Winter Jacket", Description = "Waterproof insulated winter jacket", Price = 129.99m, Stock = 75, Category = "Clothing", Image = "https://images.unsplash.com/photo-1591047139829-d91aecb6caea?w=800&q=80" },
            new { Name = "Hoodie Sweatshirt", Description = "Comfortable fleece hoodie for everyday wear", Price = 49.99m, Stock = 200, Category = "Clothing", Image = "https://images.unsplash.com/photo-1556821840-3a63f95609a7?w=800&q=80" },
            new { Name = "Formal Shirt", Description = "Slim fit formal shirt for office wear", Price = 39.99m, Stock = 180, Category = "Clothing", Image = "https://images.unsplash.com/photo-1521335629791-ce4aec67dd47?w=800&q=80" },
            new { Name = "Sneakers", Description = "Casual sneakers with breathable fabric", Price = 69.99m, Stock = 140, Category = "Clothing", Image = "https://images.unsplash.com/photo-1528701800489-20be3c4e94e5?w=800&q=80" },

            new { Name = "Indoor Plant Set", Description = "Set of 3 low-maintenance indoor plants", Price = 34.99m, Stock = 80, Category = "Home & Garden", Image = "https://images.unsplash.com/photo-1485955900006-10f4d324d411?w=800&q=80" },
            new { Name = "LED Desk Lamp", Description = "Adjustable LED desk lamp with USB charging", Price = 45.99m, Stock = 120, Category = "Home & Garden", Image = "https://images.unsplash.com/photo-1534073828943-f801091bb18c?w=800&q=80" },
            new { Name = "Wall Clock", Description = "Modern minimalist wall clock", Price = 19.99m, Stock = 150, Category = "Home & Garden", Image = "https://images.unsplash.com/photo-1503602642458-232111445657?w=800&q=80" },
            new { Name = "Coffee Maker", Description = "Automatic drip coffee maker", Price = 79.99m, Stock = 70, Category = "Home & Garden", Image = "https://images.unsplash.com/photo-1511920170033-f8396924c348?w=800&q=80" },
            new { Name = "Ceramic Dinner Set", Description = "Elegant 16-piece dinnerware set", Price = 99.99m, Stock = 90, Category = "Home & Garden", Image = "https://images.unsplash.com/photo-1604908176997-431b3a0c85fa?w=800&q=80" },

            new { Name = "Programming Cookbook", Description = "Clean Code and design patterns guide", Price = 39.99m, Stock = 90, Category = "Books", Image = "https://images.unsplash.com/photo-1516259762381-22954d7d3ad2?w=800&q=80" },
            new { Name = "Data Structures Book", Description = "Comprehensive guide to data structures", Price = 44.99m, Stock = 110, Category = "Books", Image = "https://images.unsplash.com/photo-1512820790803-83ca734da794?w=800&q=80" },
            new { Name = "AI Basics Guide", Description = "Introduction to Artificial Intelligence concepts", Price = 49.99m, Stock = 95, Category = "Books", Image = "https://images.unsplash.com/photo-1524995997946-a1c2e315a42f?w=800&q=80" },
            new { Name = "Web Development Handbook", Description = "HTML, CSS, JavaScript full guide", Price = 34.99m, Stock = 130, Category = "Books", Image = "https://images.unsplash.com/photo-1498050108023-c5249f4df085?w=800&q=80" },

            new { Name = "Running Shoes", Description = "Lightweight running shoes for all terrains", Price = 89.99m, Stock = 60, Category = "Sports", Image = "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=800&q=80" },
            new { Name = "Yoga Mat", Description = "Non-slip eco-friendly yoga mat", Price = 29.99m, Stock = 200, Category = "Sports", Image = "https://images.unsplash.com/photo-1544367567-0f2fcb009e0b?w=800&q=80" },
            new { Name = "Dumbbell Set", Description = "Adjustable dumbbells for home workouts", Price = 59.99m, Stock = 75, Category = "Sports", Image = "https://images.unsplash.com/photo-1517964603305-11c0f6f66012?w=800&q=80" },
            new { Name = "Football", Description = "Professional size 5 football", Price = 25.99m, Stock = 140, Category = "Sports", Image = "https://images.unsplash.com/photo-1517649763962-0c623066013b?w=800&q=80" },
            new { Name = "Skipping Rope", Description = "Adjustable fitness skipping rope", Price = 14.99m, Stock = 200, Category = "Sports", Image = "https://images.unsplash.com/photo-1599058917765-a780eda07a3e?w=800&q=80" }
        };

        // Upsert products
        foreach (var p in productSeeds)
        {
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Name == p.Category);
            if (category == null)
            {
                // skip products for missing category (should not happen because categories were upserted above)
                continue;
            }

            var existingProduct = await context.Products
                .FirstOrDefaultAsync(x => x.Name == p.Name);

            if (existingProduct == null)
            {
                var newProduct = new Product
                {
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.Stock,
                    CategoryId = category.Id,
                    ImageUrl = p.Image,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };
                context.Products.Add(newProduct);
            }
            else
            {
                existingProduct.Description = p.Description;
                existingProduct.Price = p.Price;
                existingProduct.StockQuantity = p.Stock;
                existingProduct.CategoryId = category.Id;
                existingProduct.ImageUrl = p.Image;
                existingProduct.IsActive = true;
                existingProduct.UpdatedAt = DateTime.UtcNow;
            }
        }

        await context.SaveChangesAsync();
    }
}
