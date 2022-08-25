using Parth_Traders.Domain.Entity.User;

namespace Parth_Traders.Service.Services.User.UserInterface
{
    public interface IOrderDetailService
    {
        OrderDetail AddOrderDetail(OrderDetail orderDetailToAdd);
        List<OrderDetail> AddAllOrderDetails(List<OrderDetail> orderDetailsToAdd);
        List<OrderDetail> GetOrderDetailsForOrderId(long orderId);
    }
}
