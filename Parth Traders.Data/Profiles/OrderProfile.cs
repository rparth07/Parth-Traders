using AutoMapper;
using Parth_Traders.Data.DataModel;
using Parth_Traders.Domain.Entity;

namespace Parth_Traders.Data.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDataModel>()
                .ForMember(dest => dest.CustomerId,
                    opt => opt.MapFrom(src => src.Customer.CustomerId));
            CreateMap<OrderDataModel, Order>()
                .ForMember(dest => dest.Customer,
                    opt => opt.MapFrom(src => src.CustomerData))
                .ForMember(dest => dest.OrderDetails,
                    opt => opt.MapFrom(src => src.OrderDetails));
        }
    }
}
