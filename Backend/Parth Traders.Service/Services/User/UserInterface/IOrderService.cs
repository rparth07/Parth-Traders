using Parth_Traders.Domain.Entity.User;
using Parth_Traders.Domain.Enums;

namespace Parth_Traders.Service.Services.User.UserInterface
{
    public interface IOrderService
    {
        Order AddOrder(Order orderToAdd);
        Order GetOrderById(long orderId);
        Order GetLatestOrderForCustomer(string userName);
        List<Order> GetAllOrdersForCustomer(string userName);
        List<Order> GetAllOrdersForCustomerWithStatus(string userName, OrderStatus orderStatus);
        void CancelOrder(long orderId);
    }
}
