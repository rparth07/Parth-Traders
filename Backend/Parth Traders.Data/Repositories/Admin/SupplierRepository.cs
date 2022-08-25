using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Parth_Traders.Data.DataModel.Admin;
using Parth_Traders.Domain.Entity.Admin;
using Parth_Traders.Domain.RepositoryInterfaces.Admin;

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

        public void AddAllSuppliers(List<Supplier> suppliers)
        {
            var supplierListToAdd = _mapper.Map<List<SupplierDataModel>>(suppliers);

            _context.Suppliers.AddRange(supplierListToAdd);
        }

        public Supplier GetSupplierById(long supplierId)
        {
            var supplier = _context.Suppliers
                .Include("Products")
                .AsNoTracking()
                .FirstOrDefault(_ => _.SupplierId == supplierId);

            return _mapper.Map<Supplier>(supplier);
        }

        public Supplier GetSupplierByName(string supplierName)
        {
            var supplier = _context.Suppliers
                .Include("Products")
                .AsNoTracking()
                .FirstOrDefault(_ => _.SupplierName == supplierName);

            return _mapper.Map<Supplier>(supplier);
        }

        public List<Supplier> GetAllSuppliers()
        {
            var suppliers = _context.Suppliers
                 .Include("Products")
                 .AsNoTracking()
                 .ToList();

            return _mapper.Map<List<Supplier>>(suppliers);
        }

        public void UpdateSupplier(Supplier supplier)
        {
            var supplierToUpdate = _mapper.Map<SupplierDataModel>(supplier);

            _context.Suppliers.Update(supplierToUpdate);

            Save();
        }

        public void DeleteSupplier(Supplier supplier)
        {
            var supplierToDelete = _mapper.Map<SupplierDataModel>(supplier);

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
