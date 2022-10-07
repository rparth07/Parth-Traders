using Parth_Traders.Domain.Entity.Admin;
using Parth_Traders.Domain.RepositoryInterfaces.Admin;
using Parth_Traders.Service.Filter;
using Parth_Traders.Service.Services.Admin.AdminInterfaces;
using Parth_Traders.Service.Services.Logger;

namespace Parth_Traders.Service.Services.Admin
{
    public class ProductHelperService : IProductHelperService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILoggerManager _logger;

        public ProductHelperService(ISupplierRepository supplierRepository,
                                    ICategoryRepository categoryRepository,
                                    ILoggerManager logger)
        {
            _supplierRepository = supplierRepository ??
                throw new ArgumentNullException(nameof(supplierRepository));
            _categoryRepository = categoryRepository ??
                throw new ArgumentNullException(nameof(categoryRepository));
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        public Product MapProductPropertiesToProduct(Product product)
        {
            product.Supplier = _supplierRepository
                .GetSupplierByName(product.Supplier.SupplierName);

            product.Category = _categoryRepository
                    .GetCategoryByName(product.Category.CategoryName);

            if (product.Category == null ||
                product.Supplier == null)
            {
                _logger.LogInfo("Invalid supplier or category name, not able to map properties!");
                throw new BadRequestException("Please enter data in correct format!");
            }

            return product;
        }
    }
}
