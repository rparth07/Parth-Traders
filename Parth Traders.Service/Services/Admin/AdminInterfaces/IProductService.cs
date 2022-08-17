using Parth_Traders.Domain.Entity.Admin;

namespace Parth_Traders.Service.Services.Admin.AdminInterfaces
{
    public interface IProductService
    {
        Product AddProduct(Product productToAdd);
        List<Product> AddAllProducts(List<Product> productsToAdd);
        Product GetProductByProductName(string productName);
        List<Product> GetAllProducts();
        void UpdateProduct(Product updatedProduct, string oldProductName);
        void DeleteProduct(string productName);
    }
}
