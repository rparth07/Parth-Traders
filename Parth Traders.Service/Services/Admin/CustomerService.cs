using Parth_Traders.Domain.Entity;
using Parth_Traders.Domain.RepositoryInterfaces.Admin;
using Parth_Traders.Service.Services.Admin.AdminInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Service.Services.Admin
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository ??
                throw new ArgumentNullException(nameof(customerRepository));
        }

        public List<Customer> AddAllCustomers(List<Customer> customersToAdd)
        {
            try
            {
                _customerRepository.AddAllCustomers(customersToAdd);
                _customerRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"Bad Request {ex}");
            }

            return customersToAdd;
        }

        public Customer AddCustomer(Customer customerToAdd)
        {
            try
            {
                _customerRepository.AddCustomer(customerToAdd);
                _customerRepository.Save();
            }
            catch (Exception ex)
            {
                //temp solution
                throw new Exception($"Please enter data in correct format.{ex}");
            }

            return customerToAdd;
        }

        public void DeleteCustomer(string customerName)
        {
            try
            {
                var customerFromRepo = _customerRepository.GetCustomerByName(customerName);
                _customerRepository.DeleteCustomer(customerFromRepo);
                _customerRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Customer not found!");
            }
        }

        public List<Customer> GetAllCustomers()
        {
            return _customerRepository.GetAllCustomers();
        }

        public Customer GetCustomerById(long customerId)
        {
            var customerToReturn = _customerRepository.GetCustomerById(customerId);
            if (customerToReturn == null)
            {
                throw new Exception("Customer not found!");
            }
            return customerToReturn;
        }

        public Customer GetCustomerByName(string customerName)
        {
            var customerToReturn = _customerRepository.GetCustomerByName(customerName);
            if (customerToReturn == null)
            {
                throw new Exception("Customer not found!");
            }
            return customerToReturn;
        }

        public void UpdateCustomer(Customer updatedCustomer, string oldCustomerName)
        {
            var customerFromRepo = GetCustomerByName(oldCustomerName);

            _customerRepository
                .UpdateCustomer(FillRequiredInfo(customerFromRepo, updatedCustomer));
        }

        private static Customer FillRequiredInfo(Customer customerFromRepo,
                                                 Customer customer)
        {
            customer.CustomerId = customerFromRepo.CustomerId;
            customer.CreatedDate = customerFromRepo.CreatedDate;
            customer.Orders = customerFromRepo.Orders;

            return customer;
        }
    }
}
