using Parth_Traders.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Service.Services.User.UserInterface
{
    public interface IOrderDetailService
    {
        OrderDetail AddOrderDetail(OrderDetail orderDetailToAdd);
        List<OrderDetail> AddAllOrderDetails(List<OrderDetail> orderDetailsToAdd);
        List<OrderDetail> GetOrderDetailsForOrderId(long orderId);
    }
}
