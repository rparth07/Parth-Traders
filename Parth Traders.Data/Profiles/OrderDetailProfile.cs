using AutoMapper;
using Parth_Traders.Data.DataModel;
using Parth_Traders.Domain.Entity;

namespace Parth_Traders.Data.Profiles
{
    public class OrderDetailProfile : Profile
    {
        public OrderDetailProfile()
        {
            CreateMap<OrderDetail, OrderDetailDataModel>();
            CreateMap<OrderDetailDataModel, OrderDetail>();
        }
    }
}
