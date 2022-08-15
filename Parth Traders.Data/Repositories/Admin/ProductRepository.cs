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

        public void AddProduct(Product product)
        {
            var productToAdd = _mapper.Map<ProductDataModel>(product);

            _context.Products.Add(productToAdd);
        }

        public void AddAllProducts(IEnumerable<Product> products)
        {
            var productListToAdd = _mapper.Map<List<ProductDataModel>>(products);

            _context.Products.AddRange(productListToAdd);
        }

        public Product GetProductByProductName(string productName)
        {
            var prouctToReturn = _context.Products
                .AsNoTracking()
                .Include(_ => _.SupplierData)
                .Include(_ => _.CategoryData)
                .Include(_ => _.OrderDetails).ThenInclude(_ => _.OrderData).ThenInclude(_ => _.CustomerData)
                .Include(_ => _.OrderDetails)
                .FirstOrDefault(_ => _.ProductName == productName);

            return _mapper.Map<Product>(prouctToReturn);
        }

        public List<Product> GetAllProducts()
        {
            var products = _context.Products
                .Include(_ => _.SupplierData)
                .Include(_ => _.CategoryData)
                .Include(_ => _.OrderDetails).ThenInclude(_ => _.OrderData).ThenInclude(_ => _.CustomerData)
                .Include(_ => _.OrderDetails)
                .ToList();

            return _mapper.Map<List<Product>>(products);
        }

        public void UpdateProduct(Product product, string oldProductName)
        {
            //TODO:For now, admin can not update category, supplier and orderDetails
            var productFromRepo = GetProductByProductName(oldProductName);

            var productToUpdate = FillRequiredInfo(productFromRepo, product);
            _context.Products.Update(_mapper.Map<ProductDataModel>(productToUpdate));

            Save();
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

        public void DeleteProduct(Product product)
        {
            var productToDelete = _mapper.Map<ProductDataModel>(product);

            _context.OrderDetails.RemoveRange(productToDelete.OrderDetails);
            _context.Products.Remove(productToDelete);
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
