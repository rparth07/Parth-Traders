using Parth_Traders.Domain.Entity.User;

namespace Parth_Traders.Service.Services.User.UserInterface
{
    public interface ICustomerService
    {
        Customer AddCustomer(Customer customerToAdd);
        List<Customer> AddAllCustomers(List<Customer> customersToAdd);
        Customer GetCustomerById(long customerId);
        Customer GetCustomerByName(string customerName);
        List<Customer> GetAllCustomers();
        void UpdateCustomer(Customer updatedCustomer, string oldCustomerName);
        void DeleteCustomer(string customerName);
    }
}
