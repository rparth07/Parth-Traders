using Parth_Traders.Domain.Entity.User;
using Parth_Traders.Domain.RepositoryInterfaces.User;
using Parth_Traders.Service.Filter;
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
                throw new BadRequestException("Please enter data in correct format!");
            }

            return GetCustomerByUserName(customerToAdd.UserName);
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
                throw new BadRequestException("One or more customer data is in wrong format. Please enter data in correct format!");
            }

            List<Customer> addedCustomers = customersToAdd
                .Select(_ => GetCustomerByUserName(_.UserName))
                .ToList();

            return addedCustomers;
        }

        public Customer GetCustomerById(long customerId)
        {
            var customerToReturn = _customerRepository.GetCustomerById(customerId);
            if (customerToReturn == null)
            {
                throw new NotFoundException("Please enter a valid customer id!");
            }
            return customerToReturn;
        }

        public Customer GetCustomerByUserName(string userName)
        {
            var customerToReturn = _customerRepository.GetCustomerByUserName(userName);
            if (customerToReturn == null)
            {
                throw new NotFoundException("Please enter a valid username!");
            }
            return customerToReturn;
        }

        public List<Customer> GetAllCustomers()
        {
            return _customerRepository.GetAllCustomers();
        }

        public void UpdateCustomer(Customer updatedCustomer, string oldUserName)
        {
            var customerFromRepo = GetCustomerByUserName(oldUserName);

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

        public void DeleteCustomer(string userName)
        {
            try
            {
                var customerFromRepo = GetCustomerByUserName(userName);
                _customerRepository.DeleteCustomer(customerFromRepo);
                _customerRepository.Save();
            }
            catch (Exception ex)
            {
                throw new NotFoundException("Please enter a valid customer name!");
            }
        }
    }
}
