using Parth_Traders.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Domain.RepositoryInterfaces.AdminInterfaces
{
    public interface IProductRepository
    {
        void AddProduct(Product productToAdd);
        void AddAllProducts(IEnumerable<Product> productsToAdd);
        Product GetProductByProductName(string productName);
        List<Product> GetAllProducts();
        void UpdateProduct(Product productId);
        void DeleteProduct(string productName);
        bool Save();
        void Dispose();
    }
}
