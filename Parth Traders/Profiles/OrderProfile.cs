using AutoMapper;
using Parth_Traders.Domain.Entity;
using Parth_Traders.Dto.Admin;

namespace Parth_Traders.Data.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.CustomerName,
                    opt => opt.MapFrom(src => src.Customer.CustomerName));
            CreateMap<OrderDto, Order>()
                .ForPath(dest => dest.Customer.CustomerName,
                    opt => opt.MapFrom(src => src.CustomerName));
        }
    }
}
