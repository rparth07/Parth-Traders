using Parth_Traders.Domain.Entity.User;
using Parth_Traders.Domain.Enums;

namespace Parth_Traders.Domain.RepositoryInterfaces.User
{
    public interface IOrderRepository
    {
        void AddOrder(Order orderToAdd);
        Order GetOrderById(long orderId);
        Order GetLatestOrderForCustomer(string customerName);
        List<Order> GetAllOrdersForCustomer(string customerName);
        List<Order> GetAllOrdersForCustomerWithStatus(string customerName, OrderStatus orderStatus);
        void CancelOrder(Order orderToCancel);
        bool Save();
        void Dispose();
    }
}
