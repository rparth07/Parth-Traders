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
        public DbSet<ProductsDataModel> Products { get; set; }

        public DbSet<CustomersDataModel> Customers { get; set; }

        public DbSet<CategoryDataModel> Categories { get; set; }

        public DbSet<OrdersDataModel> Orders { get; set; }  

        public DbSet<OrderDetailsDataModel> OrderDetails { get; set; }

        public DbSet<SuppliersDataModel> Suppliers { get; set; }

        public ParthTradersContext()
        {

        }

        public ParthTradersContext(DbContextOptions<ParthTradersContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<CustomersDataModel>(entity => {
                entity.HasIndex(e => e.CustomerEmail).IsUnique();
                entity.HasIndex(e => e.CustomerPhoneNumber).IsUnique();
            });

            modelBuilder.Entity<ProductsDataModel>(entity => {
                entity.HasIndex(e => e.ProductName).IsUnique();
            });

            modelBuilder.Entity<CategoryDataModel>(entity => {
                entity.HasIndex(e => e.CategoryName).IsUnique();
            });

            modelBuilder.Entity<SuppliersDataModel>(entity => {
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
