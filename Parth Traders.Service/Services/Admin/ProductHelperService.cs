using Parth_Traders.Domain.Entity.Admin;
using Parth_Traders.Domain.RepositoryInterfaces.Admin;
using Parth_Traders.Service.Services.Admin.AdminInterfaces;

namespace Parth_Traders.Service.Services.Admin
{
    public class ProductHelperService : IProductHelperService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductHelperService(ISupplierRepository supplierRepository,
                                    ICategoryRepository categoryRepository)
        {
            _supplierRepository = supplierRepository ??
                throw new ArgumentNullException(nameof(supplierRepository));
            _categoryRepository = categoryRepository ??
                throw new ArgumentNullException(nameof(categoryRepository));
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
                throw new Exception("Data is wrong!");
            }

            return product;
        }
    }
}
