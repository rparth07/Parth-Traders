using Parth_Traders.Domain.Entity.User;

namespace Parth_Traders.Domain.RepositoryInterfaces.User
{
    public interface ICustomerRepository
    {
        void AddCustomer(Customer customerToAdd);
        void AddAllCustomers(List<Customer> customersToAdd);
        Customer GetCustomerById(long customerId);
        Customer GetCustomerByUserName(string userName);
        List<Customer> GetAllCustomers();
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(Customer customerToDelete);
        bool Save();
        void Dispose();
    }
}
