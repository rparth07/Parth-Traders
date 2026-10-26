using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Parth_Traders.Data;
using Parth_Traders.Data.DataModel.Admin;
using Parth_Traders.Domain.Enums;
namespace Parth_Traders.Recovery;
public static class ProductRecovery
{
    public static async Task Import(ParthTradersContext db)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Recovery", "products-source.json");
        var rows = JsonSerializer.Deserialize<List<SourceProduct>>(await File.ReadAllTextAsync(path))!;
        await using var transaction = await db.Database.BeginTransactionAsync();
        var added = 0;
        foreach (var row in rows)
        {
            if (await db.Products.AnyAsync(p => p.ProductSku == row.ProductSku)) continue;
            db.Products.Add(new ProductDataModel {
                ProductName = row.ProductName, ProductSku = row.ProductSku, ProductDescription = row.ProductDescription,
                ProductType = row.ProductType, ProductRating = row.ProductRating, PiecesPerUnit = row.PiecesPerUnit,
                SinglePieceMRP = row.SinglePieceMRP, UnitPrice = row.UnitPrice, Discount = row.Discount, UnitsInStock = row.UnitsInStock,
                SourceSupplierId = row.SupplierId, SourceCategoryId = row.CategoryId,
                SupplierId = await db.Suppliers.AnyAsync(s => s.SupplierId == row.SupplierId) ? row.SupplierId : null,
                CategoryId = await db.Categories.AnyAsync(c => c.CategoryId == row.CategoryId) ? row.CategoryId : null
            }); added++;
        }
        await db.SaveChangesAsync(); await transaction.CommitAsync();
        Console.WriteLine($"Imported {added} approved SQL products; existing SKUs were preserved.");
    }
    public static async Task BootstrapAdmin(IServiceProvider services, IConfiguration config)
    {
        var name = config["BootstrapAdmin:userName"];
        var password = config["BootstrapAdmin:password"];
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(password)) return;
        var users = services.GetRequiredService<UserManager<AdminDataModel>>();
        if (await users.FindByNameAsync(name) != null) return;
        var admin = new AdminDataModel { UserName = name, Email = config["BootstrapAdmin:email"] ?? "parth-admin@admin.invalid", FirstName = "Store", LastName = "Administrator" };
        var result = await users.CreateAsync(admin, password);
        if (!result.Succeeded) throw new InvalidOperationException("Could not create bootstrap administrator. Check password policy.");
        var roleResult = await users.AddToRoleAsync(admin, "Administrator");
        if (!roleResult.Succeeded) throw new InvalidOperationException("Could not assign administrator role.");
    }
    private sealed record SourceProduct(string ProductName, string ProductSku, string ProductDescription, ProductType ProductType,
        double ProductRating, long SupplierId, long CategoryId, int PiecesPerUnit, decimal SinglePieceMRP, decimal UnitPrice, decimal Discount, long UnitsInStock);
}
