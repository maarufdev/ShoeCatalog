using Microsoft.EntityFrameworkCore;
using ShoeCatalog.DataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeCatalog.DataModels.Data
{
    public class ShoeDbContext : DbContext
    {
        public ShoeDbContext(DbContextOptions<ShoeDbContext> options): base(options)
        {
            
        }

        public DbSet<Shoe> Shoes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ShoeCategory> ShoeCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShoeCategory>()
                 .HasKey(sc => new { sc.ShoeId, sc.CategoryId });

            modelBuilder.Entity<ShoeCategory>()
                .HasOne(sc => sc.Shoe)
                .WithMany(s => s.ShoeCategories)
                .HasForeignKey(sc => sc.ShoeId);

            modelBuilder.Entity<ShoeCategory>()
            .HasOne(sc => sc.Category)
            .WithMany(c => c.ShoeCategories)
            .HasForeignKey(sc => sc.CategoryId);
        }

    }
}
