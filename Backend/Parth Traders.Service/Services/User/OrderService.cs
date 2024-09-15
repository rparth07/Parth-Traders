using Parth_Traders.Domain.Entity.User;
using Parth_Traders.Domain.Enums;
using Parth_Traders.Domain.RepositoryInterfaces.User;
using Parth_Traders.Service.Filter;
using Parth_Traders.Service.Services.Logger;
using Parth_Traders.Service.Services.User.UserInterface;

namespace Parth_Traders.Service.Services.User
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILoggerManager _logger;

        public OrderService(IOrderRepository orderRepository, ILoggerManager logger)
        {
            _orderRepository = orderRepository ??
                throw new ArgumentNullException(nameof(orderRepository));
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
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
                _logger.LogInfo("Entered data was in incorrect format and throwing error" +
                    $"{ex}");
                throw new BadRequestException("Please enter data in correct format!");
            }

            return GetLatestOrderForCustomer(orderToAdd.Customer.UserName);
        }

        public Order GetOrderById(long orderId)
        {
            var orderToReturn = _orderRepository.GetOrderById(orderId);
            if (orderToReturn == null)
            {
                _logger.LogInfo("Invalid product id was entered");
                throw new NotFoundException("Please enter a valid order id!");
            }
            return orderToReturn;
        }

        public Order GetLatestOrderForCustomer(string userName)
        {
            return _orderRepository.GetLatestOrderForCustomer(userName);
        }

        public List<Order> GetAllOrdersForCustomer(string userName)
        {
            return _orderRepository.GetAllOrdersForCustomer(userName);
        }

        public List<Order> GetAllOrdersForCustomerWithStatus(string userName,
                                                             OrderStatus orderStatus)
        {
            var ordersToReturn = _orderRepository
                .GetAllOrdersForCustomerWithStatus(userName, orderStatus);
            if (ordersToReturn == null)
            {
                _logger.LogInfo($"No orders found for {userName} with {orderStatus}");
                throw new BadRequestException($"No orders found for {userName} with {orderStatus}");
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
                _logger.LogInfo("Invalid product id was entered and throwing error" +
                    $"{ex}");
                throw new NotFoundException("Please enter a valid order id!");
            }
        }
    }
}
