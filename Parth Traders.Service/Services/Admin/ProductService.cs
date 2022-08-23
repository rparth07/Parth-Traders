using Parth_Traders.Domain.Entity.Admin;
using Parth_Traders.Domain.RepositoryInterfaces.AdminInterfaces;
using Parth_Traders.Service.Filter;
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

        public Product AddProduct(Product productToAdd)
        {
            try
            {
                _productRepository.AddProduct(productToAdd);
                _productRepository.Save();
            }
            catch (Exception ex)
            {
                //temp solution
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
                throw new BadRequestException("One or more product data is in wrong format. Please enter data in correct format!");
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
                throw new NotFoundException("Please enter a valid product name!");
            }
        }

    }
}