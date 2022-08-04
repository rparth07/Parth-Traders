using Parth_Traders.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Service.Services.Admin.AdminInterfaces
{
    public interface IProductService
    {
        Product AddProduct(Product productToAdd);
        void AddAllProducts(IEnumerable<Product> productsToAdd);
        Product GetProductByProductName(string productName);
        List<Product> GetAllProducts();
        public void UpdateProduct(Product productId);
        void DeleteProduct(string productName);
    }
}
