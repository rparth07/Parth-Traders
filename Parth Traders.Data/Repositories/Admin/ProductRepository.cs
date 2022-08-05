using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Parth_Traders.Data.DataModel;
using Parth_Traders.Domain.Entity;
using Parth_Traders.Domain.RepositoryInterfaces.AdminInterfaces;

namespace Parth_Traders.Data.Repositories.Admin
{
    public class ProductRepository : IProductRepository, IDisposable
    {
        private readonly ParthTradersContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(ParthTradersContext context, IMapper mapper)
        {
            _context = context ?? 
                throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? 
                throw new ArgumentNullException(nameof(mapper));
        }

        public void AddProduct(Product productToAdd)
        {
            var productDataToAdd = _mapper.Map<ProductDataModel>(productToAdd);

            _context.Products.Add(productDataToAdd);
        }

        public void AddAllProducts(IEnumerable<Product> productsToAdd)
        {
            throw new NotImplementedException();
        }

        public void DeleteProduct(string productName)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAllProducts()
        {
            throw new NotImplementedException();
        }

        public Product GetProductByProductName(string productName)
        {
            var prouctToReturn = _context.Products
                .AsNoTracking()
                .FirstOrDefault(_ => _.ProductName == productName);

            return _mapper.Map<Product>(prouctToReturn);
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateProduct(Product productId)
        {
            throw new NotImplementedException();
        }
    }
}
