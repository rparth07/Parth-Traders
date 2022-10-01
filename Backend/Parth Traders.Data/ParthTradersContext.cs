using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Parth_Traders.Data.Configuration;
using Parth_Traders.Data.DataModel.Admin;
using Parth_Traders.Data.DataModel.User;

namespace Parth_Traders.Data
{
    public class ParthTradersContext : IdentityDbContext<AdminUserDataModel>
    {
        public ParthTradersContext()
        {

        }

        public ParthTradersContext(DbContextOptions<ParthTradersContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerDataModel>(entity =>
            {
                entity.Property(x => x.CustomerId).ValueGeneratedOnAdd().UseIdentityColumn(1, 1);
                entity.HasIndex(e => e.CustomerEmail).IsUnique();
                entity.HasIndex(e => e.CustomerPhoneNumber).IsUnique();
            });
            modelBuilder.Entity<OrderDataModel>(entity =>
            {
                entity.Property(x => x.OrderId).ValueGeneratedOnAdd().UseIdentityColumn(1, 1);
            });
            modelBuilder.Entity<OrderDetailDataModel>(entity =>
            {
                entity.Property(x => x.OrderDetailId).ValueGeneratedOnAdd().UseIdentityColumn(1, 1);
            });

            modelBuilder.Entity<ProductDataModel>(entity =>
            {
                entity.Property(x => x.ProductId).ValueGeneratedOnAdd().UseIdentityColumn(1, 1);
                entity.HasIndex(e => e.ProductName).IsUnique();
            });

            modelBuilder.Entity<CategoryDataModel>(entity =>
            {
                entity.Property(x => x.CategoryId).ValueGeneratedOnAdd().UseIdentityColumn(1, 1);
                entity.HasIndex(e => e.CategoryName).IsUnique();
            });

            modelBuilder.Entity<SupplierDataModel>(entity =>
            {
                entity.Property(x => x.SupplierId).ValueGeneratedOnAdd().UseIdentityColumn(1, 1);
                entity.HasIndex(e => e.SupplierName).IsUnique();
                entity.HasIndex(e => e.SupplierEmail).IsUnique();
                entity.HasIndex(e => e.SupplierPhoneNumber).IsUnique();
            });
            
            modelBuilder.ApplyConfiguration(new RoleConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.EnableSensitiveDataLogging();
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server=PARTH\\SQLEXPRESS; Database=Parth Traders; Integrated Security=true;");
            }
        }

        public DbSet<ProductDataModel> Products { get; set; }

        public DbSet<CustomerDataModel> Customers { get; set; }

        public DbSet<CategoryDataModel> Categories { get; set; }

        public DbSet<OrderDataModel> Orders { get; set; }

        public DbSet<OrderDetailDataModel> OrderDetails { get; set; }

        public DbSet<SupplierDataModel> Suppliers { get; set; }
    }
}
