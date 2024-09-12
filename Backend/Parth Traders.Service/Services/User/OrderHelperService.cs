using Parth_Traders.Domain.Entity.User;
using Parth_Traders.Domain.RepositoryInterfaces.AdminInterfaces;
using Parth_Traders.Domain.RepositoryInterfaces.User;
using Parth_Traders.Service.Services.User.UserInterface;

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
                _.Product = _productRepository.GetProductBySku(_.Product.ProductSku);
            });

            return order;
        }
    }
}
