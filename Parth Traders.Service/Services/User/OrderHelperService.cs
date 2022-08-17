using Parth_Traders.Domain.Entity;
using Parth_Traders.Domain.RepositoryInterfaces.Admin;
using Parth_Traders.Domain.RepositoryInterfaces.AdminInterfaces;
using Parth_Traders.Service.Services.User.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Service.Services.User
{
    public class OrderHelperService : IOrderHelperService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;

        public OrderHelperService(ICustomerRepository customerRepository,
                                  IProductRepository productRepository)
        {
            _customerRepository = customerRepository ?? 
                throw new ArgumentNullException(nameof(customerRepository));

            _productRepository = productRepository ??
                throw new ArgumentNullException(nameof(productRepository));
        }

        public Order MapOrderPropertiesToOrder(Order order)
        {
            order.Customer = _customerRepository.GetCustomerByName(order.Customer.CustomerName);
            
            order.OrderDetails.ForEach(_ =>
            {
                _.Product = _productRepository.GetProductByName(_.Product.ProductName);
            });

            return order;
        }
    }
}
