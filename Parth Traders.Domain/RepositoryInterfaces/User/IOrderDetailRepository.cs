using Parth_Traders.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Domain.RepositoryInterfaces.User
{
    public interface IOrderDetailRepository
    {
        void AddOrderDetail(OrderDetail orderDetailToAdd);
        void AddAllOrderDetails(List<OrderDetail> orderDetailsToAdd);
        List<OrderDetail> GetOrderDetailsForOrderId(long orderId);
        bool Save();
        void Dispose();
    }
}
