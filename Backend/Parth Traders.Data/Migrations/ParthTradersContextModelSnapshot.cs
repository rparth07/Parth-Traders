﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Parth_Traders.Data;

#nullable disable

namespace Parth_Traders.Data.Migrations
{
    [DbContext(typeof(ParthTradersContext))]
    partial class ParthTradersContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Parth_Traders.Data.DataModel.Admin.CategoryDataModel", b =>
                {
                    b.Property<long>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("CategoryId"), 1L, 1);

                    b.Property<string>("CategoryDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("CategoryId");

                    b.HasIndex("CategoryName")
                        .IsUnique();

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Parth_Traders.Data.DataModel.Admin.ProductDataModel", b =>
                {
                    b.Property<long>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ProductId"), 1L, 1);

                    b.Property<long>("CategoryId")
                        .HasColumnType("bigint");

                    b.Property<long>("Discount")
                        .HasColumnType("bigint");

                    b.Property<int>("PiecesPerUnit")
                        .HasColumnType("int");

                    b.Property<string>("ProductDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ProductType")
                        .HasColumnType("int");

                    b.Property<long>("SinglePieceMRP")
                        .HasColumnType("bigint");

                    b.Property<long>("SupplierId")
                        .HasColumnType("bigint");

                    b.Property<long>("UnitPrice")
                        .HasColumnType("bigint");

                    b.Property<long>("UnitsInStock")
                        .HasColumnType("bigint");

                    b.HasKey("ProductId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ProductName")
                        .IsUnique();

                    b.HasIndex("SupplierId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Parth_Traders.Data.DataModel.Admin.SupplierDataModel", b =>
                {
                    b.Property<long>("SupplierId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("SupplierId"), 1L, 1);

                    b.Property<string>("SupplierAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SupplierEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SupplierName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SupplierPhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("SupplierId");

                    b.HasIndex("SupplierEmail")
                        .IsUnique();

                    b.HasIndex("SupplierName")
                        .IsUnique();

                    b.HasIndex("SupplierPhoneNumber")
                        .IsUnique();

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("Parth_Traders.Data.DataModel.User.CustomerDataModel", b =>
                {
                    b.Property<long>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("CustomerId"), 1L, 1);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CustomerAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerPhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("PaymentType")
                        .HasColumnType("int");

                    b.HasKey("CustomerId");

                    b.HasIndex("CustomerEmail")
                        .IsUnique();

                    b.HasIndex("CustomerPhoneNumber")
                        .IsUnique();

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Parth_Traders.Data.DataModel.User.OrderDataModel", b =>
                {
                    b.Property<long>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("OrderId"), 1L, 1);

                    b.Property<long>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<long>("GrandTotal")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("OrderStatus")
                        .HasColumnType("int");

                    b.Property<int>("PaymentType")
                        .HasColumnType("int");

                    b.HasKey("OrderId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Parth_Traders.Data.DataModel.User.OrderDetailDataModel", b =>
                {
                    b.Property<long>("OrderDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("OrderDetailId"), 1L, 1);

                    b.Property<DateTime>("BillDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("Discount")
                        .HasColumnType("bigint");

                    b.Property<long>("OrderId")
                        .HasColumnType("bigint");

                    b.Property<long>("PricePerPiece")
                        .HasColumnType("bigint");

                    b.Property<long>("ProductId")
                        .HasColumnType("bigint");

                    b.Property<long>("QuantityPurchased")
                        .HasColumnType("bigint");

                    b.Property<long>("Total")
                        .HasColumnType("bigint");

                    b.HasKey("OrderDetailId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("Parth_Traders.Data.DataModel.Admin.ProductDataModel", b =>
                {
                    b.HasOne("Parth_Traders.Data.DataModel.Admin.CategoryDataModel", "CategoryData")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Parth_Traders.Data.DataModel.Admin.SupplierDataModel", "SupplierData")
                        .WithMany("Products")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CategoryData");

                    b.Navigation("SupplierData");
                });

            modelBuilder.Entity("Parth_Traders.Data.DataModel.User.OrderDataModel", b =>
                {
                    b.HasOne("Parth_Traders.Data.DataModel.User.CustomerDataModel", "CustomerData")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CustomerData");
                });

            modelBuilder.Entity("Parth_Traders.Data.DataModel.User.OrderDetailDataModel", b =>
                {
                    b.HasOne("Parth_Traders.Data.DataModel.User.OrderDataModel", "OrderData")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Parth_Traders.Data.DataModel.Admin.ProductDataModel", "ProductData")
                        .WithMany("OrderDetails")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OrderData");

                    b.Navigation("ProductData");
                });

            modelBuilder.Entity("Parth_Traders.Data.DataModel.Admin.CategoryDataModel", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Parth_Traders.Data.DataModel.Admin.ProductDataModel", b =>
                {
                    b.Navigation("OrderDetails");
                });

            modelBuilder.Entity("Parth_Traders.Data.DataModel.Admin.SupplierDataModel", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Parth_Traders.Data.DataModel.User.CustomerDataModel", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Parth_Traders.Data.DataModel.User.OrderDataModel", b =>
                {
                    b.Navigation("OrderDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
