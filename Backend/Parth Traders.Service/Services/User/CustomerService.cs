using Parth_Traders.Domain.Entity.User;
using Parth_Traders.Domain.RepositoryInterfaces.User;
using Parth_Traders.Service.Filter;
using Parth_Traders.Service.Services.Logger;
using Parth_Traders.Service.Services.User.UserInterface;

namespace Parth_Traders.Service.Services.User
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILoggerManager _logger;

        public CustomerService(ICustomerRepository customerRepository, ILoggerManager logger)
        {
            _customerRepository = customerRepository ??
                throw new ArgumentNullException(nameof(customerRepository));
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
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
                _logger.LogInfo("Entered data was in incorrect format and throwing error" +
                    $"{ex}");
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
                _logger.LogInfo("CSV file data is in incorrect format and throwing error" +
                    $"{ex}");
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
                _logger.LogInfo("Invalid product id was entered");
                throw new NotFoundException("Please enter a valid customer id!");
            }
            return customerToReturn;
        }

        public Customer GetCustomerByUserName(string userName)
        {
            var customerToReturn = _customerRepository.GetCustomerByUserName(userName);
            if (customerToReturn == null)
            {
                _logger.LogInfo("Invalid product name was entered");
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
                _logger.LogInfo("Invalid product name was entered and throwing error" +
                    $"{ex}");
                throw new NotFoundException("Please enter a valid customer name!");
            }
        }
    }
}
