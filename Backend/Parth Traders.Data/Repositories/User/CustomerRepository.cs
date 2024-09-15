﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Parth_Traders.Data.DataModel.User;
using Parth_Traders.Domain.Entity.User;
using Parth_Traders.Domain.RepositoryInterfaces.User;

namespace Parth_Traders.Data.Repositories.User
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

        public void AddAllCustomers(List<Customer> customers)
        {
            var customerListToAdd = _mapper.Map<List<CustomerDataModel>>(customers);

            _context.Customers.AddRange(customerListToAdd);
        }

        public Customer GetCustomerById(long id)
        {
            var customer = _context.Customers
                .Include("Orders")
                .AsNoTracking()
                .FirstOrDefault(_ => _.Id == id.ToString());

            return _mapper.Map<Customer>(customer);
        }

        public Customer GetCustomerByUserName(string userName)
        {
            var customer = _context.Customers
                .Include("Orders")
                .AsNoTracking()
                .FirstOrDefault(_ => _.UserName == userName);

            return _mapper.Map<Customer>(customer);
        }

        public List<Customer> GetAllCustomers()
        {
            var customers = _context.Customers
                .Include("Orders")
                .ToList();

            var customersToReturn = _mapper.Map<List<Customer>>(customers);
            return customersToReturn;
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
