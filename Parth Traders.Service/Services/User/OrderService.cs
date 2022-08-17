using Parth_Traders.Domain.Entity;
using Parth_Traders.Domain.Enums;
using Parth_Traders.Domain.RepositoryInterfaces.User;
using Parth_Traders.Service.Services.User.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Service.Services.User
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailService _orderDetailService;

        public OrderService(IOrderRepository orderRepository, IOrderDetailService orderDetailService)
        {
            _orderRepository = orderRepository ??
                throw new ArgumentNullException(nameof(orderRepository));
            
            _orderDetailService = orderDetailService ??
                throw new ArgumentNullException(nameof(orderDetailService));
        }

        public Order AddOrder(Order orderToAdd)
        {
            try
            {
                var addedOrder = _orderRepository.AddOrder(orderToAdd);
                _orderRepository.Save();
            }
            catch (Exception ex)
            {
                //temp solution
                throw new Exception($"Please enter data in correct format.{ex}");
            }

            return orderToAdd;
        }

        public void DeleteOrder(long orderId)
        {
            try
            {
                var orderToRemove = _orderRepository.GetOrderById(orderId);
                _orderRepository.DeleteOrder(orderToRemove);
                _orderRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Order not found!");
            }
        }

        public List<Order> GetAllOrdersForCustomer(string customerName)
        {
            return _orderRepository.GetAllOrdersForCustomer(customerName);
        }

        public List<Order> GetAllOrdersForCustomerWithStatus(string customerName,
                                                             OrderStatus orderStatus)
        {
            var ordersToReturn = _orderRepository
                .GetAllOrdersForCustomerWithStatus(customerName,orderStatus);
            if (ordersToReturn == null)
            {
                throw new Exception($"No orders found for {customerName} with {orderStatus}");
            }
            return ordersToReturn;
        }

        public Order GetLatestOrderForCustomer(string customerName)
        {
            return _orderRepository.GetLatestOrderForCustomer(customerName);
        }

        public Order GetOrderById(long orderId)
        {
            var orderToReturn = _orderRepository.GetOrderById(orderId);
            if (orderToReturn == null)
            {
                throw new Exception("Order not found!");
            }
            return orderToReturn;
        }
    }
}
