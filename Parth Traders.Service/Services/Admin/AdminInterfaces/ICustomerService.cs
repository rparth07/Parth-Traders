using Parth_Traders.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Service.Services.Admin.AdminInterfaces
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
