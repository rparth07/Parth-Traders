using AutoMapper;
using Parth_Traders.Domain.Entity.User;
using Parth_Traders.Dto.User;

namespace Parth_Traders.Profiles
{
    public class OrderDetailProfile : Profile
    {
        public OrderDetailProfile()
        {
            CreateMap<OrderDetailDto, OrderDetail>()
                .ForPath(dest => dest.Product.ProductSku,
                    opt => opt.MapFrom(src => src.ProductSku));

            CreateMap<OrderDetail, OrderDetailDto>()
                .ForMember(dest => dest.ProductSku,
                    opt => opt.MapFrom(src => src.Product.ProductSku))
                .ForMember(dest => dest.OrderId,
                    opt => opt.MapFrom(src => src.Order.OrderId));
        }
    }
}
