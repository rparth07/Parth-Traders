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

        public void DeleteProduct(Product productFromRepo)
        {
            var productData = _mapper.Map<ProductDataModel>(productFromRepo);

            var productToDelete = _context.Products
                .Where(_ => _.ProductId == productData.ProductId)
                .FirstOrDefault();

            _context.OrderDetails.RemoveRange(productToDelete.OrderDetails);
            _context.Products.Remove(productToDelete);

        }

        public List<Product> GetAllProducts()
        {
            var products = _context.Products
                .Include("SupplierData")
                .Include("CategoryData")
                .Include("OrderDetails")
                .ToList();

            return _mapper.Map<List<Product>>(products);
        }

        public Product GetProductByProductName(string productName)
        {
            var prouctToReturn = _context.Products
                .AsNoTracking()
                .FirstOrDefault(_ => _.ProductName == productName);

            return _mapper.Map<Product>(prouctToReturn);
        }

        public void UpdateProduct(Product productId)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose resources when needed
            }
        }
    }
}
