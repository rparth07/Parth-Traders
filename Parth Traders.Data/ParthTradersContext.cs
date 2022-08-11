using Microsoft.EntityFrameworkCore;
using Parth_Traders.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Data
{
    public class ParthTradersContext : DbContext
    {
        public DbSet<ProductDataModel> Products { get; set; }

        public DbSet<CustomerDataModel> Customers { get; set; }

        public DbSet<CategoryDataModel> Categories { get; set; }

        public DbSet<OrderDataModel> Orders { get; set; }  

        public DbSet<OrderDetailDataModel> OrderDetails { get; set; }

        public DbSet<SupplierDataModel> Suppliers { get; set; }

        public ParthTradersContext()
        {

        }

        public ParthTradersContext(DbContextOptions<ParthTradersContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<CustomerDataModel>(entity => {
                entity.HasIndex(e => e.CustomerEmail).IsUnique();
                entity.HasIndex(e => e.CustomerPhoneNumber).IsUnique();
            });

            modelBuilder.Entity<ProductDataModel>(entity => {
                entity.HasIndex(e => e.ProductName).IsUnique();
            });

            modelBuilder.Entity<CategoryDataModel>(entity => {
                entity.HasIndex(e => e.CategoryName).IsUnique();
            });

            modelBuilder.Entity<SupplierDataModel>(entity => {
                entity.HasIndex(e => e.SupplierName).IsUnique();
                entity.HasIndex(e => e.SupplierEmail).IsUnique();
                entity.HasIndex(e => e.SupplierPhoneNumber).IsUnique();
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.EnableSensitiveDataLogging();
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server=PARTH\\SQLEXPRESS; Database=Parth Traders; Integrated Security=true;");
            }
        }
    }
}
