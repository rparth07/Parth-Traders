using Parth_Traders.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Service.Services.Admin.AdminInterfaces
{
    public interface ISupplierService
    {
        Supplier AddSupplier(Supplier supplierToAdd);
        List<Supplier> AddAllSuppliers(List<Supplier> suppliersToAdd);
        Supplier GetSupplierById(int id);
        Supplier GetSupplierByName(string supplierName);
        List<Supplier> GetAllSuppliers();
        void UpdateSupplier(Supplier updatedSupplier, string oldSupplierName);
        void DeleteSupplier(string supplierName);
    }
}
