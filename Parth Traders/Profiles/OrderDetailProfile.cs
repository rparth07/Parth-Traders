using AutoMapper;
using Parth_Traders.Domain.Entity;
using Parth_Traders.Dto.Admin;

namespace Parth_Traders.Data.Profiles
{
    public class OrderDetailProfile : Profile
    {
        public OrderDetailProfile()
        {
            CreateMap<OrderDetail, OrderDetailDto>()
                .ForMember(dest => dest.ProductName,
                    opt => opt.MapFrom(src => src.ProductData.ProductName))
                .ForMember(dest => dest.CustomerName,
                    opt => opt.MapFrom(src => src.OrderData.CustomerData.CustomerName));
            CreateMap<OrderDetailDto, OrderDetail>();
        }
    }
}
