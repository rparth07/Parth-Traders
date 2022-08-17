using Parth_Traders.Domain.Entity;
using Parth_Traders.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Domain.RepositoryInterfaces.User
{
    public interface IOrderRepository
    {
        Order AddOrder(Order orderToAdd);
        Order GetOrderById(long orderId);
        Order GetLatestOrderForCustomer(string customerName);
        List<Order> GetAllOrdersForCustomer(string customerName);
        List<Order> GetAllOrdersForCustomerWithStatus(string customerName, OrderStatus orderStatus);
        void DeleteOrder(Order orderToRemove);
        bool Save();
        void Dispose();
    }
}
