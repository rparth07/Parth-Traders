using AutoMapper;
using Parth_Traders.Data.DataModel;
using Parth_Traders.Domain.Entity;

namespace Parth_Traders.Data.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDataModel>();
            CreateMap<OrderDataModel, Order>();
        }
    }
}
