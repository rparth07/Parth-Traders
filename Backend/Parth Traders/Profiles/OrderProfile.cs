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
                .ForPath(dest => dest.Customer.UserName,
                    opt => opt.MapFrom(src => src.UserName));

            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.Customer.UserName));
        }
    }
}
