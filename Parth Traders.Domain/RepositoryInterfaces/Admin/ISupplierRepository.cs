using Parth_Traders.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Domain.RepositoryInterfaces.Admin
{
    public interface ISupplierRepository
    {
        void AddSupplier(Supplier supplierToAdd);
        void AddSupppliers(List<Supplier> suppliersToAdd);
        Supplier GetSupplierById(int id);
        List<Supplier> GetAllSuppliers();
        void UpdateSuppplier(Supplier supplierToUpdate);
        void DeleteSupplier(Supplier supplierToRemove);
        bool Save();
        void Dispose();

    }
}
