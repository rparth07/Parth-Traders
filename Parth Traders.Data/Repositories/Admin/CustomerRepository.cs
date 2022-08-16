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
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ParthTradersContext _context;
        private readonly IMapper _mapper;

        public CustomerRepository(ParthTradersContext context, IMapper mapper)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public void AddCustomer(Customer customer)
        {
            var customerToAdd = _mapper.Map<CustomerDataModel>(customer);

            _context.Customers.Add(customerToAdd);
        }

        public void AddAllCustomers(List<Customer> categories)
        {
            var customerListToAdd = _mapper.Map<List<CustomerDataModel>>(categories);

            _context.Customers.AddRange(customerListToAdd);
        }

        public Customer GetCustomerByName(string customerName)
        {
            var customer = _context.Customers
                .Include("Orders")
                .AsNoTracking()
                .FirstOrDefault(_ => _.CustomerName == customerName);

            return _mapper.Map<Customer>(customer);
        }

        public Customer GetCustomerById(long id)
        {
            var customer = _context.Customers
                .Include("Orders")
                .AsNoTracking()
                .FirstOrDefault(_ => _.CustomerId == id);

            return _mapper.Map<Customer>(customer);
        }

        public List<Customer> GetAllCustomers()
        {
            var categories = _context.Customers
                .Include("Orders")
                .ToList();

            return _mapper.Map<List<Customer>>(categories);
        }

        public void UpdateCustomer(Customer customer)
        {
            //TODO: admin can not update orders.
            _context.Customers.Update(_mapper.Map<CustomerDataModel>(customer));

            Save();
        }

        public void DeleteCustomer(Customer customer)
        {
            var customerToDelete = _mapper.Map<CustomerDataModel>(customer);

            _context.Orders.RemoveRange(customerToDelete.Orders);
            _context.Customers.Remove(customerToDelete);
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
