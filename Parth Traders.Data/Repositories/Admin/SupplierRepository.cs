using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Parth_Traders.Data.DataModel;
using Parth_Traders.Domain.Entity;
using Parth_Traders.Domain.RepositoryInterfaces.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Data.Repositories.Admin
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly ParthTradersContext _context;
        private readonly IMapper _mapper;

        public SupplierRepository(ParthTradersContext context, IMapper mapper)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public void AddSupplier(Supplier supplier)
        {
            var supplierToAdd = _mapper.Map<SupplierDataModel>(supplier);

            _context.Suppliers.Add(supplierToAdd);
        }

        public void AddSupppliers(List<Supplier> suppliers)
        {
            var supplierListToAdd = _mapper.Map<List<SupplierDataModel>>(suppliers);

            _context.Suppliers.AddRange(supplierListToAdd);
        }

        public Supplier GetSupplierById(int id)
        {
            var supplier = _context.Suppliers
                .FirstOrDefault(_ => _.SupplierId == id);

            return _mapper.Map<Supplier>(supplier);
        }

        public Supplier GetSupplierByName(string supplierName)
        {
            var supplier = _context.Suppliers
                .AsNoTracking()
                .FirstOrDefault(_ => _.SupplierName == supplierName);

            return _mapper.Map<Supplier>(supplier);
        }

        public List<Supplier> GetAllSuppliers()
        {
           var suppliers = _context.Suppliers
                .Include("Products")
                .ToList();

            return _mapper.Map<List<Supplier>>(suppliers);
        }

        public void UpdateSuppplier(Supplier supplier)
        {
            var supplierToUpdate = _mapper.Map<SupplierDataModel>(supplier);

            supplierToUpdate.SupplierId = _context.Suppliers
                .FirstOrDefault(_ => _.SupplierName == supplierToUpdate.SupplierName)
                .SupplierId;

            _context.Suppliers.Update(supplierToUpdate);

            Save();
        }

        public void DeleteSupplier(Supplier supplier)
        {
            var supplierToDelete = _context.Suppliers
                .FirstOrDefault(_ => _.SupplierName == supplier.SupplierName);

            _context.Products.RemoveRange(supplierToDelete.Products);
            _context.Suppliers.Remove(supplierToDelete);
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
