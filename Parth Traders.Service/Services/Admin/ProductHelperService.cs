using Parth_Traders.Domain.Entity;
using Parth_Traders.Domain.RepositoryInterfaces.Admin;
using Parth_Traders.Service.Services.Admin.AdminInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Service.Services.Admin
{
    public class ProductHelperService : IProductHelperService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductHelperService(ISupplierRepository supplierRepository,
                                    ICategoryRepository categoryRepository)
        {
            _supplierRepository = supplierRepository;
            _categoryRepository = categoryRepository;
        }

        public Product MapProductPropertiesToProduct(Product product, string supplierName, string categoryName)
        {
            product.SupplierData = _supplierRepository
                .GetSupplierByName(supplierName);

            product.CategoryData = _categoryRepository
                    .GetCategoryByName(categoryName);

            if(product.CategoryData == null || 
               product.SupplierData == null)
            {
                throw new Exception("Data is wrong!");
            }

            return product;
        }
    }
}
