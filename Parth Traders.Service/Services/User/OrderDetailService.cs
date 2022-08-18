using Parth_Traders.Domain.Entity.User;
using Parth_Traders.Domain.RepositoryInterfaces.User;
using Parth_Traders.Service.Services.User.UserInterface;

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
            //TODO: Need to find a way to get addedOrderDetail, right now I am not using any method in this service.
            return orderDetailToAdd;
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

            List<OrderDetail> addedOrderDetails =
                GetOrderDetailsForOrderId(orderDetailsToAdd.FirstOrDefault().Order.OrderId);

            return addedOrderDetails;
        }

        public List<OrderDetail> GetOrderDetailsForOrderId(long orderId)
        {
            return _orderDetailRepository.GetOrderDetailsForOrderId(orderId);
        }
    }
}
