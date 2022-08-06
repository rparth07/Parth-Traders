﻿using Parth_Traders.Domain.Entity;
using Parth_Traders.Domain.RepositoryInterfaces.AdminInterfaces;
using Parth_Traders.Service.Services.Admin.AdminInterfaces;

namespace Parth_Traders.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void AddAllProducts(IEnumerable<Product> productsToAdd)
        {
            throw new NotImplementedException();
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
                var productFromRepo = _productRepository.GetProductByProductName(productName);
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
            var productToReturn = _productRepository.GetProductByProductName(productName);

            return productToReturn;
        }

        public void UpdateProduct(Product productId)
        {
            throw new NotImplementedException();
        }
    }
}