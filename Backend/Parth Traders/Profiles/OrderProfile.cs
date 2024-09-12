using AutoMapper;
using Parth_Traders.Domain.Entity.User;
using Parth_Traders.Dto.User;

namespace Parth_Traders.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderDto, Order>()
                .ForPath(dest => dest.Customer.CustomerName,
                    opt => opt.MapFrom(src => src.CustomerName));

            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.CustomerName,
                    opt => opt.MapFrom(src => src.Customer.CustomerName));
        }
    }
}
