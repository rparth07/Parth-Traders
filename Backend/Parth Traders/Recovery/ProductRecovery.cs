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
    public static async Task ApplyApprovedCategories(ParthTradersContext db)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Recovery", "product-categories.proposed.json");
        var rows = JsonSerializer.Deserialize<List<CategoryAssignment>>(await File.ReadAllTextAsync(path), new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        if (rows.Count != 50 || rows.Select(r => r.ProductSku).Distinct().Count() != 50 || rows.Select(r => r.CategoryName).Distinct().Count() != 9)
            throw new InvalidOperationException("Approved category manifest is incomplete.");
        await using var transaction = await db.Database.BeginTransactionAsync();
        var products = await db.Products.OrderBy(p => p.ProductSku).ToListAsync();
        string BusinessHash() => Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(JsonSerializer.SerializeToUtf8Bytes(products.Select(p => new { p.ProductId, p.ProductSku, p.ProductName, p.ProductDescription, p.ProductType, p.ProductRating, p.SourceCategoryId, p.SourceSupplierId, p.SupplierId, p.PiecesPerUnit, p.SinglePieceMRP, p.UnitPrice, p.Discount, p.UnitsInStock }))));
        var before = BusinessHash();
        var originalLinks = products.Select(p => new { p.ProductSku, p.CategoryId, p.SourceCategoryId }).ToList();
        foreach (var row in rows)
        {
            var product = products.SingleOrDefault(p => p.ProductSku == row.ProductSku);
            if (product == null || product.ProductName != row.ProductName || product.SourceCategoryId != row.SourceCategoryId)
                throw new InvalidOperationException("Product identity differs from the approved manifest.");
            var category = await db.Categories.SingleOrDefaultAsync(c => c.CategoryName == row.CategoryName);
            if (product.CategoryId != null && (category == null || product.CategoryId != category.CategoryId))
                throw new InvalidOperationException("An existing category assignment would be overwritten; review it first.");
            if (category == null)
            {
                category = new CategoryDataModel { CategoryName = row.CategoryName, CategoryDescription = row.CategoryName };
                db.Categories.Add(category); await db.SaveChangesAsync();
            }
            product.CategoryId = category.CategoryId;
        }
        await db.SaveChangesAsync();
        if (BusinessHash() != before) throw new InvalidOperationException("Non-category business data changed; rolling back.");
        var backupDir = Path.Combine(Directory.GetCurrentDirectory(), ".netlify"); Directory.CreateDirectory(backupDir);
        await File.WriteAllTextAsync(Path.Combine(backupDir, "category-links-before-" + DateTime.UtcNow.ToString("yyyyMMddHHmmssfff") + ".json"), JsonSerializer.Serialize(originalLinks));
        await transaction.CommitAsync();
        Console.WriteLine("Applied 50 owner-approved category assignments across nine categories. Non-category product hash unchanged: " + before);
    }
    private sealed record CategoryAssignment(string ProductSku, string ProductName, long SourceCategoryId, string CategoryName);
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
