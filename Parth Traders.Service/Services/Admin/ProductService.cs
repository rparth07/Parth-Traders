using Parth_Traders.Domain.Entity;
using Parth_Traders.Domain.RepositoryInterfaces.Admin;
using Parth_Traders.Domain.RepositoryInterfaces.AdminInterfaces;
using Parth_Traders.Service.Services.Admin.AdminInterfaces;

namespace Parth_Traders.Service.Admin
{
    //TODO: Need to create exception file and use it in catch
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
       
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository ??
                throw new ArgumentNullException(nameof(productRepository));
        }

        public List<Product> AddAllProducts(List<Product> productsToAdd)
        {
            try
            {
                _productRepository.AddAllProducts(productsToAdd);
                _productRepository.Save();
            } 
            catch(Exception ex)
            {
                throw new Exception($"Bad Request {ex}");
            }

            return productsToAdd;
        }

        public Product AddProduct(Product productToAdd)
        {
            try
            {
                _productRepository.AddProduct(productToAdd);
                _productRepository.Save();
            }
            catch(Exception ex)
            {
                //temp solution
                throw new Exception($"Please enter data in correct format.{ex}");
            }

            return productToAdd;
        }

        public void DeleteProduct(string productName)
        {
            try
            {
                var productFromRepo = _productRepository.GetProductByName(productName);
                _productRepository.DeleteProduct(productFromRepo);
                _productRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Product not found!");
            }
        }

        public List<Product> GetAllProducts()
        {
            return _productRepository.GetAllProducts();
        }

        public Product GetProductByProductName(string productName)
        {
            var productToReturn = _productRepository.GetProductByName(productName);
            if(productToReturn == null)
            {
                throw new Exception("product not found!");
            }
            return productToReturn;
        }

        public void UpdateProduct(Product updatedProduct, string oldProductName)
        {
            var productFromRepo = GetProductByProductName(oldProductName);
            
            _productRepository
                .UpdateProduct(FillRequiredInfo(productFromRepo,updatedProduct));
        }

        private static Product FillRequiredInfo(Product? productFromRepo,
                                                Product product)
        {
            product.ProductId = productFromRepo.ProductId;
            product.Category = productFromRepo.Category;
            product.Supplier = productFromRepo.Supplier;
            product.OrderDetails = productFromRepo.OrderDetails;

            return product;
        }

    }
}