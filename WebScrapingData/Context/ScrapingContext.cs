using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using WebScrapingData.Model;

namespace WebScrapingData.Context
{
    public class ScrapingContext : DbContext
    {
        public ScrapingContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "scraping.db");
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public string DbPath { get; }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={DbPath}");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>()
                .HasIndex(p => p.ProductAsin)
                .IsUnique();

            builder.Entity<Review>()
                .HasOne(p => p.Product)
                .WithMany(b => b.Reviews);

            builder.Entity<Product>()
                .HasData(
                    new List<Product>
                    {
                        new()
                        {
                            IdProduct = 1,
                            ProductName = "Samsung Galaxy S21",
                            ProductAsin = "B082XY23D5",
                            Enable = true
                        },
                        new()
                        {
                            IdProduct = 2,
                            ProductName = "Logitech MX Keys",
                            ProductAsin = "B07S92QBCJ",
                            Enable = true
                        }
                    }
                );
        }
    }
}