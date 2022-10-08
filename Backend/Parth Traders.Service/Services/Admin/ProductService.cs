using Parth_Traders.Domain.Entity.Admin;
using Parth_Traders.Domain.RepositoryInterfaces.AdminInterfaces;
using Parth_Traders.Service.Filter;
using Parth_Traders.Service.Services.Admin.AdminInterfaces;
using Parth_Traders.Service.Services.Logger;

namespace Parth_Traders.Service.Admin
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILoggerManager _logger;

        public ProductService(IProductRepository productRepository, ILoggerManager logger)
        {
            _productRepository = productRepository ??
                throw new ArgumentNullException(nameof(productRepository));
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        public Product AddProduct(Product productToAdd)
        {
            try
            {
                _productRepository.AddProduct(productToAdd);
                _productRepository.Save();
            }
            catch (Exception ex)
            {
                _logger.LogInfo("Entered data was in incorrect format and throwing error" +
                    $"{ex}");
                throw new BadRequestException("Please enter data in correct format!");
            }

            return GetProductByName(productToAdd.ProductName);
        }

        public List<Product> AddAllProducts(List<Product> productsToAdd)
        {
            try
            {
                _productRepository.AddAllProducts(productsToAdd);
                _productRepository.Save();
            }
            catch (Exception ex)
            {
                _logger.LogInfo("CSV file data is in incorrect format and throwing error" +
                    $"{ex}");
                throw new BadRequestException("One or more product data is in wrong format. Please check your file and enter data in correct format!");
            }

            List<Product> addedproducts = productsToAdd
                .Select(_ => GetProductByName(_.ProductName))
                .ToList();

            return addedproducts;
        }

        public Product GetProductByName(string productName)
        {
            var productToReturn = _productRepository.GetProductByName(productName);
            if (productToReturn == null)
            {
                _logger.LogInfo("Invalid product name was entered");
                throw new NotFoundException("Please enter a valid product name!");
            }
            return productToReturn;
        }

        public List<Product> GetAllProducts()
        {
            return _productRepository.GetAllProducts();
        }

        public void UpdateProduct(Product updatedProduct, string oldProductName)
        {
            var productFromRepo = GetProductByName(oldProductName);

            _productRepository
                .UpdateProduct(FillRequiredInfo(productFromRepo, updatedProduct));
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

        public void DeleteProduct(string productName)
        {
            try
            {
                var productFromRepo = GetProductByName(productName);
                _productRepository.DeleteProduct(productFromRepo);
                _productRepository.Save();
            }
            catch (Exception ex)
            {
                _logger.LogInfo("Invalid product name was entered and throwing error" +
                    $"{ex}");
                throw new NotFoundException("Please enter a valid product name!");
            }
        }

    }
}