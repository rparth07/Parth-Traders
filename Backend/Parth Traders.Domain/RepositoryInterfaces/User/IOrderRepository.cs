using Parth_Traders.Domain.Entity.User;
using Parth_Traders.Domain.Enums;

namespace Parth_Traders.Domain.RepositoryInterfaces.User
{
    public interface IOrderRepository
    {
        void AddOrder(Order orderToAdd);
        Order GetOrderById(long orderId);
        Order GetLatestOrderForCustomer(string userName);
        List<Order> GetAllOrdersForCustomer(string userName);
        List<Order> GetAllOrdersForCustomerWithStatus(string userName, OrderStatus orderStatus);
        void CancelOrder(Order orderToCancel);
        bool Save();
        void Dispose();
    }
}
