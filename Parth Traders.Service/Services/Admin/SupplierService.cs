using Parth_Traders.Domain.Entity.Admin;
using Parth_Traders.Domain.RepositoryInterfaces.Admin;
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
                throw new Exception($"Please enter data in correct format.{ex}");
            }

            return supplierToAdd;
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
                throw new Exception($"Bad Request {ex}");
            }

            return suppliersToAdd;
        }

        public Supplier GetSupplierById(long supplierId)
        {
            var supplierToReturn = _supplierRepository.GetSupplierById(supplierId);
            if (supplierToReturn == null)
            {
                throw new Exception("Supplier not found!");
            }
            return supplierToReturn;
        }

        public Supplier GetSupplierByName(string supplierName)
        {
            var supplierToReturn = _supplierRepository.GetSupplierByName(supplierName);
            if (supplierToReturn == null)
            {
                throw new Exception("supplier not found!");
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
                var supplierFromRepo = _supplierRepository.GetSupplierByName(supplierName);
                _supplierRepository.DeleteSupplier(supplierFromRepo);
                _supplierRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Supplier not found!");
            }
        }
    }
}
