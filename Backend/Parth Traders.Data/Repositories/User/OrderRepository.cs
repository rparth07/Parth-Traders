using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Parth_Traders.Data.DataModel.User;
using Parth_Traders.Domain.Entity.User;
using Parth_Traders.Domain.Enums;
using Parth_Traders.Domain.RepositoryInterfaces.User;

namespace Parth_Traders.Data.Repositories.User
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ParthTradersContext _context;
        private readonly IMapper _mapper;

        public OrderRepository(ParthTradersContext context, IMapper mapper)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public void AddOrder(Order order)
        {
            var orderToAdd = _mapper.Map<OrderDataModel>(order);

            _context.Orders.Add(orderToAdd);
        }

        public Order GetOrderById(long orderId)
        {
            var orderToReturn = _context.Orders
                .Include("CustomerData")
                .Include("OrderDetails.ProductData")
                .AsNoTracking()
                .FirstOrDefault(_ => _.OrderId == orderId);

            return _mapper.Map<Order>(orderToReturn);
        }

        public Order GetLatestOrderForCustomer(string userName)
        {
            var orderToReturn = _context.Orders
                .Include("CustomerData")
                .Include("OrderDetails.ProductData")
                .AsNoTracking()
                .Where(_ => _.CustomerData.UserName == userName)
                .OrderByDescending(_ => _.OrderDate)
                .FirstOrDefault();

            return _mapper.Map<Order>(orderToReturn);
        }

        public List<Order> GetAllOrdersForCustomer(string userName)
        {
            var ordersToReturn = _context.Orders
                .Include("CustomerData")
                .Include("OrderDetails.ProductData")
                .AsNoTracking()
                .Where(_ => _.CustomerData.UserName == userName);

            var orders = _mapper.Map<List<Order>>(ordersToReturn);
            return orders;
        }

        public List<Order> GetAllOrdersForCustomerWithStatus(string userName,
                                                             OrderStatus orderStatus)
        {
            var ordersToReturn = _context.Orders
                .Include("CustomerData")
                .Include("OrderDetails.ProductData")
                .AsNoTracking()
                .Where(_ => _.CustomerData.UserName == userName &&
                       _.OrderStatus == orderStatus);

            return _mapper.Map<List<Order>>(ordersToReturn);
        }

        public void CancelOrder(Order order)
        {
            var orderToRemove = _mapper.Map<OrderDataModel>(order);

            orderToRemove.OrderStatus = OrderStatus.CanceledByCustomer;

            _context.Orders.Update(orderToRemove);
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose resources when needed
            }
        }
    }
}
