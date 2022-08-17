﻿using Parth_Traders.Domain.Entity.Admin;

namespace Parth_Traders.Domain.RepositoryInterfaces.Admin
{
    public interface ISupplierRepository
    {
        void AddSupplier(Supplier supplierToAdd);
        void AddAllSuppliers(List<Supplier> suppliersToAdd);
        Supplier GetSupplierById(long supplierId);
        Supplier GetSupplierByName(string supplierName);
        List<Supplier> GetAllSuppliers();
        void UpdateSupplier(Supplier supplierToUpdate);
        void DeleteSupplier(Supplier supplierToRemove);
        bool Save();
        void Dispose();

    }
}
