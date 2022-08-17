using Parth_Traders.Domain.Entity.User;
using Parth_Traders.Domain.RepositoryInterfaces.User;
using Parth_Traders.Service.Services.User.UserInterface;

namespace Parth_Traders.Service.Services.User
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository ??
                throw new ArgumentNullException(nameof(customerRepository));
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

        public List<Customer> GetAllCustomers()
        {
            return _customerRepository.GetAllCustomers();
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
    }
}
