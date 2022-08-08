using AutoMapper;
using Parth_Traders.Domain.Entity;
using Parth_Traders.Dto.Admin;

namespace Parth_Traders.Data.Profiles
{
    public class OrderDetailProfile : Profile
    {
        public OrderDetailProfile()
        {
            CreateMap<OrderDetail, OrderDetailDto>();
            CreateMap<OrderDetailDto, OrderDetail>();
        }
    }
}
