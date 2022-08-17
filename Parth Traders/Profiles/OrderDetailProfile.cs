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
                    opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.OrderId,
                    opt => opt.MapFrom(src => src.Order.OrderId))
                .ForMember(dest => dest.CustomerName,
                    opt => opt.MapFrom(src => src.Order.Customer.CustomerName));
            CreateMap<OrderDetailDto, OrderDetail>()
                .ForPath(dest => dest.Order.Customer.CustomerName,
                    opt => opt.MapFrom(src => src.CustomerName))
                .ForPath(dest => dest.Product.ProductName,
                    opt => opt.MapFrom(src => src.ProductName));
        }
    }
}
