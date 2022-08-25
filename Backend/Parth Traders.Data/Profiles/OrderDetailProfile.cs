using AutoMapper;
using Parth_Traders.Data.DataModel.User;
using Parth_Traders.Domain.Entity.User;

namespace Parth_Traders.Data.Profiles
{
    public class OrderDetailProfile : Profile
    {
        public OrderDetailProfile()
        {
            CreateMap<OrderDetail, OrderDetailDataModel>()
                .ForMember(dest => dest.OrderId,
                    opt => opt.MapFrom(src => src.Order.OrderId))
                .ForMember(dest => dest.ProductId,
                    opt => opt.MapFrom(src => src.Product.ProductId));

            CreateMap<OrderDetailDataModel, OrderDetail>()
                .ForMember(dest => dest.Product,
                    opt => opt.MapFrom(src => src.ProductData))
                .ForMember(dest => dest.Order,
                    opt => opt.MapFrom(src => src.OrderData));
        }
    }
}
