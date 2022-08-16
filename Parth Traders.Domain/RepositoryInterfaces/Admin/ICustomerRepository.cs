using Parth_Traders.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Domain.RepositoryInterfaces.Admin
{
    public interface ICustomerRepository
    {
        void AddCustomer(Customer customerToAdd);
        void AddAllCustomers(List<Customer> customersToAdd);
        Customer GetCustomerById(long customerId);
        Customer GetCustomerByName(string customerName);
        List<Customer> GetAllCustomers();
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(Customer customerToDelete);
        bool Save();
        void Dispose();
    }
}
