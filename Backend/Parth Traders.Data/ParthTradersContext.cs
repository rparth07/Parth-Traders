using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Parth_Traders.Data.Configuration;
using Parth_Traders.Data.DataModel.Admin;
using Parth_Traders.Data.DataModel.User;

namespace Parth_Traders.Data
{
    public class ParthTradersContext : IdentityDbContext<AdminDataModel, IdentityRole, string>
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
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.PhoneNumber).IsUnique();
            });
            modelBuilder.Entity<OrderDataModel>(entity =>
            {
                entity.Property(x => x.OrderId).ValueGeneratedOnAdd();
                entity.HasIndex(x => x.TransactionId).IsUnique();
            });
            modelBuilder.Entity<OrderDetailDataModel>(entity =>
            {
                entity.Property(x => x.OrderDetailId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<ProductDataModel>(entity =>
            {
                entity.Property(x => x.ProductId).ValueGeneratedOnAdd();
                entity.HasIndex(e => e.ProductName).IsUnique();
                entity.HasIndex(e => e.ProductSku).IsUnique();
            });

            modelBuilder.Entity<CategoryDataModel>(entity =>
            {
                entity.Property(x => x.CategoryId).ValueGeneratedOnAdd();
                entity.HasIndex(e => e.CategoryName).IsUnique();
            });

            modelBuilder.Entity<SupplierDataModel>(entity =>
            {
                entity.Property(x => x.SupplierId).ValueGeneratedOnAdd();
                entity.HasIndex(e => e.SupplierName).IsUnique();
                entity.HasIndex(e => e.SupplierEmail).IsUnique();
                entity.HasIndex(e => e.SupplierPhoneNumber).IsUnique();
            });

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
                foreach (var property in entity.GetProperties())
                    if (property.ClrType == typeof(decimal)) { property.SetPrecision(18); property.SetScale(2); }
            modelBuilder.ApplyConfiguration(new RoleConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<PaymentDraftDataModel> PaymentDrafts { get; set; }

        public DbSet<ProductDataModel> Products { get; set; }

        public DbSet<CustomerDataModel> Customers { get; set; }

        public DbSet<CategoryDataModel> Categories { get; set; }

        public DbSet<OrderDataModel> Orders { get; set; }

        public DbSet<OrderDetailDataModel> OrderDetails { get; set; }

        public DbSet<SupplierDataModel> Suppliers { get; set; }
    }
}
