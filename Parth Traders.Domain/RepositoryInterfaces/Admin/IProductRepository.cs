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
        void AddAllProducts(List<Product> productsToAdd);
        Product GetProductByName(string productName);
        List<Product> GetAllProducts();
        void UpdateProduct(Product product);
        void DeleteProduct(Product productToDelete);
        bool Save();
        void Dispose();
    }
}
