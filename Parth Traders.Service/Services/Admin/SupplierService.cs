using Parth_Traders.Domain.Entity.Admin;
using Parth_Traders.Domain.RepositoryInterfaces.Admin;
using Parth_Traders.Service.Filter;
using Parth_Traders.Service.Services.Admin.AdminInterfaces;

namespace Parth_Traders.Service.Services.Admin
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierService(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository ??
                throw new ArgumentNullException(nameof(supplierRepository));
        }

        public Supplier AddSupplier(Supplier supplierToAdd)
        {
            try
            {
                _supplierRepository.AddSupplier(supplierToAdd);
                _supplierRepository.Save();
            }
            catch (Exception ex)
            {
                //temp solution
                throw new BadRequestException("Please enter data in correct format!");
            }

            return GetSupplierByName(supplierToAdd.SupplierName);
        }

        public List<Supplier> AddAllSuppliers(List<Supplier> suppliersToAdd)
        {
            try
            {
                _supplierRepository.AddAllSuppliers(suppliersToAdd);
                _supplierRepository.Save();
            }
            catch (Exception ex)
            {
                throw new BadRequestException("One or more supplier data is in wrong format. Please enter data in correct format!");
            }

            List<Supplier> addedSuppliers = suppliersToAdd
                .Select(_ => GetSupplierByName(_.SupplierName))
                .ToList();

            return addedSuppliers;
        }

        public Supplier GetSupplierById(long supplierId)
        {
            var supplierToReturn = _supplierRepository.GetSupplierById(supplierId);
            if (supplierToReturn == null)
            {
                throw new NotFoundException("Please enter a valid supplier id!");
            }
            return supplierToReturn;
        }

        public Supplier GetSupplierByName(string supplierName)
        {
            var supplierToReturn = _supplierRepository.GetSupplierByName(supplierName);
            if (supplierToReturn == null)
            {
                throw new NotFoundException("Please enter a valid supplier name!");
            }
            return supplierToReturn;
        }

        public List<Supplier> GetAllSuppliers()
        {
            return _supplierRepository.GetAllSuppliers();
        }

        public void UpdateSupplier(Supplier updatedSupplier, string oldSupplierName)
        {
            var supplierFromRepo = GetSupplierByName(oldSupplierName);

            _supplierRepository
                .UpdateSupplier(FillRequiredInfo(supplierFromRepo, updatedSupplier));
        }

        private static Supplier FillRequiredInfo(Supplier? supplierFromRepo,
                                                 Supplier supplier)
        {
            supplier.SupplierId = supplierFromRepo.SupplierId;
            supplier.Products = supplierFromRepo.Products;

            return supplier;
        }

        public void DeleteSupplier(string supplierName)
        {
            try
            {
                var supplierFromRepo = GetSupplierByName(supplierName);
                _supplierRepository.DeleteSupplier(supplierFromRepo);
                _supplierRepository.Save();
            }
            catch (Exception ex)
            {
                throw new NotFoundException("Please enter a valid supplier name!");
            }
        }
    }
}
