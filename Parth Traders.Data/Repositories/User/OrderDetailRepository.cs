using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Parth_Traders.Data.DataModel;
using Parth_Traders.Domain.Entity;
using Parth_Traders.Domain.RepositoryInterfaces.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parth_Traders.Data.Repositories.User
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly ParthTradersContext _context;
        private readonly IMapper _mapper;

        public OrderDetailRepository(ParthTradersContext context, IMapper mapper)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public void AddAllOrderDetails(List<OrderDetail> orderDetails)
        {
            var orderDetailsToAdd = _mapper.Map<List<OrderDetailDataModel>>(orderDetails);

            _context.OrderDetails.AddRange(orderDetailsToAdd);
        }

        public void AddOrderDetail(OrderDetail orderDetail)
        {
            var orderDetailToAdd = _mapper.Map<OrderDetailDataModel>(orderDetail);

            _context.OrderDetails.Add(orderDetailToAdd);
        }

        public List<OrderDetail> GetOrderDetailsForOrderId(long orderId)
        {
            var orderDetails = _context.OrderDetails
                .Include(_ => _.OrderData)
                .ThenInclude(_ => _.CustomerData)
                .Include("ProductData")
                .AsNoTracking()
                .Where(_ => _.OrderId == orderId);

            return _mapper.Map<List<OrderDetail>>(orderDetails);
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
