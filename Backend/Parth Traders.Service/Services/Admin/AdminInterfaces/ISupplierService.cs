using Parth_Traders.Domain.Entity.Admin;

namespace Parth_Traders.Service.Services.Admin.AdminInterfaces
{
    public interface ISupplierService
    {
        Supplier AddSupplier(Supplier supplierToAdd);
        List<Supplier> AddAllSuppliers(List<Supplier> suppliersToAdd);
        Supplier GetSupplierById(long supplierId);
        Supplier GetSupplierByName(string supplierName);
        List<Supplier> GetAllSuppliers();
        void UpdateSupplier(Supplier updatedSupplier, string oldSupplierName);
        void DeleteSupplier(string supplierName);
    }
}
