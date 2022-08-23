using Parth_Traders.Domain.Entity.User;
using Parth_Traders.Domain.Enums;
using Parth_Traders.Domain.RepositoryInterfaces.User;
using Parth_Traders.Service.Filter;
using Parth_Traders.Service.Services.User.UserInterface;

namespace Parth_Traders.Service.Services.User
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository ??
                throw new ArgumentNullException(nameof(orderRepository));
        }

        public Order AddOrder(Order orderToAdd)
        {
            try
            {
                _orderRepository.AddOrder(orderToAdd);
                _orderRepository.Save();
            }
            catch (Exception ex)
            {
                throw new BadRequestException("Please enter data in correct format!");
            }

            return GetLatestOrderForCustomer(orderToAdd.Customer.CustomerName);
        }

        public Order GetOrderById(long orderId)
        {
            var orderToReturn = _orderRepository.GetOrderById(orderId);
            if (orderToReturn == null)
            {
                throw new NotFoundException("Please enter a valid order id!");
            }
            return orderToReturn;
        }

        public Order GetLatestOrderForCustomer(string customerName)
        {
            return _orderRepository.GetLatestOrderForCustomer(customerName);
        }

        public List<Order> GetAllOrdersForCustomer(string customerName)
        {
            return _orderRepository.GetAllOrdersForCustomer(customerName);
        }

        public List<Order> GetAllOrdersForCustomerWithStatus(string customerName,
                                                             OrderStatus orderStatus)
        {
            var ordersToReturn = _orderRepository
                .GetAllOrdersForCustomerWithStatus(customerName, orderStatus);
            if (ordersToReturn == null)
            {
                throw new BadRequestException($"No orders found for {customerName} with {orderStatus}");
            }
            return ordersToReturn;
        }

        //TODO: need to refactor order details based on behavior of further implementation.
        public void CancelOrder(long orderId)
        {
            try
            {
                var orderToRemove = GetOrderById(orderId);
                _orderRepository.CancelOrder(orderToRemove);
                _orderRepository.Save();
            }
            catch (Exception ex)
            {
                throw new NotFoundException("Please enter a valid order id!");
            }
        }
    }
}
