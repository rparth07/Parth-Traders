using Parth_Traders.Domain.Entity;
using Parth_Traders.Domain.RepositoryInterfaces.User;
using Parth_Traders.Service.Services.User.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Service.Services.User
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepository;

        public OrderDetailService(IOrderDetailRepository orderDetailRepository)
        {
            _orderDetailRepository = orderDetailRepository ??
                throw new ArgumentNullException(nameof(orderDetailRepository));
        }

        public List<OrderDetail> AddAllOrderDetails(List<OrderDetail> orderDetailsToAdd)
        {
            try
            {
                _orderDetailRepository.AddAllOrderDetails(orderDetailsToAdd);
                _orderDetailRepository.Save();
            }
            catch (Exception ex)
            {
                //temp solution
                throw new Exception($"Please enter data in correct format.{ex}");
            }

            return orderDetailsToAdd;
        }

        public OrderDetail AddOrderDetail(OrderDetail orderDetailToAdd)
        {
            try
            {
                _orderDetailRepository.AddOrderDetail(orderDetailToAdd);
                _orderDetailRepository.Save();
            }
            catch (Exception ex)
            {
                //temp solution
                throw new Exception($"Please enter data in correct format.{ex}");
            }

            return orderDetailToAdd;
        }

        public List<OrderDetail> GetOrderDetailsForOrderId(long orderId)
        {
            return _orderDetailRepository.GetOrderDetailsForOrderId(orderId);
        }
    }
}
